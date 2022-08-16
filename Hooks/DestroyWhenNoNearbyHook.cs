using HarmonyLib;
using Unity.Collections;
using ProjectM;
using System;
using TheJanitor.Utils;
using ProjectM.Shared;
using Unity.Entities;

namespace TheJanitor.Hooks
{
    [HarmonyPatch(typeof(DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn), nameof(DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn.OnUpdate))]
    public class DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn_Patch
    {
        private static void Postfix(DestroyWhenNoCharacterNearbyAfterDurationSystem_Spawn __instance)
        {
            if (__instance.__OnUpdate_LambdaJob0_entityQuery != null)
            {
                if (Plugin.isAutoClean.Value)
                {
                    var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
                    foreach (var entity in entities)
                    {
                        try
                        {
                            if (__instance.EntityManager.HasComponent<ItemPickup>(entity))
                            {
                                TaskRunner.Start(taskWorld =>
                                {
                                    //DestroyUtility.Destroy(__instance.EntityManager, entity);
                                    __instance.EntityManager.AddComponent<DestroyTag>(entity);
                                    return new object();
                                }, false, TimeSpan.FromSeconds(Plugin.onCleanTimer.Value));
                            }
                        }
                        catch { }
                    }
                }
            }
        }
    }
}