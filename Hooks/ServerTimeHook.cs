using System;
using HarmonyLib;
using ProjectM;
using Unity.Entities;

namespace TheJanitor.Hooks
{
    public delegate void OnUpdateEventHandler(World world);

    [HarmonyPatch(typeof(ServerTimeSystem_Server), nameof(ServerTimeSystem_Server.OnUpdate))]
    public static class ServerEvents
    {
        public static event OnUpdateEventHandler OnUpdate;
        private static void Postfix(ServerTimeSystem_Server __instance)
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
}