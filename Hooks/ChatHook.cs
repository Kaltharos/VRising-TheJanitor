using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using TemplateMods.Systems;

namespace TemplateMods.Hooks
{
    [HarmonyPatch(typeof(ChatMessageSystem), nameof(ChatMessageSystem.OnUpdate))]
    public class ChatMessageSystem_Patch
    {
        public static void Prefix(ChatMessageSystem __instance)
        {
            if (ChatSystem.isEnabled)
            {
                NativeArray<Entity> entities = __instance.__ChatMessageJob_entityQuery.ToEntityArray(Allocator.Temp);
                foreach (var entity in entities)
                {
                    var fromData = __instance.EntityManager.GetComponentData<FromCharacter>(entity);
                    var userData = __instance.EntityManager.GetComponentData<User>(fromData.User);
                    var chatEventData = __instance.EntityManager.GetComponentData<ChatMessageEvent>(entity);

                    if (chatEventData.MessageText.Equals(ChatSystem.Commands) && userData.IsAdmin)
                    {
                        //-- Do something...
                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, userData, "Hello!");
                        __instance.EntityManager.AddComponent<DestroyTag>(entity);
                    }
                }
            }
        }
    }
}
