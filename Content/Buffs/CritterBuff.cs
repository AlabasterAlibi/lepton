using Lepton.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Lepton.Content.Buffs
{
    class CritterBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Critter Love");
            Description.SetDefault("Replaces around 75% of spawns with critters");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPotionPlayer>().CritterPotion = true;
        }
    }
}
