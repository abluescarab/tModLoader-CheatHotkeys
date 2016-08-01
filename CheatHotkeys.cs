using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace CheatHotkeys {
    public class CheatHotkeys : Mod {
        private bool godMode = false;
        private bool unlimitedAmmo = false;

        private HotKey lifeKey = new HotKey("Refill Life", Keys.Z);
        private HotKey manaKey = new HotKey("Refill Mana", Keys.X);
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
            RegisterHotKey(godModeKey.Name, godModeKey.DefaultKey.ToString());
            RegisterHotKey(unlimitedAmmoKey.Name, unlimitedAmmoKey.DefaultKey.ToString());
        }

        public override void HotKeyPressed(string name) {
            if(PlayerInput.Triggers.JustPressed.KeyStatus[GetTriggerName(name)]) {
                if(name.Equals(lifeKey.Name)) {
                    RefillLife();
                }
                else if(name.Equals(manaKey.Name)) {
                    RefillMana();
                }
                else if(name.Equals(godModeKey.Name)) {
                    ToggleGodMode();
                }
                else if(name.Equals(unlimitedAmmoKey.Name)) {
                    ToggleUnlimitedAmmo();
                }
            }
        }

        public void RefillLife() {
            Player player = Main.player[Main.myPlayer];
            player.statLife = player.statLifeMax;
            player.HealEffect(player.statLifeMax, true);
            Main.NewText("Life refilled!");
        }

        public void RefillMana() {
            Player player = Main.player[Main.myPlayer];
            player.statMana = player.statManaMax;
            player.ManaEffect(player.statManaMax);
            Main.NewText("Mana refilled!");
        }

        public void ToggleGodMode() {
            GodMode = !GodMode;
            Main.NewText("God mode has been " + (GodMode ? "enabled" : "disabled") + "!");
        }

        public void ToggleUnlimitedAmmo() {
            UnlimitedAmmo = !UnlimitedAmmo;
            Main.NewText("Unlimited ammo has been " + (UnlimitedAmmo  ? "enabled" : "disabled") + "!");
        }

        public string GetTriggerName(string name) {
            return Name + ": " + name;
        }
    }
}
