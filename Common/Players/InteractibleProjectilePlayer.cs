using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Common.Players
{
    public enum BetterChest
    {
        None = -1,
        Safe = -3,
        Forge = -4
    }

    public class InteractibleProjectilePlayer : ModPlayer
    {
        public BetterChest chest;
        public int flyingSafeProjectile;
        public int flyingForgeProjectile;

        public override void PostUpdate()
        {
            HandleBeingInProjectileRange();
        }

        public void HandleBeingInProjectileRange()
        {
            if (!Main.playerInventory) // Player closed inventory
            {
                flyingSafeProjectile = -1;
                flyingForgeProjectile = -1;
                chest = BetterChest.None;
                return;
            }
            if (flyingSafeProjectile >= 0 && chest == BetterChest.Safe)
            {
                if (!Main.projectile[flyingSafeProjectile].active)
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    Player.chest = -1;
                    Recipe.FindRecipes();
                    return;
                }
                Player.chest = -3;
                int num = (int)(((double)Player.position.X + (double)Player.width * 0.5) / 16.0);
                int num2 = (int)(((double)Player.position.Y + (double)Player.height * 0.5) / 16.0);
                Vector2 val = Main.projectile[flyingSafeProjectile].Hitbox.ClosestPointInRect(Player.Center);
                Player.chestX = (int)val.X / 16;
                Player.chestY = (int)val.Y / 16;
                if (num < Player.chestX - Player.tileRangeX || num > Player.chestX + Player.tileRangeX + 1 || num2 < Player.chestY - Player.tileRangeY || num2 > Player.chestY + Player.tileRangeY + 1)
                {
                    if (Player.chest != -1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    Player.chest = -1;
                    chest = BetterChest.None;
                    Recipe.FindRecipes();
                }
            }
            if (flyingForgeProjectile >= 0 && chest == BetterChest.Forge)
            {
                if (!Main.projectile[flyingForgeProjectile].active)
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    Player.chest = -1;
                    Recipe.FindRecipes();
                    return;
                }
                Player.chest = -4;
                int num = (int)(((double)Player.position.X + (double)Player.width * 0.5) / 16.0);
                int num2 = (int)(((double)Player.position.Y + (double)Player.height * 0.5) / 16.0);
                Vector2 val = Main.projectile[flyingForgeProjectile].Hitbox.ClosestPointInRect(Player.Center);
                Player.chestX = (int)val.X / 16;
                Player.chestY = (int)val.Y / 16;
                if (num < Player.chestX - Player.tileRangeX || num > Player.chestX + Player.tileRangeX + 1 || num2 < Player.chestY - Player.tileRangeY || num2 > Player.chestY + Player.tileRangeY + 1)
                {
                    if (Player.chest != -1)
                    {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    Player.chest = -1;
                    chest = BetterChest.None;
                    Recipe.FindRecipes();
                }
            }
        }
    }
}
