using System;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class DamageResolver
    {
        public class DamageOutcome
        {
            public int AbsorbedByArmor {get; set;}
            public int DealtToPool {get; set;}
            public bool ArmorShattered {get; set;}
            public bool TargetDefeated {get; set;}
            public bool LilyRageTriggered {get; set;}
        }

        // Bypasses armor entirely. Used by damage-over-time.
        public static DamageOutcome ApplyDirectDamage(CombatantState target, int amount,
                                                      BattleContext context)
        {
            var outcome = new DamageOutcome { DealtToPool = Math.Max(0, amount) };

            target.Profile.CurrentResources =
                Math.Max(0, target.Profile.CurrentResources - outcome.DealtToPool);

            FinaliseDefeat(target, outcome, context);
            return outcome;
        }

        // Routes damage through armor where the target has any. Unarmored (Lily, all demons) take it straight to the pool.
        public static DamageOutcome ApplyDamage(CombatantState target, int rawDamage,
                                                BattleContext context)
        {
            var outcome = new DamageOutcome();
            if (rawDamage < 0) rawDamage = 0;

            bool unarmored = target.Profile.ArmorClass == ArmorWeightClass.None
                             || !target.Profile.RequiresArmorRepair;

            if (unarmored)
            {
                outcome.DealtToPool = rawDamage;
                target.Profile.CurrentResources =
                    Math.Max(0, target.Profile.CurrentResources - rawDamage);
            }
            else
            {
                double mitigationRate = target.Profile.ArmorClass == ArmorWeightClass.Light
                    ? GameConfig.LightArmorReduction
                    : GameConfig.MediumArmorReduction;

                int intendedAbsorb = (int)(rawDamage * mitigationRate);

                int actuallyAbsorbed = Math.Min(intendedAbsorb, target.Profile.ArmorIntegrityState);
                target.Profile.ArmorIntegrityState -= actuallyAbsorbed;

                int bleed = rawDamage - actuallyAbsorbed;

                outcome.AbsorbedByArmor = actuallyAbsorbed;
                outcome.DealtToPool = bleed;
                outcome.ArmorShattered = target.Profile.ArmorIntegrityState <= 0;

                target.Profile.CurrentResources =
                    Math.Max(0, target.Profile.CurrentResources - bleed);
            }

            if (rawDamage > 0) StatusEffectService.OnDamaged(target);

            FinaliseDefeat(target, outcome, context);
            return outcome;
        }

        private static void FinaliseDefeat(CombatantState target, DamageOutcome outcome,
                                           BattleContext context)
        {
            if (target.Profile.CurrentResources > 0) return;

            // Lily on HP hitting zero is the loss condition, not a retreat.
            if (target.Codename == GameConfig.CodenameNirvana)
            {
                outcome.LilyRageTriggered = true;
                outcome.TargetDefeated = true;
                target.MarkDefeated();
                return;
            }

            outcome.TargetDefeated = true;
            target.MarkDefeated();

            // Retreating combatants free their slot so the line closes up.
            context?.FormationOf(target)?.Vacate(target);
        }
    }
}