using Lepton.Common.Configs;
using Lepton.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Common.GlobalItems
{
    class AnglerSetBonus : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ItemID.AnglerHat;
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            return Lepton.ServerConfig.AnglerSetBonusEnabled && head.type == ItemID.AnglerHat && body.type == ItemID.AnglerVest && legs.type == ItemID.AnglerPants ? "Angler Set" : "";
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            player.setBonus = "Adds another line to fishing rods";
            ModContent.GetInstance<AnglerSetBonusPlayer>().AnglerSetBonus = true;
        }
    }
}
