using System;
using HarmonyLib;
using ProjectM;
using ProjectM.Shared;
using TheJanitor.Utils;
using Unity.Collections;

namespace TheJanitor.Hooks;

[HarmonyPatch]
public static class DestroyWhenNoNearbyHook
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn),
        nameof(DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn.OnUpdate))]
    private static void DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn_OnUpdate(
        DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn __instance)
    {
        if (__instance?.__OnUpdate_LambdaJob0_entityQuery == null) return;
        if (!Plugin.IsAutoClean.Value) return;
        
        var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities)
        {
            try
            {
                if (__instance.EntityManager.HasComponent<ItemPickup>(entity))
                {
                    TaskRunner.Start(taskWorld =>
                    {
                        DestroyUtility.CreateDestroyEvent(__instance.EntityManager,entity, DestroyReason.Default, DestroyDebugReason.None);
                        DestroyUtility.Destroy(__instance.EntityManager,entity);
                        return new object();
                    }, false, TimeSpan.FromSeconds(Plugin.OnCleanTimer.Value));
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}