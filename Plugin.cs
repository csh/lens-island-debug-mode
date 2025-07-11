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
        _harmony.PatchAll();
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
    
    [HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DevBuild), MethodType.Getter)]
    class Patch_DevBuild
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DevPlayingLiveBuild), MethodType.Getter)]
    class Patch_DevPlayingLiveBuild
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.DebugCommandAllowed))]
    class Patch_DebugCommandAllowed
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
