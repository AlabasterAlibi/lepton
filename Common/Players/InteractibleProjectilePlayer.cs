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
            if (!Main.playerInventory || Player.chest != (int)chest) // Player closed inventory
            {
                flyingSafeProjectile = -1;
                flyingForgeProjectile = -1;
                chest = BetterChest.None;
                return;
            }

            handleProjectile(flyingSafeProjectile, BetterChest.Safe);
            handleProjectile(flyingForgeProjectile, BetterChest.Forge);

            void handleProjectile(int projectile, BetterChest chest)
            {
                if (projectile >= 0 && this.chest == chest)
                {
                    if (!Main.projectile[projectile].active)
                    {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                        Player.chest = -1;
                        Recipe.FindRecipes();
                        return;
                    }
                    Player.chest = (int)chest;
                    int num = (int)((Player.position.X + Player.width * 0.5) / 16.0);
                    int num2 = (int)((Player.position.Y + Player.height * 0.5) / 16.0);
                    Vector2 val = Main.projectile[projectile].Hitbox.ClosestPointInRect(Player.Center);
                    Player.chestX = (int)val.X / 16;
                    Player.chestY = (int)val.Y / 16;
                    if (num < Player.chestX - Player.tileRangeX || num > Player.chestX + Player.tileRangeX + 1 || num2 < Player.chestY - Player.tileRangeY || num2 > Player.chestY + Player.tileRangeY + 1)
                    {
                        if (Player.chest != -1)
                        {
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        Player.chest = -1;
                        this.chest = BetterChest.None;
                        Recipe.FindRecipes();
                    }
                }
            }
        }
    }
}
