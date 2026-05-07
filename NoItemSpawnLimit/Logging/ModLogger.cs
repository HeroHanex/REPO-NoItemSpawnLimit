using BepInEx.Logging;

namespace NoItemSpawnLimit.Logging;

public enum ModLogLevel
{
    Error,
    Warning,
    Info,
    Debug
}

public static class ModLogger
{
    private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("NoItemSpawnLimit");

    private static bool CanLog(ModLogLevel level)
    {
        return level <= Patches.ConfigManager.CurrentLogLevel;
    }

    public static void LogFatal(object data)
    {
        if (CanLog(ModLogLevel.Error))
        {
            Logger.LogFatal(data);
        }
    }

    public static void LogError(object data)
    {
        if (CanLog(ModLogLevel.Error))
        {
            Logger.LogError(data);
        }
    }

    public static void LogWarning(object data)
    {
        if (CanLog(ModLogLevel.Warning))
        {
            Logger.LogWarning(data);
        }
    }

    public static void LogInfo(object data)
    {
        if (CanLog(ModLogLevel.Info))
        {
            Logger.LogInfo(data);
        }
    }

    public static void LogDebug(object data)
    {
        if (CanLog(ModLogLevel.Debug))
        {
            Logger.LogDebug(data);
        }
    }
}