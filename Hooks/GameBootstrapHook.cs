using Unity.Collections;
using Unity.Entities;
using AutoCloseDoors.Systems;
using HarmonyLib;
using ProjectM;

namespace AutoCloseDoors.Hooks
{
    [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.Start))]
    public static class GameBootstrapStart_Patch
    {
        public static void Postfix()
        {
            Plugin.OnGameInitialized();
        }
    }

    [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.OnApplicationQuit))]
    public static class GameBootstrapQuit_Patch
    {
        public static void Prefix()
        {
            AutoCloseDoor.RevertAutoClose();
        }
    }
}
