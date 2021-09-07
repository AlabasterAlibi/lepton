using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lepton.Common.GlobalNPCs
{
    class SlimeStaffKingSlime : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.KingSlime;
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!Main.expertMode && !Main.masterMode)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, 4));
            }
        }
    }
}
