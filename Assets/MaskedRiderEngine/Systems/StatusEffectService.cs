using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class StatusEffectService
    {
        // Rolls a weapon's status chance against a target. Returns the effect applied, or None.
        public static StatusEffectType TryApply(CombatantState target, WeaponDefinition weapon)
        {
            if (weapon.StatusEffect == StatusEffectType.None) return StatusEffectType.None;
            if (weapon.StatusEffectChance <= 0.0) return StatusEffectType.None;
            if (target == null || !target.IsAlive) return StatusEffectType.None;

            if (!Rng.Chance(weapon.StatusEffectChance)) return StatusEffectType.None;

            target.ApplyStatus(weapon.StatusEffect, StatusEffect.DefaultDuration(weapon.StatusEffect));
            return weapon.StatusEffect;
        }

        public static bool RollToHit(CombatantState attacker)
        {
            if (attacker.HasStatus(StatusEffectType.Blind))
                return !Rng.Chance(GameConfig.BlindMissChance);

            return true;
        }

        // Damage-over-time, applied at the start of the carrier's turn.
        public static void ApplyTickDamage(CombatantState combatant, BattleContext context)
        {
            if (!combatant.IsAlive) return;

            int total = 0;
            foreach (var status in combatant.ActiveStatuses)
                total += StatusEffect.TickDamage(status.Type);

            if (total > 0) DamageResolver.ApplyDirectDamage(combatant, total, context);
        }

        /// Landing a hit wakes a sleeping target. Without this, Drowsy Smoke would lock an enemy out permanently.
        public static void OnDamaged(CombatantState target)
        {
            if (target.HasStatus(StatusEffectType.Sleep))
                target.ClearStatus(StatusEffectType.Sleep);
        }
    }
}