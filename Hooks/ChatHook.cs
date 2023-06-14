using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using ProjectM.Shared;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TheJanitor.Hooks;

[HarmonyPatch]
public static class ChatHook
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChatMessageSystem), nameof(ChatMessageSystem.OnUpdate))]
    public static void ChatMessageSystem_OnUpdate(ref ChatMessageSystem __instance)
    {
        if (!Plugin.IsChatListen.Value) return;
        
        var entities = __instance.__ChatMessageJob_entityQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities)
        {
            var fromData = __instance.EntityManager.GetComponentData<FromCharacter>(entity);
            var userData = __instance.EntityManager.GetComponentData<User>(fromData.User);
            var chatEventData = __instance.EntityManager.GetComponentData<ChatMessageEvent>(entity);

            if (chatEventData.MessageText.Equals(Plugin.OnChatCommands.Value) && userData.IsAdmin)
            {
                var query = VWorld.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<DestroyWhenNoCharacterNearbyAfterDuration>(),
                        ComponentType.ReadOnly<ItemPickup>(),
                        ComponentType.ReadOnly<LocalToWorld>()
                    },
                    Options = EntityQueryOptions.IncludeDisabled
                });
                    
                foreach (var e in query.ToEntityArray(Allocator.Temp))
                {
                    DestroyUtility.CreateDestroyEvent(__instance.EntityManager,e, DestroyReason.Default, DestroyDebugReason.None);
                    DestroyUtility.Destroy(__instance.EntityManager,e);
                }
                   
                ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, userData,
                    "All dropped item entities has been cleared.");
            }
        }
    }
}