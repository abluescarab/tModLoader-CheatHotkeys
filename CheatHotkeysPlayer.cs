using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    internal class CheatHotkeysPlayer : ModPlayer {
        private readonly int BUFF_TIME = int.MaxValue;

        public override bool Autoload(ref string name) {
            return true;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            CheatHotkeys chmod = (CheatHotkeys)mod;

            if(chmod.GodMode) {
                return false;
            }

            if(chmod.KnockbackDisabled) {
                hitDirection = 0;
            }

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void PreUpdateBuffs() {
            if(((CheatHotkeys)mod).GodMode) {
                RemoveDebuffs();
                RefillMana(false);
            }

            base.PreUpdateBuffs();
        }

        public override bool CanBeHitByProjectile(Projectile proj) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.CanBeHitByProjectile(proj);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo) {
            if(((CheatHotkeys)mod).UnlimitedAmmo) {
                return false;
            }

            return base.ConsumeAmmo(weapon, ammo);
        }

        public void RefillLife() {
            int maxLife = (player.statLifeMax2 > player.statLifeMax ? player.statLifeMax2 : player.statLifeMax);

            player.statLife = maxLife;
            player.HealEffect(maxLife, true);
        }

        public void RefillMana(bool showEffect) {
            int maxMana = (player.statManaMax2 > player.statManaMax ? player.statManaMax2 : player.statManaMax);

            player.statMana = maxMana;

            if(showEffect) {
                player.ManaEffect(maxMana);
            }
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
                            player.ClearBuff(i);
                        }
                        break;
                }
            }
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

            CheatHotkeys chmod = (CheatHotkeys)mod;

            if(chmod.EnabledByHotkey[mode]) {
                player.ClearBuff((int)mode);
                chmod.EnabledByHotkey[mode] = false;
            }
        }

        public void EnableMiningBuff(MiningBuffMode mode) {
            if(mode == MiningBuffMode.All || mode == MiningBuffMode.None) return;

            CheatHotkeys chmod = (CheatHotkeys)mod;

            if(player.FindBuffIndex((int)mode) == -1) {
                chmod.EnabledByHotkey[mode] = true;
            }

            if(chmod.EnabledByHotkey[mode]) {
                player.AddBuff((int)mode, BUFF_TIME, true);
            }
        }
    }
}
