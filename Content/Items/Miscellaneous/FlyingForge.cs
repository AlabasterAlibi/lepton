using Lepton.Common.Configs;
using Lepton.Content.Projectiles;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Content.Items.Miscellaneous
{
    class FlyingForge : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
			return ModContent.GetInstance<LeptonServerConfig>().FlyingForgeEnabled;
        }

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Flying Defender's Forge");
			Tooltip.SetDefault("Summons a flying Defender's Forge to store your items");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 4f;
			Item.shoot = ModContent.ProjectileType<FlyingForgeProjectile>();
			Item.width = 36;
			Item.height = 22;
			Item.UseSound = SoundID.DD2_BallistaTowerShot;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.DefendersForge)
				.AddIngredient(ItemID.CreativeWings)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
