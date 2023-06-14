using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using BepInEx.Unity.IL2CPP;
using Bloodstone.API;
using TheJanitor.Utils;

namespace TheJanitor
{
    [BepInPlugin(PackageID, Name, Version)]
    [BepInDependency("gg.deca.Bloodstone")]
    [Reloadable]
    public class Plugin : BasePlugin, IRunOnInitialized
    {
        private const string PackageID = "kaltharos.vrising.thejanitor";
        private const string Name = "TheJanitor";
        private const string Version = "1.0.2";

        private static Harmony Harmony { get; set; }
        public static ConfigEntry<bool> IsChatListen { get; private set; }
        public static ConfigEntry<string> OnChatCommands { get; private set; }
        public static ConfigEntry<bool> IsAutoClean { get; private set; }
        public static ConfigEntry<int> OnCleanTimer { get; private set; }
        public static ManualLogSource Logger { get; private set; }

        private void InitConfig()
        {
            IsChatListen = Config.Bind("Config", "Enable Chat Listen", true, "Toggle to activate/deactivate the chat monitoring system. This determines if the system listens to chat messages in real-time.");
            OnChatCommands = Config.Bind("Config", "Chat Command", "~cleanallnow", "This command, when executed by an admin, triggers the purging of all items presently dropped on the server. It doesn't affect items dropped before the command was set.");
            IsAutoClean = Config.Bind("Config", "Enable Auto Cleaner", true, "Toggle to activate/deactivate the automatic cleaner. When activated, the system automatically removes newly dropped items from the server, except for relics and death bags.");
            OnCleanTimer = Config.Bind("Config", "Auto Clean Timer", 600, "Determines the delay (in seconds) before the automatic cleaner removes a dropped item. This timer starts once the item is dropped.");
        }

        public override void Load()
        {
            InitConfig();
            Logger = Log;
            Harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PackageID);
            TaskRunner.Initialize();
            Log.LogInfo($"Plugin {Name}-v{Version} is loaded!");
        }

        public override bool Unload()
        {
            Config.Clear();
            Harmony.UnpatchSelf();
            TaskRunner.Destroy();
            return true;
        }

        public void OnGameInitialized()
        {
            //
        }
    }
}