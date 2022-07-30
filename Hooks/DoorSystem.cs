using HarmonyLib;
using Unity.Entities;
using Unity.Collections;
using ProjectM.Network;
using ProjectM;
using AutoCloseDoors.Systems;
using ProjectM.Gameplay.Systems;
using System;
using AutoCloseDoors.Utils;

namespace AutoCloseDoors.Hooks
{
    [HarmonyPatch(typeof(OpenDoorSystem), nameof(OpenDoorSystem.OnUpdate))]
    public class OpenDoorSystem_Patch
    {
        private static void Prefix(OpenDoorSystem __instance)
        {
            if (AutoCloseDoor.isAutoCloseDoor)
            {
                if (__instance.__OnUpdate_LambdaJob0_entityQuery != null)
                {
                    var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
                    foreach (var entity in entities)
                    {
                        var Target = __instance.EntityManager.GetComponentData<SpellTarget>(entity).Target;
                        AutoCloseDoor.DoorReceiver(Target, __instance.EntityManager);
                    }
                }
            }
        }
    }
}
