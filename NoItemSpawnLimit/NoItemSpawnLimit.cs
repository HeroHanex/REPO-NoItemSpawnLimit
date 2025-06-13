using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NoItemSpawnLimit.Patches;
using UnityEngine;

namespace NoItemSpawnLimit;

[BepInPlugin("HeroHanex.NoItemSpawnLimit", "NoItemSpawnLimit", "1.0.3")]
public class NoItemSpawnLimit : BaseUnityPlugin
{
    internal static NoItemSpawnLimit Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        ConfigManager.Init(Config);

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
        Harmony.PatchAll(typeof(LevelGeneratorPatch));
        Harmony.PatchAll(typeof(PunManagerPatch));
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }

    private void Update()
    {
        // Code that runs every frame goes here
    }
}