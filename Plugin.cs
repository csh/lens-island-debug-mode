using System.Diagnostics.CodeAnalysis;
using BepInEx;
using HarmonyLib;

namespace LensIslandDebugMode;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DebugModePlugin : BaseUnityPlugin
{
    private Harmony _harmony;

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(typeof(DebugConsolePatches));
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
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