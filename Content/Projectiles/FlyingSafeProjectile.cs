using Lepton.Common.Players;
using Terraria;
using Terraria.ID;

namespace Lepton.Content.Projectiles
{
    class FlyingSafeProjectile : InteractibleProjectile
    {
        public override BetterChest ChestType => BetterChest.Safe;
        public override int OpenSound => SoundID.MenuOpen;
        public override int CloseSound => SoundID.MenuClose;
        public override int DisplayItem => ItemID.Safe;

        public override void SetPlayerField(Player player, int value) { player.GetModPlayer<InteractibleProjectilePlayer>().flyingSafeProjectile = value; }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 46;
            Projectile.height = 28;
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.frameCounter >= 120)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
