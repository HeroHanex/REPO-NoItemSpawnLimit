using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using NoItemSpawnLimit.Logging;
using Logger = NoItemSpawnLimit.Logging.ModLogger;

namespace NoItemSpawnLimit.Patches;

public class ConfigManager
{
    private static ConfigFile? _config;
    private static ConfigEntry<bool>? RemoveSpawnLimit;
    private static readonly Dictionary<string, ConfigEntry<int>> itemMaxAmountConfigs = new();
    private static ConfigEntry<ModLogLevel>? LogLevel;
    public static ModLogLevel CurrentLogLevel => LogLevel?.Value ?? ModLogLevel.Info;
    private const int DefaultMaxAmount = 10000;
    private static readonly char[] invalidChars = { '=', '\n', '\t', '"', '\'', '[', ']' };

    public static void Init(ConfigFile config)
    {
        _config = config;
        RemoveSpawnLimit = config.Bind("General", "RemoveSpawnLimit", true, "Remove item spawn limit");
        LogLevel = config.Bind(
            "General",
            "LogLevel",
            ModLogLevel.Info,
            "Logging level"
        );
        Logger.LogDebug("ConfigManager initialized.");
        config.Save();
    }

    public static void LoadMaxItemConfig()
    {
        if (_config == null)
        {
            throw new InvalidOperationException("ConfigManager not initialized. Call Init() before loading config.");
        }

        if (StatsManager.instance == null || StatsManager.instance.itemDictionary.Count == 0)
        {
            Logger.LogDebug("StatsManager or itemDictionary is not ready. It should be ready once loaded into a level.");
            return;
        }

        foreach (var kvp in StatsManager.instance.itemDictionary)
        {
            if (!itemMaxAmountConfigs.ContainsKey(kvp.Key))
            {
                int itemMaxAmount = kvp.Value.maxAmount;
                string sanitizedKey = SanitizeString(kvp.Key);

                var entry = _config.Bind(
                    "ItemLimits",
                    sanitizedKey,
                    itemMaxAmount,
                    new ConfigDescription(
                        $"Max amount for item '{kvp.Key}'",
                        new AcceptableValueRange<int>(0, DefaultMaxAmount)
                    )
                );
                itemMaxAmountConfigs[kvp.Key] = entry;
            }
        }

        if (RemoveSpawnLimit == null)
        {
            throw new InvalidOperationException("RemoveSpawnLimit config entry is null. This should not happen if Init() was called correctly.");
        }

        _config.Save();

        if (RemoveSpawnLimit.Value)
        {
            Logger.LogInfo($"RemoveSpawnLimit is enabled. Applying predefined config of {DefaultMaxAmount} to all items...");
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
                continue;
            }

            value.maxAmount = itemMaxAmountConfig.Value.Value;
        }
    }

    private static void ApplyPredefinedConfig()
    {
        foreach (var item in StatsManager.instance.itemDictionary)
        {
            item.Value.maxAmount = DefaultMaxAmount;
        }
    }

    private static string SanitizeString(string input)
    {
        foreach (char c in invalidChars)
        {
            input = input.Replace(c, '_');
        }

        return input;
    }
}