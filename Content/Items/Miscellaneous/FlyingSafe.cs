using Lepton.Common.Configs;
using Lepton.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Content.Items.Miscellaneous
{
    class FlyingSafe : ModItem
    {
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<LeptonServerConfig>().FlyingSafeEnabled;
		}

        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Summons a flying safe to store your items");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 4f;
			Item.shoot = ModContent.ProjectileType<FlyingSafeProjectile>();
			Item.width = 46;
			Item.height = 28;
			Item.UseSound = new LegacySoundStyle(SoundID.Tink, 2);
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 2);
		}

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.Safe)
				.AddIngredient(ItemID.CreativeWings)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
        }
    }
}
