using BepInEx.Configuration;

namespace HighJump
{
    // Holds the settings for local jump height.
    internal sealed class ModConfigFile
    {
        public ConfigEntry<bool> Enabled { get; }
        public ConfigEntry<float> HeightMultiplier { get; }

        // Binds settings with four jump height choices.
        public ModConfigFile(ConfigFile config)
        {
            Enabled = config.Bind("General", "Enabled", true, "Increase jump height while sprinting.");
            HeightMultiplier = config.Bind("General", "HeightMultiplier", 2f,
                new ConfigDescription("Approximate jump height multiplier. 1 is vanilla.",
                    new AcceptableValueList<float>(1f, 2f, 3f, 4f)));
        }
    }
}
