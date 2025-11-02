using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Photon.Pun;
using BepInEx.Logging;

namespace NoItemSpawnLimit.Patches;

[HarmonyPatch(typeof(PunManager))]
public class PunManagerPatch
{
    private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("NoItemSpawnLimit");
    private const float SpawnOffsetFacing = 7.0f;
    private const float SpawnOffsetHeight = 2.0f;
    private static Vector3 SpawnOffset = new Vector3(0.0f, SpawnOffsetHeight, SpawnOffsetFacing);
    private static Vector3 SpawnOffsetLobby = new Vector3(SpawnOffsetFacing, SpawnOffsetHeight, 0.0f);

    [HarmonyPrefix, HarmonyPatch(nameof(PunManager.TruckPopulateItemVolumes))]
    static void TruckPopulateItemVolumesPatch(PunManager __instance)
    {
        ItemManager.instance.spawnedItems.Clear();

        if (SemiFunc.IsNotMasterClient())
        {
            return;
        }

        List<ItemVolume> shelfVolumes = __instance.itemManager.itemVolumes;
        List<Item> purchasedItems = __instance.itemManager.purchasedItems;

        Logger.LogInfo($"Initial shelfVolumes: {shelfVolumes.Count}, purchasedItems: {purchasedItems.Count}");

        // Fill shelves as normal
        while (shelfVolumes.Count > 0 && purchasedItems.Count > 0)
        {
            bool placed = false;
            for (int i = 0; i < purchasedItems.Count; i++)
            {
                Item item = purchasedItems[i];
                ItemVolume slot = shelfVolumes.Find(v => v.itemVolume == item.itemVolume);
                if (slot != null)
                {
                    __instance.SpawnItem(item, slot);
                    shelfVolumes.Remove(slot);
                    purchasedItems.RemoveAt(i);
                    placed = true;
                    break;
                }
            }
            if (!placed)
                break;
        }

        Logger.LogInfo($"After shelf fill: shelfVolumes: {shelfVolumes.Count}, purchasedItems: {purchasedItems.Count}");

        // Spawn remaining items at truck spawn points
        if (purchasedItems.Count > 0)
        {
            List<SpawnPoint> spawnPoints = Object.FindObjectsOfType<SpawnPoint>().ToList();
            spawnPoints.Shuffle();
            int spawnIndex = 0;

            foreach (Item item in purchasedItems.ToList())
            {
                // Ignore energy crystals
                if (item.itemType == SemiFunc.itemType.power_crystal)
                {
                    Logger.LogInfo($"Ignoring item: {item.itemName}");
                    continue;
                }

                // Spawn at a random spawn point
                // Offset is different for lobby
                Vector3 spawnPosition = spawnPoints[spawnIndex % spawnPoints.Count].transform.position;
                Vector3 offset = RunManager.instance.levelCurrent == RunManager.instance.levelLobby ? SpawnOffsetLobby : SpawnOffset;
                Vector3 adjustedPosition = spawnPosition + offset;

                SpawnItemAtPosition(item, adjustedPosition);
                purchasedItems.Remove(item);
                spawnIndex++;
            }
        }

        foreach (ItemVolume v in __instance.itemManager.itemVolumes)
        {
            Object.Destroy(v.gameObject);
        }
    }

    static void SpawnItemAtPosition(Item item, Vector3 position)
    {
        ShopManager.instance.itemRotateHelper.transform.parent = ShopManager.instance.transform;
        ShopManager.instance.itemRotateHelper.transform.localRotation = item.spawnRotationOffset;
        Quaternion rotation = ShopManager.instance.itemRotateHelper.transform.rotation;

        if (SemiFunc.IsMasterClient())
        {
            PhotonNetwork.InstantiateRoomObject(item.prefab.ResourcePath, position, rotation, 0);
        }
        else if (!SemiFunc.IsMultiplayer())
        {
            Object.Instantiate(item.prefab.Prefab, position, rotation);
        }
    }
}