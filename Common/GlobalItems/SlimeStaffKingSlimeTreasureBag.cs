using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Common.GlobalItems
{
    class SlimeStaffKingSlimeTreasureBag : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context.Equals("bossBag") && arg == ItemID.KingSlimeBossBag && Main.rand.Next(4) == 0)
            {
                player.QuickSpawnItem(ItemID.SlimeStaff);
            }
        }
    }
}
