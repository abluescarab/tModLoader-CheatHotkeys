using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    internal class CheatHotkeysSystem : ModSystem {
        public static bool MoneyKeyPressed = false;
        public static double MoneyKeyPressTime = 0.0;
        public static int MoneyKeySpeedMultiplier = 1;
        public static double MoneyKeyIncreaseTime = 0.0;

        public static bool MiningBuffKeyPressed = false;
        public static double MiningBuffKeyPressTime = 0.0;

        public static ModKeybind LifeKey;
        public static ModKeybind ManaKey;
        public static ModKeybind RemoveDebuffsKey;
        public static ModKeybind MiningBuffKey;
        public static ModKeybind GodModeKey;
        public static ModKeybind UnlimitedAmmoKey;
        public static ModKeybind MoneyKey;
        public static ModKeybind DisableKnockbackKey;

        public static bool GodMode { get; set; }
        public static bool UnlimitedAmmo { get; set; }
        public static bool KnockbackDisabled { get; set; }
        public static MiningBuffMode MiningBuff { get; set; }
        public static Dictionary<MiningBuffMode, bool> EnabledByHotkey { get; private set; }

        public override void Load() {
            LifeKey = KeybindLoader.RegisterKeybind(Mod, "Refill Life", Keys.Z.ToString());
            ManaKey = KeybindLoader.RegisterKeybind(Mod, "Refill Mana", Keys.X.ToString());
            RemoveDebuffsKey = KeybindLoader.RegisterKeybind(Mod, "Remove Debuffs", Keys.C.ToString());
            MiningBuffKey = KeybindLoader.RegisterKeybind(Mod, "Mining Buff Mode", Keys.V.ToString());
            GodModeKey = KeybindLoader.RegisterKeybind(Mod, "Toggle God Mode", Keys.F.ToString());
            UnlimitedAmmoKey = KeybindLoader.RegisterKeybind(Mod, "Toggle Unlimited Ammo", Keys.G.ToString());
            MoneyKey = KeybindLoader.RegisterKeybind(Mod, "Give Money", Keys.P.ToString());
            DisableKnockbackKey = KeybindLoader.RegisterKeybind(Mod, "Disable Knockback", Keys.N.ToString());

            EnabledByHotkey = new Dictionary<MiningBuffMode, bool>(3);
            EnabledByHotkey.Add(MiningBuffMode.Dangersense, false);
            EnabledByHotkey.Add(MiningBuffMode.Hunter, false);
            EnabledByHotkey.Add(MiningBuffMode.Spelunker, false);
        }

        public override void PostUpdateInput() {
            GameTime time = Main._drawInterfaceGameTime;
            CheatHotkeysPlayer player = Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>();

            if(MiningBuffKeyPressed && (time.TotalGameTime.TotalMilliseconds - MiningBuffKeyPressTime) >= 500.0) {
                player.ToggleMiningBuffModes();
                Main.NewText("Mining modes have all been " + (MiningBuff == MiningBuffMode.All ? "enabled" : "disabled") + "!");
                MiningBuffKeyPressed = false;
            }
            else if(MiningBuffKeyPressed && !MiningBuffKey.Current) {
                player.CycleMiningBuffMode();
                Main.NewText("Mining buff set to " + MiningBuff.ToString() + "!");
                MiningBuffKeyPressed = false;
            }

            if(MoneyKeyPressed) {
                if(!MoneyKey.Current) {
                    MoneyKeySpeedMultiplier = 1;
                    Main.NewText("Gave you lots of money!");
                    MoneyKeyPressed = false;
                }
                else {
                    if((time.TotalGameTime.TotalMilliseconds - MoneyKeyIncreaseTime) >= 1000.0 & (MoneyKeySpeedMultiplier < 32)) {
                        MoneyKeySpeedMultiplier *= 2;
                        MoneyKeyIncreaseTime = time.TotalGameTime.TotalMilliseconds;
                    }

                    if((time.TotalGameTime.TotalMilliseconds - MoneyKeyPressTime) >= (1000.0 / MoneyKeySpeedMultiplier)) {
                        player.GiveMoney();
                        MoneyKeyPressTime = time.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
        }

        //public void RefillLife() {
        //    Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RefillLife();
        //}

        //public void RefillMana() {
        //    Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RefillMana(true);
        //}

        //public void RemoveDebuffs() {
        //    Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RemoveDebuffs();
        //}

        //public void GiveMoney() {
        //    Player player = Main.LocalPlayer;
        //    player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemID.PlatinumCoin, 10);
        //}

        //public void CycleMiningBuffMode() {
        //    MiningBuffMode lastMode = MiningBuff;

        //    switch(MiningBuff) {
        //        case MiningBuffMode.Dangersense:
        //            MiningBuff = MiningBuffMode.Hunter;
        //            break;
        //        case MiningBuffMode.Hunter:
        //            MiningBuff = MiningBuffMode.Spelunker;
        //            break;
        //        case MiningBuffMode.Spelunker:
        //            MiningBuff = MiningBuffMode.Dangersense;
        //            break;
        //        default:
        //            MiningBuff = MiningBuffMode.Dangersense;
        //            break;
        //    }

        //    Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().UpdateMiningBuffs(MiningBuff);
        //}

        //public void ToggleMiningBuffModes() {
        //    Player player = Main.LocalPlayer;
        //    int dangersense = player.FindBuffIndex((int)MiningBuffMode.Dangersense);
        //    int hunter = player.FindBuffIndex((int)MiningBuffMode.Hunter);
        //    int spelunker = player.FindBuffIndex((int)MiningBuffMode.Spelunker);

        //    MiningBuff = (MiningBuff == MiningBuffMode.None ? MiningBuffMode.All : MiningBuffMode.None);

        //    if(dangersense == -1 && hunter == -1 && spelunker == -1 && MiningBuff == MiningBuffMode.None) {
        //        MiningBuff = MiningBuffMode.All;
        //    }
        //    else if(dangersense >= 0 && hunter >= 0 && spelunker >= 0 && MiningBuff == MiningBuffMode.All) {
        //        MiningBuff = MiningBuffMode.None;
        //    }

        //    Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().UpdateMiningBuffs(MiningBuff);
        //}
    }
}
