using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Lepton.Common.Configs
{
    public class LeptonServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Miscellaneous")]
        [Label("Angler Armor Set Bonus")]
        [Tooltip("Toggles the extra fishing line bonus for the Angler Armor")]
        [DefaultValue(true)]
        public bool AnglerSetBonusEnabled;

        [Label("Flying Safe")]
        [Tooltip("Toggles whether to load the Flying Safe. Reload required")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool FlyingSafeEnabled;

        [Label("Flying Defender's Forge")]
        [Tooltip("Toggles whether to load the Flying Defender's Forge. Reload required")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool FlyingForgeEnabled;

        [Label("Critter Potion")]
        [Tooltip("Toggles whether to load the Critter Potion. Reload required")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool CritterPotionEnabled;

        [Label("Construction")]
        public Construction construction = new();

        [Label("Compatibility")]
        public Compatibility compatibility = new();

        [SeparatePage]
        public class Construction
        {
            [Label("Architect Tinker Upgrades")]
            [Tooltip("Toggles whether to load the Architect Utility Belt and its upgrades. Reload required")]
            [DefaultValue(true)]
            [ReloadRequired]
            public bool ArchitectUpgradesEnabled;
        }

        [SeparatePage]
        public class Compatibility
        {
            [Header("AutoTrash")]
            [Label("Auto Trash Researched Items")]
            [Tooltip("Toggles automatically trashing fully researched Journey Mode items")]
            [DefaultValue(true)]
            public bool JourneyAutoTrashEnabled;

            [Label("Disable AutoTrash Ctrl + Shift Click functionality")]
            [Tooltip("Disables AutoTrash's Ctrl + Shift Click function, which conflicts with Lepton's")]
            [DefaultValue(true)]
            public bool ShouldDisableAutoTrashKeybind;
        }
    }
}
