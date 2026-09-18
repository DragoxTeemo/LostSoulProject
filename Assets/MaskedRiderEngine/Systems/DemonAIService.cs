using System.Collections.Generic;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class DemonAIService
    {
        public static CombatExecutionEngine.TurnResult TakeTurn(CombatantState demon,
                                                                BattleContext context)
        {
            var result = new CombatExecutionEngine.TurnResult();

            if (!demon.IsAlive || demon.IsIncapacitated)
            {
                result.Log($"{demon.Codename} cannot act.");
                return result;
            }

            List<CombatantState> validTargets = LineTargetingService.GetValidTargets(demon, context);
            if (validTargets.Count == 0)
            {
                result.Log($"{demon.Codename} has no reachable targets.");
                return result;
            }

            CombatantState target = validTargets[Core.Rng.Next(0, validTargets.Count)];
            WeaponInstance weapon = PickWeapon(demon);

            if (weapon == null)
            {
                result.Log($"{demon.Codename} has no usable weapon.");
                return result;
            }

            return CombatActionService.ExecuteAttackAction(demon, target, weapon, context);
        }
        private static WeaponInstance PickWeapon(CombatantState demon)
        {
            var usable = demon.Profile.Weapons.FindAll(w => w.CanFire);
            if (usable.Count == 0) return null;
            return usable[Core.Rng.Next(0, usable.Count)];
        }
    }
}