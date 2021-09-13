using Lepton.Common.Configs;
using Lepton.Common.Players;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Lepton
{
    public class Lepton : Mod
    {
        public const string AssetPath = "Lepton/Assets/";
        public static LeptonServerConfig ServerConfig => ModContent.GetInstance<LeptonServerConfig>();
        public static LeptonClientConfig ClientConfig => ModContent.GetInstance<LeptonClientConfig>();
        public static ModKeybind InstantResearchKeybind;
        private static MethodInfo ShouldItemBeTrashed = null;
        private static MethodInfo ShiftClickSlot = null;

        public override void Load()
        {
            InstantResearchKeybind = KeybindLoader.RegisterKeybind(this, "Instant Research Additional Key", Microsoft.Xna.Framework.Input.Keys.LeftControl);

            IL.Terraria.UI.ItemSlot.OverrideHover_ItemArray_int_int += HookOverrideHover;
            IL.Terraria.Player.HandleBeingInChestRange += HookHandleBeingInChestRange;

            // If AutoTrash is loaded, hooks into ShouldItemBeTrashed and ShiftClickSlot
            if (ModLoader.TryGetMod("AutoTrash", out Mod autoTrash))
            {
                Type autoTrashPlayer = null;

                Assembly autoTrashAssembly = autoTrash.GetType().Assembly;
                foreach (Type t in autoTrashAssembly.GetTypes())
                {
                    if (t.Name == "AutoTrashPlayer")
                    {
                        autoTrashPlayer = t;
                    }
                }

                if (autoTrashPlayer != null)
                {
                    ShouldItemBeTrashed = autoTrashPlayer.GetMethod("ShouldItemBeTrashed", BindingFlags.NonPublic | BindingFlags.Instance);
                    ShiftClickSlot = autoTrashPlayer.GetMethod("ShiftClickSlot", BindingFlags.Public | BindingFlags.Instance);
                }

                if (ShouldItemBeTrashed != null)
                {
                    ModifyShouldItemBeTrashed += HookShouldItemBeTrashed;
                }

                if (ShiftClickSlot != null)
                {
                    ModifyShiftClickSlot += HookShiftClickSlot;
                }
            }
        }

        // IL edit that prevents Terraria from trying to close flying chests when it shouldn't
        private void HookHandleBeingInChestRange(ILContext il)
        {
            var c = new ILCursor(il);

            c.Index = c.Instrs.Count;
            if (!c.TryGotoPrev(i => i.MatchRet()))
            {
                return;
            }

            var label = c.MarkLabel();

            c.Index = 0;

            // Go after the first if statement
            if (!c.TryGotoNext(i => i.MatchLdarg(0)))
            {
                return;
            }

            // Skip whole function if true
            c.EmitDelegate<Func<bool>>(() =>
            {
                return Main.LocalPlayer.GetModPlayer<InteractibleProjectilePlayer>().chest != BetterChest.None;
            });
            c.Emit(OpCodes.Brtrue, label);
        }

        public override void Unload()
        {
            InstantResearchKeybind = null;

            IL.Terraria.UI.ItemSlot.OverrideHover_ItemArray_int_int -= HookOverrideHover;
            if (ShouldItemBeTrashed != null)
            {
                ModifyShouldItemBeTrashed -= HookShouldItemBeTrashed;
            }

            if (ShiftClickSlot != null)
            {
                ModifyShiftClickSlot -= HookShiftClickSlot;
            }
        }

        // IL edit that disables AutoTrash's Ctrl + Shift click function
        private void HookShiftClickSlot(ILContext il)
        {
            var c = new ILCursor(il);

            c.Index = c.Instrs.Count;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(0)))
            {
                return;
            }

            var label = c.MarkLabel();

            c.Index = 0;
            c.EmitDelegate<Func<bool>>(() =>
             {
                 return Lepton.ServerConfig.compatibility.ShouldDisableAutoTrashKeybind;
             });

            c.Emit(OpCodes.Brtrue, label);
        }

        // IL edit that auto trashes fully researched Journey Mode items
        private void HookShouldItemBeTrashed(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(moveType: MoveType.After, i => i.MatchLdcI4(0)))
            {
                return;
            }

            c.Emit(OpCodes.Ldarg_1);
            c.EmitDelegate<Func<int, Item, int>>((returnValue, item) =>
            {
                if (Main.GameModeInfo.IsJourneyMode && Lepton.ServerConfig.compatibility.JourneyAutoTrashEnabled)
                {
                    var fullyResearchedItems = new List<int>();
                    CreativeItemSacrificesCatalog.Instance.FillListOfItemsThatCanBeObtainedInfinitely(fullyResearchedItems);
                    if (fullyResearchedItems.Contains(item.type))
                    {
                        return 1;
                    }
                }
                return returnValue;
            });
        }

        // IL edit that displays the magnifying glass when holding Shift + InstantResearchHotkey
        private void HookOverrideHover(ILContext il)
        {
            var c = new ILCursor(il);

            // Find jump destination to become label
            if (!c.TryGotoNext(i => i.MatchLdsflda(typeof(Main).GetField("keyState"))))
            {
                return;
            }

            // Create label pointing at the destination
            var label = c.MarkLabel();
            c.Index = 0;

            var tempLabel = label;

            // Target the sixth matching break instruction
            for (int i = 0; i < 6; ++i)
            {
                if (!c.TryGotoNext(i => i.MatchBr(out label)))
                {
                    return;
                }
                label = tempLabel;
            }

            c.EmitDelegate<Action>(() =>
            {
                if (Lepton.InstantResearchKeybind.Current && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && Main.GameModeInfo.IsJourneyMode)
                {
                    Main.cursorOverride = 2;
                }
            });
        }

        private static event ILContext.Manipulator ModifyShouldItemBeTrashed
        {
            add
            {
                HookEndpointManager.Modify(ShouldItemBeTrashed, value);
            }
            remove
            {
                HookEndpointManager.Unmodify(ShouldItemBeTrashed, value);
            }
        }

        private static event ILContext.Manipulator ModifyShiftClickSlot
        {
            add
            {
                HookEndpointManager.Modify(ShiftClickSlot, value);
            }
            remove
            {
                HookEndpointManager.Unmodify(ShiftClickSlot, value);
            }
        }
    }
}