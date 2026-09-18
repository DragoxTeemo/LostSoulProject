using System;
using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class CombatExecutionEngine
    {
        public class TurnResult
        {
            public List<string> LogLines {get; } = new List<string>();
            public List<StatusEffectType> AppliedStatuses {get; } = new List<StatusEffectType>();

            public bool Success {get; set;}
            public bool Missed {get; set;}
            public int TotalDamage {get; set;}
            public bool LilyRageTriggered {get; set;}
            public bool BattleEnded {get; set;}

            public string LogMessage => string.Join(" ", LogLines);
            public void Log(string line)
            {
                if (!string.IsNullOrWhiteSpace(line)) LogLines.Add(line);
            }
        }

        public static TurnResult ExecuteAttack(CombatantState attacker,
                                               CombatantState primaryTarget,
                                               WeaponInstance weaponInstance,
                                               BattleContext context)
        {
            var result = new TurnResult();
            WeaponDefinition weapon = weaponInstance.Definition;

            if (!attacker.IsAlive)
            {
                result.Log($"{attacker.Codename} cannot act.");
                return result;
            }

            if (attacker.IsIncapacitated)
            {
                result.Log($"{attacker.Codename} is incapacitated and loses the action.");
                return result;
            }

            if (!LineTargetingService.IsTargetValid(attacker, primaryTarget, context))
            {
                result.Log($"{attacker.Codename} cannot reach {primaryTarget?.Codename ?? "that target"} from this position.");
                return result;
            }

            if (!weaponInstance.CanFire)
            {
                result.Log($"{weapon.Name} is empty. Reload required.");
                return result;
            }

            // Accuracy roll (Blind).
            if (!StatusEffectService.RollToHit(attacker))
            {
                weaponInstance.TryConsumeRound();
                result.Missed = true;
                result.Log($"{attacker.Codename} is blinded and misses with {weapon.Name}!");
                return result;
            }

            weaponInstance.TryConsumeRound();
            result.Success = true;

            var targets = TargetResolver.Resolve(attacker, primaryTarget, weapon, context);
            if (targets.Count == 0)
            {
                result.Log("No valid targets remain.");
                return result;
            }

            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;

                int damage = CalculateRawDamage(attacker, weapon, result);
                var outcome = DamageResolver.ApplyDamage(target, damage, context);
                result.TotalDamage += outcome.DealtToPool;

                if (damage == 0)
                {
                    result.Log($"{attacker.Codename} uses {weapon.Name} on {target.Codename}, dealing no damage.");
                }
                else if (outcome.AbsorbedByArmor > 0)
                {
                    result.Log($"{attacker.Codename} strikes {target.Codename}: {outcome.AbsorbedByArmor} absorbed by armor, {outcome.DealtToPool} hits PV!");
                }
                else
                {
                    result.Log($"{attacker.Codename} deals {outcome.DealtToPool} direct damage to {target.Codename}.");
                }

                var applied = StatusEffectService.TryApply(target, weapon);
                if (applied != StatusEffectType.None)
                {
                    result.AppliedStatuses.Add(applied);
                    result.Log($"{target.Codename} is afflicted with {applied}!");
                }

                if (outcome.LilyRageTriggered)
                {
                    result.LilyRageTriggered = true;
                    result.BattleEnded = true;
                    result.Log("LILY'S HP REACHED 0. She enters a blind rage, annihilating all foes and knocking allies out of their armor! (EXP lost. Reloading previous save...)");
                    return result;
                }

                if (outcome.TargetDefeated)
                {
                    result.Log(target.Profile.RequiresArmorRepair
                        ? $"{target.Codename}'s armor shatters! They are forced to retreat."
                        : $"{target.Codename} is destroyed.");
                }
            }

            if (context.IsBattleOver) result.BattleEnded = true;
            return result;
        }

        private static int CalculateRawDamage(CombatantState attacker,
                                              WeaponDefinition weapon,
                                              TurnResult result)
        {
            if (weapon.BasePower <= 0) return 0;

            double damage = weapon.BasePower;

            if (attacker.Codename == GameConfig.CodenameNirvana)
            {
                // Stress sharpens her. Permanent while Stressed or Manic.
                if (attacker.CurrentSanityTier == SanityTier.Stressed ||
                    attacker.CurrentSanityTier == SanityTier.Manic)
                {
                    damage *= GameConfig.StressedDamageMultiplier;
                }
            }
            else if (attacker.Codename == GameConfig.CodenameVector)
            {
                // Momentum scales with turns already spent in this battle.
                double momentum = Math.Min(
                    GameConfig.VectorMomentumCap,
                    1.0 + (attacker.TurnsInBattle * GameConfig.VectorMomentumPerTurn));
                damage *= momentum;

                if (weapon.HasVarianceModifier)
                {
                    if (Rng.Chance(GameConfig.VectorVarianceSuccessChance))
                    {
                        damage *= GameConfig.VectorVarianceHighMultiplier;
                        result.Log("[Vector found a weak spot]");
                    }
                    else
                    {
                        damage *= GameConfig.VectorVarianceLowMultiplier;
                        result.Log("[Vector failed to find a weak spot]");
                    }
                }
            }

            return (int)damage;
        }
    }
}