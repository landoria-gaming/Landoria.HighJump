using HarmonyLib;

namespace HighJump
{
    // Detects the first sprint jump after Valheim launches it.
    [HarmonyPatch(typeof(Character), nameof(Character.ForceJump))]
    internal static class ForceJumpPatch
    {
        // Records the first jump speed for the midair jump.
        private static void Postfix(Character __instance)
        {
            if (JumpPatch.SprintJumpCall && __instance == Player.m_localPlayer)
            {
                JumpPatch.StartJump(__instance);
            }
        }
    }
}
