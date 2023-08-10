using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    internal class CheatHotkeysPlayer : ModPlayer {
        private readonly int BUFF_TIME = int.MaxValue;

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if(CheatHotkeysSystem.LifeKey.JustPressed) {
                RefillLife();
                Main.NewText("Life refilled!");
            }
            else if(CheatHotkeysSystem.ManaKey.JustPressed) {
                RefillMana(true);
                Main.NewText("Mana refilled!");
            }
            else if(CheatHotkeysSystem.RemoveDebuffsKey.JustPressed) {
                RemoveDebuffs();
                Main.NewText("Removed debuffs!");
            }
            else if(CheatHotkeysSystem.GodModeKey.JustPressed) {
                ToggleGodMode();
                Main.NewText("God mode has been " + (CheatHotkeysSystem.GodMode ? "enabled" : "disabled") + "!");
            }
            else if(CheatHotkeysSystem.UnlimitedAmmoKey.JustPressed) {
                CheatHotkeysSystem.UnlimitedAmmo = !CheatHotkeysSystem.UnlimitedAmmo;
                Main.NewText("Unlimited ammo has been " + (CheatHotkeysSystem.UnlimitedAmmo ? "enabled" : "disabled") + "!");
            }
            else if(CheatHotkeysSystem.MiningBuffKey.JustPressed) {
                CheatHotkeysSystem.MiningBuffKeyPressed = true;
                CheatHotkeysSystem.MiningBuffKeyPressTime = Main._drawInterfaceGameTime.TotalGameTime.TotalMilliseconds;
            }
            else if(CheatHotkeysSystem.MoneyKey.JustPressed) {
                GiveMoney();
                CheatHotkeysSystem.MoneyKeyPressed = true;
                CheatHotkeysSystem.MoneyKeyPressTime = Main._drawInterfaceGameTime.TotalGameTime.TotalMilliseconds;
            }
            else if(CheatHotkeysSystem.DisableKnockbackKey.JustPressed) {
                CheatHotkeysSystem.KnockbackDisabled = !CheatHotkeysSystem.KnockbackDisabled;
                Main.NewText("Knockback has been " + (CheatHotkeysSystem.KnockbackDisabled ? "disabled" : "enabled") + "!");
            }
        }

        public override void PostHurt(Player.HurtInfo info) {
            if(CheatHotkeysSystem.GodMode) {
                Player.QuickHeal();
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers) {
            if(CheatHotkeysSystem.GodMode) {
                modifiers.DisableDust();
                modifiers.DisableSound();
                modifiers.SetMaxDamage(0); // set to 0 in case the 1 minimum changes
            }

            if(CheatHotkeysSystem.KnockbackDisabled) {
                modifiers.HitDirectionOverride = 0;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if(CheatHotkeysSystem.GodMode) {
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void PreUpdateBuffs() {
            if(CheatHotkeysSystem.GodMode) {
                RemoveDebuffs();
                RefillMana(false);
            }

            base.PreUpdateBuffs();
        }

        public override bool CanBeHitByProjectile(Projectile proj) {
            if(CheatHotkeysSystem.GodMode) {
                return false;
            }

            return base.CanBeHitByProjectile(proj);
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo) {
            if(CheatHotkeysSystem.UnlimitedAmmo) {
                return false;
            }

            return base.CanConsumeAmmo(weapon, ammo);
        }

        public void ToggleGodMode() {
            CheatHotkeysSystem.GodMode = !CheatHotkeysSystem.GodMode;

            if(CheatHotkeysSystem.GodMode) {
                RefillLife();
                RemoveDebuffs();
            }
        }

        public void RefillLife() {
            int maxLife = (Player.statLifeMax2 > Player.statLifeMax ? Player.statLifeMax2 : Player.statLifeMax);

            Player.statLife = maxLife;
            Player.HealEffect(maxLife, true);
        }

        public void RefillMana(bool showEffect) {
            int maxMana = (Player.statManaMax2 > Player.statManaMax ? Player.statManaMax2 : Player.statManaMax);

            Player.statMana = maxMana;

            if(showEffect) {
                Player.ManaEffect(maxMana);
            }
        }

        public void GiveMoney() {
            Player.QuickSpawnItem(Player.GetSource_DropAsItem(), ItemID.PlatinumCoin, 10);
        }

        public void RemoveDebuffs() {
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
                        if(Main.debuff[i]) {
                            Player.ClearBuff(i);
                        }
                        break;
                }
            }
        }

        public void CycleMiningBuffMode() {
            switch(CheatHotkeysSystem.MiningBuff) {
                case MiningBuffMode.Dangersense:
                    CheatHotkeysSystem.MiningBuff = MiningBuffMode.Hunter;
                    break;
                case MiningBuffMode.Hunter:
                    CheatHotkeysSystem.MiningBuff = MiningBuffMode.Spelunker;
                    break;
                case MiningBuffMode.Spelunker:
                    CheatHotkeysSystem.MiningBuff = MiningBuffMode.Dangersense;
                    break;
                case MiningBuffMode.All:
                case MiningBuffMode.None:
                default:
                    CheatHotkeysSystem.MiningBuff = MiningBuffMode.Dangersense;
                    break;
            }

            UpdateMiningBuffs(CheatHotkeysSystem.MiningBuff);
        }

        public void ToggleMiningBuffModes() {
            int dangersense = Player.FindBuffIndex((int)MiningBuffMode.Dangersense);
            int hunter = Player.FindBuffIndex((int)MiningBuffMode.Hunter);
            int spelunker = Player.FindBuffIndex((int)MiningBuffMode.Spelunker);

            CheatHotkeysSystem.MiningBuff = (CheatHotkeysSystem.MiningBuff == MiningBuffMode.None ? MiningBuffMode.All : MiningBuffMode.None);

            if(dangersense == -1 && hunter == -1 && spelunker == -1 && CheatHotkeysSystem.MiningBuff == MiningBuffMode.None) {
                CheatHotkeysSystem.MiningBuff = MiningBuffMode.All;
            }
            else if(dangersense >= 0 && hunter >= 0 && spelunker >= 0 && CheatHotkeysSystem.MiningBuff == MiningBuffMode.All) {
                CheatHotkeysSystem.MiningBuff = MiningBuffMode.None;
            }

            UpdateMiningBuffs(CheatHotkeysSystem.MiningBuff);
        }

        public void UpdateMiningBuffs(MiningBuffMode mode) {
            if(mode == MiningBuffMode.All) {
                EnableMiningBuff(MiningBuffMode.Dangersense);
                EnableMiningBuff(MiningBuffMode.Hunter);
                EnableMiningBuff(MiningBuffMode.Spelunker);
            }
            else if(mode == MiningBuffMode.None) {
                DisableMiningBuff(MiningBuffMode.Dangersense);
                DisableMiningBuff(MiningBuffMode.Hunter);
                DisableMiningBuff(MiningBuffMode.Spelunker);
            }
            else {
                if(mode == MiningBuffMode.Dangersense)
                    EnableMiningBuff(MiningBuffMode.Dangersense);
                else
                    DisableMiningBuff(MiningBuffMode.Dangersense);

                if(mode == MiningBuffMode.Hunter)
                    EnableMiningBuff(MiningBuffMode.Hunter);
                else
                    DisableMiningBuff(MiningBuffMode.Hunter);

                if(mode == MiningBuffMode.Spelunker)
                    EnableMiningBuff(MiningBuffMode.Spelunker);
                else
                    DisableMiningBuff(MiningBuffMode.Spelunker);
            }
        }

        public void DisableMiningBuff(MiningBuffMode mode) {
            if(mode == MiningBuffMode.All || mode == MiningBuffMode.None) return;

            if(CheatHotkeysSystem.EnabledByHotkey[mode]) {
                Player.ClearBuff((int)mode);
                CheatHotkeysSystem.EnabledByHotkey[mode] = false;
            }
        }

        public void EnableMiningBuff(MiningBuffMode mode) {
            if(mode == MiningBuffMode.All || mode == MiningBuffMode.None) return;
            
            if(Player.FindBuffIndex((int)mode) == -1) {
                CheatHotkeysSystem.EnabledByHotkey[mode] = true;
            }

            if(CheatHotkeysSystem.EnabledByHotkey[mode]) {
                Player.AddBuff((int)mode, BUFF_TIME, true);
            }
        }
    }
}
