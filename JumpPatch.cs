using HarmonyLib;

namespace HighJump
{
    // Scales local jump force and tracks the boosted jump arc.
    [HarmonyPatch(typeof(Character), nameof(Character.Jump), new[] { typeof(bool) })]
    internal static class JumpPatch
    {
        internal static bool BoostingJumpCall { get; private set; }
        internal static float ActiveHeightMultiplier { get; private set; } = 1f;
        internal static Character ActiveCharacter { get; private set; }

        // Scales takeoff speed to match the faster gravity of the boosted arc.
        private static void Prefix(Character __instance, out float __state)
        {
            __state = __instance.m_jumpForce;
            var settings = Plugin.Settings;
            if (settings == null || !settings.Enabled.Value || __instance != Player.m_localPlayer)
            {
                return;
            }

            BoostingJumpCall = true;
            __instance.m_jumpForce *= settings.HeightMultiplier.Value;
        }

        // Restores the original force and call state even if a patch throws.
        private static System.Exception Finalizer(Character __instance, float __state, System.Exception __exception)
        {
            __instance.m_jumpForce = __state;
            BoostingJumpCall = false;
            return __exception;
        }

        // Starts the faster arc only after Valheim actually launches a boosted jump.
        internal static void StartBoostedArc(Character character)
        {
            ActiveCharacter = character;
            ActiveHeightMultiplier = Plugin.Settings.HeightMultiplier.Value;
        }

        // Ends the boosted arc when the player lands or the plugin shuts down.
        internal static void StopBoostedArc()
        {
            ActiveCharacter = null;
            ActiveHeightMultiplier = 1f;
        }
    }
}
