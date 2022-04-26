using Lepton.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Lepton.Content.Projectiles
{
	public abstract class InteractibleProjectile : ModProjectile
	{
		public abstract string HighlightPath { get; }
		public abstract BetterChest ChestType { get; }
		public abstract int OpenSound { get; }
		public abstract int CloseSound { get; }
		public abstract int DisplayItem { get; }

		public abstract void SetPlayerField(Player player, int value);

		public override void SetDefaults()
        {
			Projectile.aiStyle = 97;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 10800;
			Projectile.hide = false;
		}

		public override void PostAI()
        {
			Projectile.spriteDirection = 1;
        }

		// Draws the projectile's highlight when necessary
		public override void PostDraw(Color lightColor)
		{
			Texture2D highlight = (Texture2D)ModContent.Request<Texture2D>(HighlightPath);
			int averageLight = (lightColor.R + lightColor.G + lightColor.B) / 3;
			Color highlightColor = Colors.GetSelectionGlowColor(TryInteractingWithProjectile(this) == 2, averageLight);
			if (averageLight > 10 && Main.SmartCursorIsUsed)
			{
				Vector2 drawPos = (Projectile.position - Main.screenPosition) + Projectile.Center + new Vector2(0f, Projectile.gfxOffY);
				Main.EntitySpriteDraw(highlight, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), highlightColor, Projectile.rotation, Projectile.Center, Projectile.scale, SpriteEffects.None, 0);
			}
        }

        public int TryInteractingWithProjectile(InteractibleProjectile projectile)
        {
			var proj = projectile.Projectile;
			if (Main.gamePaused || Main.gameMenu)
			{
				return 0;
			}
			bool thinCursor = !Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad;
			Player localPlayer = Main.LocalPlayer;
			var modPlayer = localPlayer.GetModPlayer<InteractibleProjectilePlayer>();
			Point val = proj.Center.ToTileCoordinates();
			Vector2 compareSpot = localPlayer.Center;
			Point closestSpot = proj.Hitbox.ClosestPointInRect(compareSpot).ToTileCoordinates();
			if (!localPlayer.IsInTileInteractionRange(closestSpot.X, closestSpot.Y))
			{
				return 0;
			}
			Matrix val2 = Matrix.Invert(Main.GameViewMatrix.ZoomMatrix);
			Vector2 val3 = Main.ReverseGravitySupport(Main.MouseScreen);
			Vector2.Transform(Main.screenPosition, val2);
			Vector2 v = Vector2.Transform(val3, val2) + Main.screenPosition;
			Rectangle hitbox = proj.Hitbox;
			bool underCursor = hitbox.Contains(v.ToPoint());
			if (!((underCursor || Main.SmartInteractProj == proj.whoAmI) & !localPlayer.lastMouseInterface))
			{
				if (!thinCursor)
				{
					return 1;
				}
				return 0;
			}
			Main.HasInteractibleObjectThatIsNotATile = true;
			if (underCursor)
			{
				localPlayer.noThrow = 2;
				localPlayer.cursorItemIconEnabled = true;
				localPlayer.cursorItemIconID = DisplayItem;
			}
			if (PlayerInput.UsingGamepad)
			{
				localPlayer.GamepadEnableGrappleCooldown();
			}
			if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
			{
				Main.mouseRightRelease = false;
				localPlayer.tileInteractAttempted = true;
				localPlayer.tileInteractionHappened = true;
				localPlayer.releaseUseTile = false;
				if (modPlayer.chest == ChestType)
				{
					localPlayer.chest = -1;
					modPlayer.chest = BetterChest.None;
					SoundEngine.PlaySound(CloseSound);
					Recipe.FindRecipes();
				}
				else
				{
					localPlayer.chest = (int)ChestType;
					modPlayer.chest = ChestType;
					for (int i = 0; i < 40; i++)
					{
						ItemSlot.SetGlow(i, -1f, chest: true);
					}
                    SetPlayerField(localPlayer, proj.whoAmI);
					localPlayer.chestX = val.X;
					localPlayer.chestY = val.Y;
					localPlayer.SetTalkNPC(-1);
					Main.SetNPCShopIndex(0);
					Main.playerInventory = true;
					SoundEngine.PlaySound(OpenSound);
					Recipe.FindRecipes();
				}
			}
			if (!Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad)
			{
				return 0;
			}
			if (!thinCursor)
			{
				return 2;  // Selected
			}
			return 0;
		}
    }
}
