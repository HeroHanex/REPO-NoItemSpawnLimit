using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace NoItemSpawnLimit.Patches;

public class ConfigManager
{
    private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("NoItemSpawnLimit");
    private static ConfigFile? _config;
    private static ConfigEntry<bool>? RemoveSpawnLimit;
    private static readonly Dictionary<string, ConfigEntry<int>> itemMaxAmountConfigs = new();
    private const int defaultMaxAmount = 10000;

    public static void Init(ConfigFile config)
    {
        _config = config;
        RemoveSpawnLimit = config.Bind("General", "RemoveSpawnLimit", true, "Remove item spawn limit");
        Logger.LogInfo("ConfigManager initialized.");
        config.Save();
    }

    public static void LoadMaxItemConfig()
    {
        if (_config == null)
        {
            Logger.LogError("ConfigManager: _config is null. Init() wasn't called.");
            return;
        }

        if (StatsManager.instance == null || StatsManager.instance.itemDictionary.Count == 0)
        {
            Logger.LogWarning("StatsManager or itemDictionary is not ready. It should be ready once loaded into a level.");
            return;
        }

        foreach (var kvp in StatsManager.instance.itemDictionary)
        {
            if (!itemMaxAmountConfigs.ContainsKey(kvp.Key))
            {
                int itemMaxAmount = kvp.Value.maxAmount;
                var entry = _config.Bind(
                    "ItemLimits",
                    kvp.Key,
                    itemMaxAmount,
                    new ConfigDescription(
                        $"Max amount for item '{kvp.Key}'",
                        new AcceptableValueRange<int>(0, defaultMaxAmount)
                    )
                );
                itemMaxAmountConfigs[kvp.Key] = entry;
            }
        }

        if (RemoveSpawnLimit == null)
        {
            RemoveSpawnLimit = _config.Bind("General", "RemoveSpawnLimit", true, "Remove item spawn limit");
            Logger.LogInfo("RemoveSpawnLimit config was null. Created RemoveSpawnLimit config.");
        }

        _config.Save();

        if (RemoveSpawnLimit.Value)
        {
            Logger.LogInfo($"RemoveSpawnLimit is enabled. Applying predefined config of {defaultMaxAmount} to all items...");
            ApplyPredefinedConfig();
        }
        else
        {
            Logger.LogInfo("RemoveSpawnLimit is disabled. Applying per-item config...");
            ApplyItemConfig();
        }
    }

    private static void ApplyItemConfig()
    {
        foreach (var itemMaxAmountConfig in itemMaxAmountConfigs)
        {
            if (!StatsManager.instance.itemDictionary.TryGetValue(itemMaxAmountConfig.Key, out var value))
            {
                Logger.LogWarning($"Item '{itemMaxAmountConfig.Key}' not found in itemDictionary.");
                return;
            }

            value.maxAmount = itemMaxAmountConfig.Value.Value;
        }
    }

    private static void ApplyPredefinedConfig()
    {
        foreach (var item in StatsManager.instance.itemDictionary)
        {
            item.Value.maxAmount = defaultMaxAmount;
        }
    }
}