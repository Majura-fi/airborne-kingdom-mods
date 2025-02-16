using System.Linq;
using System.Reflection;
using System.Threading;
using BepInEx;
using HarmonyLib;
using Tangier.Buildings.Movement;
using Tangier.Buildings.Placement;
using Tangier.Economy.Data;
using Tangier.UI.HUD.Exploration;
using UnityEngine;

namespace RevealMap;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(MapUI), "OpenMap")]
class PatchMapUIOpenMap
{
    [HarmonyPrefix]
    public static void Prefix(MapUI __instance)
    {
        MethodInfo? mi = __instance.GetType().GetMethod("RevealMap", BindingFlags.NonPublic | BindingFlags.Instance);
        mi?.Invoke(__instance, null);
    }
}
