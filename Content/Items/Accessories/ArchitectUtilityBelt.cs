using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;
using Lepton.Common.Configs;

namespace Lepton.Content.Items.Accessories
{
    //[AutoloadEquip(EquipType.Waist)]
    class ArchitectUtilityBelt : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<LeptonServerConfig>().construction.ArchitectUpgradesEnabled;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Architect Utility Belt");
            Tooltip.SetDefault("Increases block placement & tool range by 2\n" +
                               "Increases mining speed by 25%");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Toolbelt)
                .AddIngredient(ItemID.Toolbox)
                .AddIngredient(ItemID.AncientChisel)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pickSpeed -= 0.25f;
            if (player.whoAmI == Main.myPlayer)
            {
                Player.tileRangeX += 2;
                Player.tileRangeY += 2;
            }
        }
    }
}