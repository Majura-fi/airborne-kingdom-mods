using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Tangier.Buildings.Movement;
using Tangier.Buildings.Placement;
using Tangier.Economy.Data;
using UnityEngine;

namespace InfiniteStorage;

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

[HarmonyPatch(typeof(ResourceInventoryData), "StorageAmount", MethodType.Setter)]
class PatchResourceDataTryTradeFor
{
    [HarmonyPrefix]
    public static void Prefix(ref float value)
    {
        value = 100000f;
    }
}


[HarmonyPatch(typeof(ResourceInventoryData), "StorageAmount", MethodType.Getter)]
class PatchWeightBalancerGetTotalWeight
{
    [HarmonyPrefix]
    public static void Postfix(ref float __result)
    {
        __result = 100000f;
    }
}
