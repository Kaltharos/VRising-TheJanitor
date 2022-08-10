using HarmonyLib;
using ProjectM;

namespace TheJanitor.Hooks
{
    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.VerifyServerGameSettings))]
    public class ServerGameSetting_Patch
    {
        private static bool isInitialized = false;
        public static void Postfix()
        {
            if (isInitialized == false)
            {
                //-- Do Something after all game data from save is loaded.
                isInitialized = true;
            }
        }
    }
}
