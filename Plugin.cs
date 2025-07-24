using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace LensIslandDebugMode;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DebugModePlugin : BaseUnityPlugin
{
    private ConfigEntry<bool> _enablePatches;
    private Harmony _harmony;
    
    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");

        /*
         * Default to false so that if somebody forgets to disable the mod before joining
         * a server any unknown side effects can be prevented.
         */
        _enablePatches = Config.Bind("Debug Mode", "Enable", false);
        
        _enablePatches.SettingChanged += ToggleDebugMode;

        if (_enablePatches.Value)
        {
            ApplyPatch();
        }
    }

    private void ToggleDebugMode(object sender, EventArgs e)
    {
        if (_enablePatches.Value)
        {
            ApplyPatch();
        }
        else
        {
            RevertPatch();
        }
    }

    private void ApplyPatch()
    {
        if (_harmony is not null) return;
        _harmony = new  Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(typeof(DebugConsolePatches));
    }

    private void RevertPatch()
    {
        if (_harmony is null) return;
        _harmony.UnpatchSelf();
        _harmony = null;
    }

    private void OnDestroy()
    {
        if (_enablePatches is not null)
        {
            _enablePatches.SettingChanged -= ToggleDebugMode;
        }
        RevertPatch();
    }

    public static class DebugConsolePatches
    {
        [HarmonyPrefix, HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DebugCommandAllowed))]
        public static bool DebugCommandAllowedPrefix(
            [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Harmony")]
            ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DevBuild), MethodType.Getter)]
        public static bool DevBuildPrefix(
            [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Harmony")]
            ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DevPlayingLiveBuild), MethodType.Getter)]
        public static bool DevPlayingLiveBuildPrefix(
            [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Harmony")]
            ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}