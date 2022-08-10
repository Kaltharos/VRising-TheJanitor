using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using TheJanitor;
using TheJanitor.Utils;

#if WETSTONE
using Wetstone.API;
#endif

[assembly: AssemblyVersion(BuildConfig.Version)]
[assembly: AssemblyTitle(BuildConfig.Name)]

namespace TheJanitor
{
    [BepInPlugin(BuildConfig.PackageID, BuildConfig.Name, BuildConfig.Version)]

#if WETSTONE
    [BepInDependency("xyz.molenzwiebel.wetstone")]
    [Reloadable]
    public class Plugin : BasePlugin, IRunOnInitialized
#else
    public class Plugin : BasePlugin
#endif
    {
        private Harmony harmony;

        public static ConfigEntry<bool> isChatListen;
        public static ConfigEntry<string> onChatCommands;
        public static ConfigEntry<bool> isAutoClean;
        public static ConfigEntry<int> onCleanTimer;

        public static bool isInitialized = false;

        public static ManualLogSource Logger;

        private static World _serverWorld;
        public static World Server
        {
            get
            {
                if (_serverWorld != null) return _serverWorld;

                _serverWorld = GetWorld("Server")
                    ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");
                return _serverWorld;
            }
        }

        public static bool IsServer => Application.productName == "VRisingServer";

        private static World GetWorld(string name)
        {
            foreach (var world in World.s_AllWorlds)
            {
                if (world.Name == name)
                {
                    return world;
                }
            }

            return null;
        }

        public void InitConfig()
        {
            isChatListen = Config.Bind("Config", "Enable Chat Listen", true, "Enable/disable the chat listener.");
            onChatCommands = Config.Bind("Config", "Chat Command", "~cleanallnow", "Clean all dropped items on the server.\n" +
                "Command is only usable by admin.");
            isAutoClean = Config.Bind("Config", "Enable Auto Cleaner", true, "Enable the auto cleaner.\n" +
                "Does not included an already existing dropped items.\n" +
                "Relics & death bags are also excluded.");
            onCleanTimer = Config.Bind("Config", "Auto Clean Timer", 600, "Timer in seconds to wait before the dropped item is deleted automatically.");
        }

        public override void Load()
        {
            InitConfig();
            Logger = Log;
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            TaskRunner.Initialize();

            Log.LogInfo($"Plugin {BuildConfig.Name}-v{BuildConfig.Version} is loaded!");
        }

        public override bool Unload()
        {
            Config.Clear();
            harmony.UnpatchSelf();

            TaskRunner.Destroy();

            return true;
        }

        public void OnGameInitialized()
        {

        }
    }
}
