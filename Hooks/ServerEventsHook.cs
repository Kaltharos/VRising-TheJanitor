using System;
using HarmonyLib;
using ProjectM;
using ProjectM.Gameplay.Systems;
using Unity.Entities;

namespace TheJanitor.Hooks;

public delegate void OnUpdateEventHandler(World world);

[HarmonyPatch]
public static class ServerEvents
{
    public static event OnUpdateEventHandler OnUpdate;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(StatChangeSystem), nameof(StatChangeSystem.OnUpdate))]
    private static void StatChangeSystem_OnUpdate(ServerTimeSystem_Server __instance)
    {
        try
        {
            OnUpdate?.Invoke(__instance.World);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e);
        }
    }
}