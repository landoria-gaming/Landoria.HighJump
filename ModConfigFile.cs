using BepInEx.Configuration;

namespace HighJump
{
    // Holds the settings for local jump height.
    internal sealed class ModConfigFile
    {
        public ConfigEntry<bool> Enabled { get; }
        public ConfigEntry<float> HeightMultiplier { get; }

        // Binds settings with a safe range for normal play.
        public ModConfigFile(ConfigFile config)
        {
            Enabled = config.Bind("General", "Enabled", true, "Increase your character's jump height.");
            HeightMultiplier = config.Bind("General", "HeightMultiplier", 2f,
                new ConfigDescription("Approximate jump height multiplier. 1 is vanilla.",
                    new AcceptableValueRange<float>(1f, 4f)));
        }
    }
}
