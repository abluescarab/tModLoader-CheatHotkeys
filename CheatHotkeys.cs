using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace CheatHotkeys {
    public class CheatHotkeys : Mod {
        private bool godMode = false;
        private bool unlimitedAmmo = false;

        private HotKey lifeKey = new HotKey("Refill Life", Keys.Z);
        private HotKey manaKey = new HotKey("Refill Mana", Keys.X);
        private HotKey removeDebuffs = new HotKey("Remove Debuffs", Keys.C);
        private HotKey godModeKey = new HotKey("Toggle God Mode", Keys.F);
        private HotKey unlimitedAmmoKey = new HotKey("Toggle Unlimited Ammo", Keys.G);

        public bool GodMode {
            get { return godMode; }
            set { godMode = value; }
        }

        public bool UnlimitedAmmo {
            get { return unlimitedAmmo; }
            set { unlimitedAmmo = value; }
        }
        
        public override void Load() {
            Properties = new ModProperties() {
                Autoload = true
            };

            RegisterHotKey(lifeKey.Name, lifeKey.DefaultKey.ToString());
            RegisterHotKey(manaKey.Name, manaKey.DefaultKey.ToString());
            RegisterHotKey(removeDebuffs.Name, removeDebuffs.DefaultKey.ToString());
            RegisterHotKey(godModeKey.Name, godModeKey.DefaultKey.ToString());
            RegisterHotKey(unlimitedAmmoKey.Name, unlimitedAmmoKey.DefaultKey.ToString());
        }

        public override void HotKeyPressed(string name) {
            if(PlayerInput.Triggers.JustPressed.KeyStatus[GetTriggerName(name)]) {
                if(name.Equals(lifeKey.Name)) {
                    RefillLife();
                    Main.NewText("Life refilled!");
                }
                else if(name.Equals(manaKey.Name)) {
                    RefillMana();
                    Main.NewText("Mana refilled!");
                }
                else if(name.Equals(removeDebuffs.Name)) {
                    RemoveDebuffs();
                    Main.NewText("Removed debuffs!");
                }
                else if(name.Equals(godModeKey.Name)) {
                    ToggleGodMode();
                    Main.NewText("God mode has been " + (GodMode ? "enabled" : "disabled") + "!");
                }
                else if(name.Equals(unlimitedAmmoKey.Name)) {
                    ToggleUnlimitedAmmo();
                    Main.NewText("Unlimited ammo has been " + (UnlimitedAmmo ? "enabled" : "disabled") + "!");
                }
            }
        }

        public void RefillLife() {
            Player player = Main.player[Main.myPlayer];
            player.statLife = player.statLifeMax;
            player.HealEffect(player.statLifeMax, true);
        }

        public void RefillMana() {
            Player player = Main.player[Main.myPlayer];
            player.statMana = player.statManaMax;
            player.ManaEffect(player.statManaMax);
        }

        public void RemoveDebuffs() {
            Player player = Main.player[Main.myPlayer];

            for(int i = 0; i < Main.debuff.Length; i++) {
                switch(i) {
                    case BuffID.Horrified:      // fighting Wall of Flesh
                    case BuffID.TheTongue:      // in contact with Wall of Flesh tongue
                    case BuffID.Obstructed:     // attacked by a Brain Suckler
                    case BuffID.Suffocation:    // in contact with silt/sand/slush
                    case BuffID.Burning:        // in contact with hot blocks
                    case BuffID.WaterCandle:    // around a water candle
                        break;
                    default:
                        if(Main.debuff[i])
                            player.ClearBuff(i);
                        break;
                }
            }
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

        public string GetTriggerName(string name) {
            return Name + ": " + name;
        }
    }
}
