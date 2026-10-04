using HarmonyLib;
using UnityEngine;

namespace HighJump
{
    // Allows one normal midair jump after a sprint jump.
    [HarmonyPatch(typeof(Character), nameof(Character.Jump), new[] { typeof(bool) })]
    internal static class JumpPatch
    {
        internal static bool SprintJumpCall { get; private set; }
        private static Character _activeCharacter;
        private static float _launchSpeed;
        private static bool _secondJumpUsed;

        // Records a sprint jump or launches its second jump during ascent.
        private static bool Prefix(Character __instance)
        {
            if (__instance != Player.m_localPlayer)
            {
                return true;
            }

            if (__instance.IsDead())
            {
                ResetJump();
                return true;
            }

            if (__instance.IsOnGround())
            {
                ResetJump();
                SprintJumpCall = __instance.IsRunning();
                return true;
            }

            return !TrySecondJump(__instance);
        }

        // Clears the call marker even if Valheim or another patch throws.
        private static System.Exception Finalizer(System.Exception __exception)
        {
            SprintJumpCall = false;
            return __exception;
        }

        // Remembers the speed of a real sprint jump after Valheim launches it.
        internal static void StartJump(Character character)
        {
            _activeCharacter = character;
            _launchSpeed = character.GetVelocity().y;
            _secondJumpUsed = false;
        }

        // Clears the jump allowance for the next sprint jump.
        private static void ResetJump()
        {
            _activeCharacter = null;
            _launchSpeed = 0f;
            _secondJumpUsed = false;
        }

        // Launches a normal-strength second jump from the current position.
        private static bool TrySecondJump(Character character)
        {
            if (character != _activeCharacter || _secondJumpUsed || _launchSpeed <= 0f
                || character.IsSwimming() || character.IsAttached() || character.IsDebugFlying())
            {
                return false;
            }

            Vector3 velocity = character.GetVelocity();
            if (velocity.y <= 0f)
            {
                return false;
            }

            _secondJumpUsed = true;
            velocity.y = _launchSpeed;
            if (!character.HaveStamina(character.m_jumpStaminaUsage))
            {
                velocity *= character.m_jumpForceTiredFactor;
            }

            character.ForceJump(velocity);
            return true;
        }
    }
}
