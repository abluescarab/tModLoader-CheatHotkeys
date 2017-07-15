using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    public class CheatHotkeys : Mod {
        private ModHotKey lifeKey;
        private ModHotKey manaKey;
        private ModHotKey removeDebuffsKey;
        private ModHotKey miningBuffKey;
        private ModHotKey godModeKey;
        private ModHotKey unlimitedAmmoKey;

        private bool miningBuffKeyPressed = false;
        private double miningBuffKeyPressTime = 0.0;

        public bool GodMode { get; set; }
        public bool UnlimitedAmmo { get; set; }
        public MiningBuffMode MiningBuff { get; set; }
        public Dictionary<MiningBuffMode, bool> EnabledByHotkey { get; private set; }

        public override void Load() {
            Properties = new ModProperties() {
                Autoload = true
            };

            lifeKey = RegisterHotKey("Refill Life", Keys.Z.ToString());
            manaKey = RegisterHotKey("Refill Mana", Keys.X.ToString());
            removeDebuffsKey = RegisterHotKey("Remove Debuffs", Keys.C.ToString());
            miningBuffKey = RegisterHotKey("Mining Buff Mode", Keys.V.ToString());
            godModeKey = RegisterHotKey("Toggle God Mode", Keys.F.ToString());
            unlimitedAmmoKey = RegisterHotKey("Toggle Unlimited Ammo", Keys.G.ToString());

            EnabledByHotkey = new Dictionary<MiningBuffMode, bool>(3);
            EnabledByHotkey.Add(MiningBuffMode.Dangersense, false);
            EnabledByHotkey.Add(MiningBuffMode.Hunter, false);
            EnabledByHotkey.Add(MiningBuffMode.Spelunker, false);
        }

        public override void HotKeyPressed(string name) {
            if(lifeKey.JustPressed) {
                RefillLife();
                Main.NewText("Life refilled!");
            }
            else if(manaKey.JustPressed) {
                RefillMana();
                Main.NewText("Mana refilled!");
            }
            else if(removeDebuffsKey.JustPressed) {
                RemoveDebuffs();
                Main.NewText("Removed debuffs!");
            }
            else if(godModeKey.JustPressed) {
                ToggleGodMode();
                Main.NewText("God mode has been " + (GodMode ? "enabled" : "disabled") + "!");
            }
            else if(unlimitedAmmoKey.JustPressed) {
                ToggleUnlimitedAmmo();
                Main.NewText("Unlimited ammo has been " + (UnlimitedAmmo ? "enabled" : "disabled") + "!");
            }
            else if(miningBuffKey.JustPressed) {
                miningBuffKeyPressed = true;
                miningBuffKeyPressTime = Main._drawInterfaceGameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public override void PostUpdateInput() {
            GameTime time = Main._drawInterfaceGameTime;

            if(miningBuffKeyPressed && (time.TotalGameTime.TotalMilliseconds - miningBuffKeyPressTime) >= 500.0) {
                ToggleMiningBuffModes();
                Main.NewText("Mining modes have all been " + (MiningBuff == MiningBuffMode.All ? "enabled" : "disabled") + "!");
                miningBuffKeyPressed = false;
            }
            else if(miningBuffKeyPressed && !miningBuffKey.Current) {
                CycleMiningBuffMode();
                Main.NewText("Mining buff set to " + MiningBuff.ToString() + "!");
                miningBuffKeyPressed = false;
            }
        }

        public void RefillLife() {
            Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RefillLife();
        }

        public void RefillMana() {
            Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RefillMana();
        }

        public void RemoveDebuffs() {
            Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().RemoveDebuffs();
        }

        public void ToggleGodMode() {
            GodMode = !GodMode;

            if(GodMode) {
                RefillLife();
                RemoveDebuffs();
            }
        }

        public void ToggleUnlimitedAmmo() {
            UnlimitedAmmo = !UnlimitedAmmo;
        }

        public void CycleMiningBuffMode() {
            MiningBuffMode lastMode = MiningBuff;

            switch(MiningBuff) {
                case MiningBuffMode.Dangersense:
                    MiningBuff = MiningBuffMode.Hunter;
                    break;
                case MiningBuffMode.Hunter:
                    MiningBuff = MiningBuffMode.Spelunker;
                    break;
                case MiningBuffMode.Spelunker:
                    MiningBuff = MiningBuffMode.Dangersense;
                    break;
                default:
                    MiningBuff = MiningBuffMode.Dangersense;
                    break;
            }

            Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().UpdateMiningBuffs(MiningBuff);
        }

        public void ToggleMiningBuffModes() {
            Player player = Main.LocalPlayer;
            int dangersense = player.FindBuffIndex((int)MiningBuffMode.Dangersense);
            int hunter = player.FindBuffIndex((int)MiningBuffMode.Hunter);
            int spelunker = player.FindBuffIndex((int)MiningBuffMode.Spelunker);

            MiningBuff = (MiningBuff == MiningBuffMode.None ? MiningBuffMode.All : MiningBuffMode.None);

            if(dangersense == -1 && hunter == -1 && spelunker == -1 && MiningBuff == MiningBuffMode.None) {
                MiningBuff = MiningBuffMode.All;
            }
            else if(dangersense >= 0 && hunter >= 0 && spelunker >= 0 && MiningBuff == MiningBuffMode.All) {
                MiningBuff = MiningBuffMode.None;
            }

            Main.LocalPlayer.GetModPlayer<CheatHotkeysPlayer>().UpdateMiningBuffs(MiningBuff);
        }
    }
}
