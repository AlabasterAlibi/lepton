using Terraria.ModLoader;

namespace Lepton.Common.Players
{
    class AnglerSetBonusPlayer : ModPlayer
    {
        public bool AnglerSetBonus;

        public override void ResetEffects()
        {
            AnglerSetBonus = false;
        }
    }
}
