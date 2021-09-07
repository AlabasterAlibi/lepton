using Lepton.Common.Players;
using Terraria;
using Terraria.ID;

namespace Lepton.Content.Projectiles
{
    class FlyingForgeProjectile : InteractibleProjectile
    {
        public override int ChestType => -4;
        public override int OpenSound => SoundID.MenuOpen;
        public override int CloseSound => SoundID.MenuClose;
        public override int DisplayItem => ItemID.DefendersForge;

        public override void SetPlayerField(Player player, int value) { player.GetModPlayer<InteractibleProjectilePlayer>().flyingForgeProjectile = value; }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 32;
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
