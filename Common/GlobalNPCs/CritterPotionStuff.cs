using Lepton.Common.Players;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ID.NPCID;

namespace Lepton.Common.GlobalNPCs
{
    class CritterPotionStuff : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo info)
        {
            if (!info.Player.GetModPlayer<BuffPotionPlayer>().CritterPotion || Main.invasionType != 0) { return; }

            int crittersAdded = 0;

            // Helper functions
            void addCritters(bool shouldAdd, int goldNPC, params int[] npcs)
            {
                if (shouldAdd)
                {
                    foreach (int npc in npcs)
                    {
                        if (!pool.ContainsKey(npc))
                        {
                            pool.Add(npc, 1f);
                            crittersAdded++;
                        }
                    }
                    if (goldNPC != None)
                    {
                        if (!pool.ContainsKey(goldNPC))
                        {
                            pool.Add(goldNPC, 0.0025f);
                        }
                    }
                }
            }

            bool WaterSurface()
            {
                if (info.SafeRangeX || !info.Water)
                {
                    return false;
                }
                for (int num = info.SpawnTileY - 1; num > info.SpawnTileY - 50; num--)
                {
                    if (Main.tile[info.SpawnTileX, num].LiquidType == 0 && !WorldGen.SolidTile(info.SpawnTileX, num) && !WorldGen.SolidTile(info.SpawnTileX, num + 1) && !WorldGen.SolidTile(info.SpawnTileX, num + 2))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool surface = info.SpawnTileY <= Main.worldSurface;
            bool underground = info.SpawnTileY > Main.worldSurface && info.SpawnTileY <= Main.rockLayer;
            bool cavern = info.SpawnTileY > Main.rockLayer && info.SpawnTileY <= Main.maxTilesY - 210;
            bool hell = info.SpawnTileY > Main.maxTilesY - 210;

            bool water = info.Water;

            Player player = info.Player;
            bool forest = player.ZonePurity;
            bool snow = player.ZoneSnow;
            bool desert = player.ZoneDesert;
            bool jungle = player.ZoneJungle;
            bool hallow = player.ZoneHallow;
            bool ocean = (info.SpawnTileX < 250 || info.SpawnTileX > Main.maxTilesX - 250) && Main.tileSand[info.SpawnTileType] && (surface || underground);
            bool mushroom = player.ZoneGlowshroom;
            bool graveyard = player.ZoneGraveyard;

            bool day = Main.dayTime;
            bool windy = NPC.TooWindyForButterflies;
            bool rainy = Main.raining;

            addCritters(surface && (forest || snow || hallow), GoldBird, Bird, BirdBlue, BirdRed);
            addCritters(surface && forest, GoldBunny, Bunny);
            addCritters(surface && forest, SquirrelGold, Squirrel, SquirrelRed);
            addCritters(surface && WaterSurface(), None, Duck, DuckWhite);
            addCritters(cavern, None, Enumerable.Range(639, 14).Append(Snail).ToArray()); // Gem Critters & Snail
            addCritters((surface && rainy) || underground, GoldWorm, Worm);
            addCritters(surface && Star.starfallBoost >= 2f, None, EnchantedNightcrawler);
            addCritters(ocean && water, GoldSeahorse, Seahorse, Dolphin);
            addCritters(surface && !day, None, Firefly);
            addCritters(surface && !day && forest, None, Owl);
            addCritters(surface && !day && hallow, None, LightningBug);
            addCritters(surface && jungle, GoldFrog, Frog);
            addCritters(mushroom, None, GlowingSnail);
            addCritters(water, GoldGoldfish, Goldfish);
            addCritters(surface && desert && WaterSurface(), GoldDragonfly, BlackDragonfly, YellowDragonfly, OrangeDragonfly, Grebe);
            addCritters(surface && forest && windy, GoldLadyBug, LadyBug);
            addCritters(hell, None, Lavafly, MagmaSnail, HellButterfly);
            addCritters(surface && graveyard, None, Maggot, Rat);
            addCritters(underground || cavern, GoldMouse, Mouse);
            addCritters(surface && snow, None, Penguin, PenguinBlack);
            addCritters(desert && water, None, Pupfish);
            addCritters(desert && day, None, Scorpion);
            addCritters(desert, None, Scorpion);
            addCritters(ocean, None, Seagull);
            addCritters(forest && day && WaterSurface(), None, Turtle);
            addCritters(jungle && WaterSurface(), None, TurtleJungle);
            addCritters(ocean && WaterSurface(), None, SeaTurtle);
            addCritters(surface && (desert || jungle) && day && !windy && WaterSurface(), GoldWaterStrider, WaterStrider);
            addCritters(surface && forest && !windy && WaterSurface(), GoldDragonfly, RedDragonfly, BlueDragonfly, GreenDragonfly);
            addCritters(surface && forest && !windy, GoldButterfly, Butterfly);
            addCritters(surface && hallow && !day && info.PlanteraDefeated, None, EmpressButterfly);
            addCritters(mushroom && Main.hardMode, None, TruffleWorm);

            pool[0] *= crittersAdded * 0.5f; // Scales vanilla spawns so that ~75% of spawns are critters
        }
    }
}
