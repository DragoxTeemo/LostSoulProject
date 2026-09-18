using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class LineTargetingService
    {
        public static bool IsTargetValid(CombatantState attacker,
                                         CombatantState target,
                                         BattleContext context)
        {
            if (attacker == null || target == null || context == null) return false;
            if (!attacker.IsAlive) return false;
            if (!target.IsAlive) return false;

            // Living opponents in frontline-first order. Dead members drop out,
            // so survivors shift up in priority automatically.
            List<CombatantState> candidates = context.GetEnemies(attacker);
            int index = candidates.IndexOf(target);
            if (index < 0) return false; // Not a legal opponent at all

            switch (attacker.Profile.Blueprint.TargetingProfile)
            {
                case TargetingProfile.FrontlineOnly:
                    return index == 0;

                case TargetingProfile.FirstTwo:
                    // Valid on index 0 and 1. With one foe left, only index 0 exists.
                    return index <= 1;

                case TargetingProfile.BacklineTwo:
                    // With two or fewer opponents, everyone is reachable.
                    if (candidates.Count <= 2) return true;
                    return index >= candidates.Count - 2;

                case TargetingProfile.Any:
                default:
                    return true;
            }
        }

        public static List<CombatantState> GetValidTargets(CombatantState attacker,
                                                           BattleContext context)
        {
            var valid = new List<CombatantState>();
            foreach (var candidate in context.GetEnemies(attacker))
                if (IsTargetValid(attacker, candidate, context)) valid.Add(candidate);

            return valid;
        }
    }
}