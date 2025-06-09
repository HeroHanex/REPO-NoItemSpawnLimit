using HarmonyLib;
using BepInEx.Logging;

namespace NoItemSpawnLimit.Patches;

[HarmonyPatch(typeof(LevelGenerator))]
public class LevelGeneratorPatch
{
    private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("NoItemSpawnLimit");

    [HarmonyPostfix, HarmonyPatch(nameof(LevelGenerator.GenerateDone))]
    static void GenerateDonePatch(LevelGenerator __instance)
    {
        if (StatsManager.instance.itemDictionary == null || StatsManager.instance.itemDictionary.Count == 0)
        {
            return;
        }

        Logger.LogInfo("LevelGeneratorPatch: Level generation done. Loading item config...");
        ConfigManager.LoadMaxItemConfig();
    }
}
