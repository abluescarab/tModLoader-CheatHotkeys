using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CheatHotkeys {
    internal class CheatHotkeysPlayer : ModPlayer {
        public override bool Autoload(ref string name) {
            return true;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
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
            CheatHotkeys chmod = (CheatHotkeys)mod;

            if(chmod.GodMode) {
                RemoveDebuffs();
                player.statMana = player.statManaMax;
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
            player.statLife = player.statLifeMax;
            player.HealEffect(player.statLifeMax, true);
        }

        public void RefillMana() {
            player.statMana = player.statManaMax;
            player.ManaEffect(player.statManaMax);
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
                        if(Main.debuff[i])
                            player.ClearBuff(i);
                        break;
                }
            }
        }
    }
}
