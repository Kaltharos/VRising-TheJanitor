using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using TemplateMods;
using TemplateMods.Systems;

#if WETSTONE
using Wetstone.API;
#endif

[assembly: AssemblyVersion(BuildConfig.Version)]
[assembly: AssemblyTitle(BuildConfig.Name)]

namespace TemplateMods
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

        private static ConfigEntry<bool> EnableChatHook;

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
            EnableChatHook = Config.Bind("Config", "Enable Chat Hook", true, "Enable/disable the chat listener.");
        }

        public override void Load()
        {
            InitConfig();
            Logger = Log;
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            Log.LogInfo($"Plugin {BuildConfig.Name}-v{BuildConfig.Version} is loaded!");
        }

        public override bool Unload()
        {
            Config.Clear();
            harmony.UnpatchSelf();
            return true;
        }

        public void OnGameInitialized()
        {
            if (isInitialized) return;
            ChatSystem.isEnabled = EnableChatHook.Value;
            isInitialized = true;
        }
    }
}
