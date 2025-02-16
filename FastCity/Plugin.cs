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

namespace FastCity;

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

[HarmonyPatch(typeof(CarrierMovement), "UpdatePropulsionMoveValues")]
class PatchMapUIOpenMap
{
    [HarmonyPostfix]
    public static void Postfix(CarrierMovement __instance)
    {
        FieldInfo? fi = __instance.GetType().GetField("maxPropulsionSpeed", BindingFlags.Instance | BindingFlags.NonPublic);
        float currentSpeed = ((float?)fi?.GetValue(__instance)) ?? 0f;
        fi?.SetValue(__instance, currentSpeed * 50f);

        fi = __instance.GetType().GetField("maxPropulsionAcceleration", BindingFlags.Instance | BindingFlags.NonPublic);
        float currentmaxPropulsionAcceleration = ((float?)fi?.GetValue(__instance)) ?? 0f;
        fi?.SetValue(__instance, currentmaxPropulsionAcceleration * 10f);
    }
}
