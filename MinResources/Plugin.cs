using System.Linq;
using System.Reflection;
using System.Threading;
using BepInEx;
using HarmonyLib;
using Tangier.Buildings.Movement;
using Tangier.Buildings.Placement;
using Tangier.Economy.Data;
using UnityEngine;

namespace MinResources;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static bool CurrentlyAccessing = false;
    public static readonly float MinAmount = 50f;
    private static readonly Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(ResourceInventoryData), "CurrentAmountStored", MethodType.Setter)]
class PatchResourceDataCurrentAmountStoredSetter
{
    [HarmonyPostfix]
    public static void Postfix(ResourceInventoryData __instance)
    {
        if (Plugin.CurrentlyAccessing)
        {
            return;
        }

        Plugin.CurrentlyAccessing = true;

        if (__instance.CurrentAmountStored < Plugin.MinAmount)
        {
            MethodInfo? setter = AccessTools.PropertySetter(typeof(ResourceInventoryData), "CurrentAmountStored");
            setter?.Invoke(__instance, [Plugin.MinAmount]);
        }

        Plugin.CurrentlyAccessing = false;
    }
}

[HarmonyPatch(typeof(ResourceInventoryData), "CurrentAmountStored", MethodType.Getter)]
class PatchResourceDataCurrentAmountStoredGetter
{
    [HarmonyPostfix]
    public static void Postfix(ResourceInventoryData __instance)
    {
        if (Plugin.CurrentlyAccessing)
        {
            return;
        }

        Plugin.CurrentlyAccessing = true;

        if (__instance.CurrentAmountStored < Plugin.MinAmount)
        {
            MethodInfo? setter = AccessTools.PropertySetter(typeof(ResourceInventoryData), "CurrentAmountStored");
            setter?.Invoke(__instance, [Plugin.MinAmount]);
        }

        Plugin.CurrentlyAccessing = false;
    }
}

[HarmonyPatch(typeof(ResourceData), "WithdrawResource")]
class PatchWithdrawResourceWithdrawResource
{
    [HarmonyPostfix]
    public static void Postfix(ResourceData __instance)
    {
        if (__instance.CurrentAmount < Plugin.MinAmount)
        {
            __instance.StoreResource(Plugin.MinAmount - __instance.CurrentAmount);
        }
    }
}

[HarmonyPatch(typeof(ResourceData), "WithdrawAsMuchAsPossible")]
class PatchWithdrawResourceWithdrawAsMuchAsPossible
{
    [HarmonyPostfix]
    public static void Postfix(ResourceData __instance)
    {
        if (__instance.CurrentAmount < Plugin.MinAmount)
        {
            __instance.StoreResource(Plugin.MinAmount - __instance.CurrentAmount);
        }
    }
}

[HarmonyPatch(typeof(ResourceData), "TryWithdraw")]
class PatchWithdrawResourceTryWithdraw
{
    [HarmonyPostfix]
    public static void Postfix(ResourceData __instance)
    {
        if (__instance.CurrentAmount < Plugin.MinAmount)
        {
            __instance.StoreResource(Plugin.MinAmount - __instance.CurrentAmount);
        }
    }
}

[HarmonyPatch(typeof(ResourceData), "TryTradeFor")]
class PatchWithdrawResourceTryTradeFor
{
    [HarmonyPostfix]
    public static void Postfix(ResourceData __instance)
    {
        if (__instance.CurrentAmount < Plugin.MinAmount)
        {
            __instance.StoreResource(Plugin.MinAmount - __instance.CurrentAmount);
        }
    }
}