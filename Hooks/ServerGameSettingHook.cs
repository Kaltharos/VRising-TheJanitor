using AutoCloseDoors.Systems;
using HarmonyLib;
using ProjectM;

namespace AutoCloseDoors.Hooks
{
    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.VerifyServerGameSettings))]
    public class ServerGameSetting_Patch
    {
        private static bool isInitialized = false;
        public static void Postfix()
        {
            if (isInitialized == false)
            {
                if (AutoCloseDoor.isAutoCloseDoor) AutoCloseDoor.InitializeAutoClose();
                isInitialized = true;
            }
        }
    }
}
