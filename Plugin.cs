using BepInEx;
using HarmonyLib;

namespace HighJump
{
    // Owns the jump patch lifecycle.
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "HighJump";
        public const string PluginName = "HighJump";
        public const string PluginVersion = "1.0.2";
        private Harmony _harmony;

        // Installs the local jump patch.
        private void Awake()
        {
            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();
            Logger.LogInfo($"{PluginName} {PluginVersion} loaded.");
        }

        // Removes only this plugin's patches.
        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
