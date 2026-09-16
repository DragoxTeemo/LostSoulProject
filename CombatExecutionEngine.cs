using System;
using System.Collections.Generic;
using MaskedRiderEngine;

namespace MaskedRiderEngine.Systems
{
    public class CombatExecutionEngine
    {
        private const double mediumArmorReduction = 0.60;
        private const double lightArmorReduction = 0.40;
        private static readonly Random _rng = new Random();

        public class TurnResult
        {
            public string LogMessage {get; set;}
            public bool LilyRageTriggered {get;set;}
            public bool BattleEnded {get; set;}
            public List<string> AppliedStatuses {get; set;} = new List<string>();
        }

        public static TurnResult ExecuteAttack(
            CombatantState attacker,
            CombatantState target,
            CharacterWeapon weapon,
            List<CombatantState> allAllies,
            List<CombatantState> allEnemies)
        {
            var result = new TurnResult();
            int rawDamage = weapon.BasePower > 0 ? weapon.BasePower : 10;

            string attackerCodename = attacker.Profile.Blueprint.Codename;
            string targetCodename = target.Profile.Blueprint.Codename;

            // Apply Character Specific Character Modifier
            if (attackerCodename == "Nirvana")
            {
                // Damage increases for lily
                attacker.Sanity += 2;
                attacker.Sanity = Math.Min(GameConfig.MaxSanity, attacker.Sanity);

                //Once Lily reaches her stress threshold (21+), her damage increases and remains at 10%
                if (attacker.CurrentSanityTier == SanityTier.Stressed || attacker.CurrentSanityTier == SanityTier.Manic)
                {
                    rawDamage = (int)(rawDamage * 1.10); // Permanent +10% damage boost while stressed/manic
                }
            }
            else if (attackerCodename == "Vector")
            {
                // Alvin: Momentum scaling over combat turns
                attacker.TurnsInBattle++;
                double momentumBonus = Math.Min(1.5, 1.0 + (attacker.TurnsInBattle * 0.05));
                rawDamage = (int)(rawDamage * momentumBonus);

                if (weapon.HasVarianceModifier)
                {
                    double roll = _rng.NextDouble();
                    if (roll < 0.60)
                    {
                        rawDamage = (int)(rawDamage * 1.25); //60% chance to deal more damage
                        result.LogMessage += "[Vector found a weak spot]";
                    }
                    else
                    {
                        rawDamage = (int)(rawDamage * 0.80); // 40% chance to deal less damage
                        result.LogMessage += "Failed to find weak spot";
                    }
                }
            }

            if (targetCodename == "Nirvana" || target.Profile.ArmorClass == ArmorWeightClass.None)
            {
                // Direct HP pool (Lily/Demons)
                target.Profile.CurrentResources = Math.Max(0, target.Profile.CurrentResources - rawDamage);
                result.LogMessage = $"{attackerCodename} deals {rawDamage} directdamage to {targetCodename}.";
            }
            else
            {
                double mitigationRate = (target.Profile.ArmorClass == ArmorWeightClass.Light) ? LightArmorReduction : MediumArmorReduction;
                
                int absorbedByArmor = (int)(rawDamage * mitigationRate);
                int bleedingToPv = rawDamage - absorbedByArmor;

                if (target.Profile.ArmorIntegrityState >= absorbedByArmor)
                {
                    target.Profile.ArmorIntegrityState -= absorbedByArmor;
                }
                else
                {
                    absorbedByArmor = target.Profile.ArmorIntegrityState;
                    target.Profile.ArmorIntegrityState = 0;
                    bleedingToPv = rawDamage - absorbedByArmor;
                }

                target.Profile.CurrentResources = Math.Max(0, target.Profile.CurrentResources - bleedingToPv);
                result.LogMessage = $"{attackerCodename} strikes {targetCodename}: {absorbedByArmor} absorbed by armor, {bleedingToPv} hits PV!";

                // check PV Retreat Condition (once PV reaches 0)
                if (target.Profile.CurrentResources <= 0)
                {
                    result.LogMessage += $" {targetCodename}'s armor shatters! They are forced to retreat.";
                }
            }

            //Checks if Lily's HP reaches 0
            if (targetCodename == "Nirvana" && target.Profile.CurrentResources <= 0)
            {
                result.LilyRageTriggered = true;
                result.BattleEnded = true;
                result.LogMessage = "LILY'S HP REACHED 0. She enters a blind rage, annihilating all foes and knocking allies out of their armor! (EXP lost. Reloading previous save...)";
            }

            return result;
        }
    }
}