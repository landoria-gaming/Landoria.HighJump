using HarmonyLib;
using UnityEngine;

namespace HighJump
{
    // Applies extra gravity only while a boosted local jump is airborne.
    [HarmonyPatch(typeof(Character), nameof(Character.CustomFixedUpdate))]
    internal static class JumpGravityPatch
    {
        // Keeps boosted jumps high but close to the vanilla airtime.
        private static void Prefix(Character __instance, float dt)
        {
            if (__instance != JumpPatch.ActiveCharacter)
            {
                return;
            }

            if (__instance.IsOnGround() || __instance.IsDead() || __instance.IsSwimming()
                || __instance.IsAttached() || __instance.IsDebugFlying()
                || Plugin.Settings == null || !Plugin.Settings.Enabled.Value)
            {
                JumpPatch.StopBoostedArc();
                return;
            }

            Vector3 extraGravity = Physics.gravity * ((JumpPatch.ActiveHeightMultiplier - 1f) * dt);
            __instance.SetVelocity(__instance.GetVelocity() + extraGravity);
        }
    }
}
