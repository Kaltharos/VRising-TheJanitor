using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using AutoCloseDoors.Systems;

namespace AutoCloseDoors.Hooks
{
    [HarmonyPatch(typeof(ChatMessageSystem), nameof(ChatMessageSystem.OnUpdate))]
    public class ChatMessageSystem_Patch
    {
        public static void Prefix(ChatMessageSystem __instance)
        {
            if (AutoCloseDoor.isEnableUninstall)
            {
                NativeArray<Entity> entities = __instance.__ChatMessageJob_entityQuery.ToEntityArray(Allocator.Temp);
                foreach (var entity in entities)
                {
                    var fromData = __instance.EntityManager.GetComponentData<FromCharacter>(entity);
                    var userData = __instance.EntityManager.GetComponentData<User>(fromData.User);
                    var chatEventData = __instance.EntityManager.GetComponentData<ChatMessageEvent>(entity);

                    if (chatEventData.MessageText.Equals(AutoCloseDoor.UninstallCommand) && userData.IsAdmin)
                    {
                        AutoCloseDoor.isAutoCloseDoor = false;
                        AutoCloseDoor.RevertAutoClose();
                        ServerChatUtils.SendSystemMessageToClient(__instance.EntityManager, userData, "AutoCloseDoor has been uninstalled.");
                        __instance.EntityManager.AddComponent<DestroyTag>(entity);
                    }
                }
            }
        }
    }
}
