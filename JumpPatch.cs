using HarmonyLib;
using UnityEngine;

namespace HighJump
{
    // Temporarily scales jump force only for the local player.
    [HarmonyPatch(typeof(Character), nameof(Character.Jump), new[] { typeof(bool) })]
    internal static class JumpPatch
    {
        // Scales launch speed by the square root of the desired height ratio.
        private static void Prefix(Character __instance, out float __state)
        {
            __state = __instance.m_jumpForce;
            var settings = Plugin.Settings;
            if (settings == null || !settings.Enabled.Value || __instance != Player.m_localPlayer)
            {
                return;
            }

            __instance.m_jumpForce *= Mathf.Sqrt(settings.HeightMultiplier.Value);
        }

        // Restores the original force even if another jump patch throws.
        private static System.Exception Finalizer(Character __instance, float __state, System.Exception __exception)
        {
            __instance.m_jumpForce = __state;
            return __exception;
        }
    }
}
