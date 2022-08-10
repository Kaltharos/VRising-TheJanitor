using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TheJanitor.Hooks
{
    [HarmonyPatch(typeof(ChatMessageSystem), nameof(ChatMessageSystem.OnUpdate))]
    public class ChatMessageSystem_Patch
    {
        public static void Prefix(ChatMessageSystem __instance)
        {
            if (Plugin.isChatListen.Value)
            {
                NativeArray<Entity> entities = __instance.__ChatMessageJob_entityQuery.ToEntityArray(Allocator.Temp);
                foreach (var entity in entities)
                {
                    var fromData = __instance.EntityManager.GetComponentData<FromCharacter>(entity);
                    var userData = __instance.EntityManager.GetComponentData<User>(fromData.User);
                    var chatEventData = __instance.EntityManager.GetComponentData<ChatMessageEvent>(entity);

                    if (chatEventData.MessageText.Equals(Plugin.onChatCommands.Value) && userData.IsAdmin)
                    {
                        var query = Plugin.Server.EntityManager.CreateEntityQuery(new EntityQueryDesc()
                        {
                            All = new ComponentType[]
                                {
                                    ComponentType.ReadOnly<DestroyWhenNoCharacterNearbyAfterDuration>(),
                                    ComponentType.ReadOnly<ItemPickup>(),
                                    ComponentType.ReadOnly<LocalToWorld>()
                                },
                            Options = EntityQueryOptions.IncludeDisabled
                        });

                        var arrays = query.ToEntityArray(Allocator.Temp);
                        foreach (var item in arrays)
                        {
                            __instance.EntityManager.AddComponent<DestroyTag>(item);
                        }
                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, userData, "All dropped item entities has been cleared.");

                        __instance.EntityManager.AddComponent<DestroyTag>(entity);
                    }
                }
            }
        }
    }
}
