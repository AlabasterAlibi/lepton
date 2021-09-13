using Lepton.Content.Buffs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Content.Items.Consumables.Potions
{
    class CritterPotion : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return Lepton.ServerConfig.CritterPotionEnabled;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Replaces around 75% of spawns with critters\n" +
                               "'Tastes like paper...and pacifism!'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 5);
            Item.buffType = ModContent.BuffType<CritterBuff>();
            Item.buffTime = 36000; // 10 minutes
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BattlePotion)
                .AddIngredient(ItemID.CalmingPotion)
                .AddIngredient(ItemID.DontHurtCrittersBook)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
