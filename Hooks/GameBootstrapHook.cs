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
            var Plugin = new Plugin();
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
