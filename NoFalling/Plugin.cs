using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Tangier.Buildings.Movement;
using Tangier.Buildings.Placement;
using UnityEngine;

namespace NoFalling;

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

[HarmonyPatch]
class PatchCarrierMovement
{
    public static MethodInfo TargetMethod()
    {
        return typeof(CarrierMovement)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(mi => mi.Name == "ShouldFall")
            .Where(mi => mi.GetParameters().Length > 0)
            .First();
    }

    [HarmonyPostfix]
    public static void Postfix(CarrierMovement __instance, ref bool __result, ref float fallRate)
    {
        FieldInfo? f = __instance.GetType().GetField("fallRate", BindingFlags.NonPublic | BindingFlags.Instance);
        f?.SetValue(__instance, 0f);

        __result = false;
        fallRate = 0;
    }
}


[HarmonyPatch(typeof(WeightBalancer), "TotalWeight", MethodType.Setter)]
class PatchWeightBalancerSetTotalWeight
{
    [HarmonyPrefix]
    public static void Prefix(ref float value)
    {
        value = 0.1f;
    }
}


[HarmonyPatch(typeof(WeightBalancer), "TotalWeight", MethodType.Getter)]
class PatchWeightBalancerGetTotalWeight
{
    [HarmonyPrefix]
    public static void Postfix(ref float __result)
    {
        __result = 0.1f;
    }
}
