using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    public class CheatHotkeys : Mod {
        private bool godMode = false;
        private bool unlimitedAmmo = false;

        private ModHotKey lifeKey;
        private ModHotKey manaKey;
        private ModHotKey removeDebuffsKey;
        private ModHotKey godModeKey;
        private ModHotKey unlimitedAmmoKey;

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

            lifeKey = RegisterHotKey("Refill Life", Keys.Z.ToString());
            manaKey = RegisterHotKey("Refill Mana", Keys.X.ToString());
            removeDebuffsKey = RegisterHotKey("Remove Debuffs", Keys.C.ToString());
            godModeKey = RegisterHotKey("Toggle God Mode", Keys.F.ToString());
            unlimitedAmmoKey = RegisterHotKey("Toggle Unlimited Ammo", Keys.G.ToString());
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
