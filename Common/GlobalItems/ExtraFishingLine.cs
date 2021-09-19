using Lepton.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Lepton.Common.GlobalItems
{
    class ExtraFishingLine : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.fishingPole != 0;
        }

        public override bool Shoot(Item item, Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<AnglerSetBonusPlayer>().AnglerSetBonus)
            {
                float spreadAmount = 75f; // how much the different bobbers are spread out.

                Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

                // Generate new bobber
                Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
            }
            return true;
        }
    }
}
