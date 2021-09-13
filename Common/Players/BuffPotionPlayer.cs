using Terraria.ModLoader;

namespace Lepton.Common.Players
{
    public class BuffPotionPlayer : ModPlayer
    {
        public bool RareCreaturePotion;
        public bool CritterPotion;

        public override void ResetEffects()
        {
            RareCreaturePotion = false;
            CritterPotion = false;
        }
    }
}
