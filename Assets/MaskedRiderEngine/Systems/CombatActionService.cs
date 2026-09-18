using System;
using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class CombatActionService
    {
        // Generic attack. Works for Nirvana, Vector, Falcon, Circuit and every demon. Falcon  has ActionsPerTurn = 2.
        public static CombatExecutionEngine.TurnResult ExecuteAttackAction(
            CombatantState attacker,
            CombatantState target,
            WeaponInstance weapon,
            BattleContext context)
        {
            var result = new CombatExecutionEngine.TurnResult();

            if (attacker == null || weapon == null || context == null)
            {
                result.Log("Invalid action.");
                return result;
            }

            if (!attacker.IsAlive)
            {
                result.Log($"{attacker.Codename} is out of the fight.");
                return result;
            }

            if (attacker.IsIncapacitated)
            {
                result.Log($"{attacker.Codename} cannot act this turn.");
                return result;
            }

            if (!attacker.Economy.HasActionRemaining)
            {
                result.Log($"{attacker.Codename} has no actions remaining this turn.");
                return result;
            }

            if (!weapon.CanFire)
            {
                result.Log($"{weapon.Name} is empty. Reload first.");
                return result;
            }

            if (!LineTargetingService.IsTargetValid(attacker, target, context))
            {
                result.Log($"{attacker.Codename} cannot legally target {target?.Codename ?? "that"}.");
                return result;
            }

            // Only spend the action once every precondition has passed, so a
            // rejected action never costs the player a turn.
            attacker.Economy.TryConsumeAction();

            return CombatExecutionEngine.ExecuteAttack(attacker, target, weapon, context);
        }

        // Reload. Costs a bonus action.
        public static CombatExecutionEngine.TurnResult ExecuteReloadAction(
            CombatantState combatant,
            WeaponInstance weapon)
        {
            var result = new CombatExecutionEngine.TurnResult();

            if (!combatant.IsAlive || combatant.IsIncapacitated)
            {
                result.Log($"{combatant.Codename} cannot act this turn.");
                return result;
            }

            if (!weapon.IsAmmoBased)
            {
                result.Log($"{weapon.Name} does not use ammunition.");
                return result;
            }

            if (!combatant.Economy.HasBonusActionRemaining)
            {
                result.Log($"{combatant.Codename} has no bonus action remaining.");
                return result;
            }

            if (!weapon.TryReload())
            {
                result.Log($"{weapon.Name} could not be reloaded (full clip or no reserve).");
                return result;
            }

            combatant.Economy.TryConsumeBonusAction();
            result.Success = true;
            result.Log($"{combatant.Codename} reloads {weapon.Name}. ({weapon.CurrentAmmo} loaded, {weapon.ReserveAmmo} in reserve)");
            return result;
        }

        // Vector's Rally. Restores PV to armored allies.
        // Lily is excluded because she is on HP, not PV.
        public static CombatExecutionEngine.TurnResult ExecuteVectorRallyAction(
            CombatantState vector,
            BattleContext context)
        {
            var result = new CombatExecutionEngine.TurnResult();

            if (!vector.IsAlive || vector.IsIncapacitated)
            {
                result.Log($"{vector.Codename} cannot act this turn.");
                return result;
            }

            if (!vector.Economy.HasActionRemaining)
            {
                result.Log("Action already used this turn.");
                return result;
            }

            vector.Economy.TryConsumeAction();

            int restored = 0;
            foreach (var ally in context.GetAllies(vector))
            {
                if (!ally.IsAlive) continue;
                if (ally.Codename == GameConfig.CodenameNirvana) continue; // HP, not PV
                if (ally.Profile.UsesHealthPoints) continue;

                int before = ally.Profile.CurrentResources;
                ally.Profile.CurrentResources = Math.Min(
                    ally.Profile.MaxResource,
                    ally.Profile.CurrentResources + GameConfig.VectorRallyPvRestore);
                restored += ally.Profile.CurrentResources - before;
            }

            result.Success = true;
            result.Log($"Vector rallies the team, recovering {restored} total PV.");
            return result;
        }

        // Circuit's device craft. Spends Ash to top up a weapon's reserve.
        public static CombatExecutionEngine.TurnResult ExecuteCraftAction(
            CombatantState circuit,
            WeaponInstance weapon,
            ref int ashPool,
            int roundsPerCraft = 6)
        {
            var result = new CombatExecutionEngine.TurnResult();
            int cost = weapon.Definition.AshCostToCraft;

            if (cost <= 0)
            {
                result.Log($"{weapon.Name} cannot be crafted.");
                return result;
            }

            if (!circuit.Economy.HasActionRemaining)
            {
                result.Log("Action already used this turn.");
                return result;
            }

            if (ashPool < cost)
            {
                result.Log($"Not enough Ash ({ashPool}/{cost}).");
                return result;
            }

            circuit.Economy.TryConsumeAction();
            ashPool -= cost;
            weapon.AddReserve(roundsPerCraft);

            result.Success = true;
            result.Log($"Circuit crafts {roundsPerCraft} rounds of {weapon.Name} for {cost} Ash.");
            return result;
        }
    }
}