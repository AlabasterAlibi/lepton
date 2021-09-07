using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader;

namespace Lepton.Common.Players
{
    class QuickResearchPlayer : ModPlayer
    {
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (Main.cursorOverride == 2 && Main.GameModeInfo.IsJourneyMode && !Main.CreativeMenu.Blocked && !Main.drawingPlayerChat)
            {
                if (!Main.CreativeMenu.Enabled)
                {
                    Main.CreativeMenu.ToggleMenu();
                }
                var powersMenu = typeof(CreativeUI).GetField("_uiState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.CreativeMenu);
                typeof(UICreativePowersMenu).GetMethod("ToggleMainCategory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(powersMenu, new object[] { 2 }); // Opens research menu
                typeof(UICreativePowersMenu).GetMethod("RefreshElementsOrder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(powersMenu, null); // Updates to display it

                Main.CreativeMenu.SwapItem(ref inventory[slot]);
                SoundEngine.PlaySound(7);

                var itemsDisplay = typeof(UICreativePowersMenu).GetField("_infiniteItemsWindow", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(powersMenu);
                typeof(UICreativeInfiniteItemsDisplay).GetMethod("sacrificeButton_OnClick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(itemsDisplay, new object[] { null, null }); // Parameters aren't used in method

                // Prevents menu from being closed after research
                if (!Main.CreativeMenu.IsShowingResearchMenu())
                {
                    typeof(UICreativePowersMenu).GetMethod("ToggleMainCategory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(powersMenu, new object[] { 2 }); // Opens research menu again
                    typeof(UICreativePowersMenu).GetMethod("RefreshElementsOrder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(powersMenu, null); // Updates to display it again
                }
                return true;
            }
            return false;
        }
    }
}
