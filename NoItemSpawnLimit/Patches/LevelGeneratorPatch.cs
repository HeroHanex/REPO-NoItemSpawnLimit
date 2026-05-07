using HarmonyLib;
using Logger = NoItemSpawnLimit.Logging.ModLogger;

namespace NoItemSpawnLimit.Patches;

[HarmonyPatch(typeof(LevelGenerator))]
public class LevelGeneratorPatch
{
    [HarmonyPostfix, HarmonyPatch(nameof(LevelGenerator.GenerateDone))]
    static void GenerateDonePatch(LevelGenerator __instance)
    {
        if (StatsManager.instance.itemDictionary == null || StatsManager.instance.itemDictionary.Count == 0)
        {
            return;
        }

        if (SemiFunc.IsNotMasterClient())
        {
            return;
        }

        Logger.LogDebug("LevelGeneratorPatch: Level generation done. Loading item config...");
        ConfigManager.LoadMaxItemConfig();
    }
}
