using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CheatHotkeys {
    internal class MPlayer : ModPlayer {
        public override bool Autoload(ref string name) {
            return true;
        }

        public override void PostUpdateEquips() {
            if(((CheatHotkeys)mod).GodMode) {
                player.statMana = player.statManaMax;
            }
            base.PostUpdateEquips();
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref string deathText) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref deathText);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref string deathText) {
            if(((CheatHotkeys)mod).GodMode) {
                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref deathText);
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
    }
}
