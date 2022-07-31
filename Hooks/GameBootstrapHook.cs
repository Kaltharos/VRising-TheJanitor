using HarmonyLib;
using ProjectM;

namespace TemplateMods.Hooks
{
    [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.Start))]
    public static class GameBootstrapStart_Patch
    {
        public static void Postfix()
        {
            //-- Do things after server world is created.
            var Plugin = new Plugin();
            Plugin.OnGameInitialized();
        }
    }

    [HarmonyPatch(typeof(GameBootstrap), nameof(GameBootstrap.OnApplicationQuit))]
    public static class GameBootstrapQuit_Patch
    {
        public static void Prefix()
        {
            //-- Do something on game server shutdown.
        }
    }
}
