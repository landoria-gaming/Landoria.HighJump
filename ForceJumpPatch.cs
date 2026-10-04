using HarmonyLib;

namespace HighJump
{
    // Detects a real launch made by the local boosted jump call.
    [HarmonyPatch(typeof(Character), nameof(Character.ForceJump))]
    internal static class ForceJumpPatch
    {
        // Starts the boosted arc after Valheim applies its jump velocity.
        private static void Postfix(Character __instance)
        {
            if (JumpPatch.BoostingJumpCall && __instance == Player.m_localPlayer)
            {
                JumpPatch.StartBoostedArc(__instance);
            }
        }
    }
}
