using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class TargetResolver
    {
        public static List<CombatantState> Resolve(
            CombatantState attacker,
            CombatantState primaryTarget,
            WeaponDefinition weapon,
            BattleContext context)
        {
            var targets = new List<CombatantState>();
            if (primaryTarget == null || !primaryTarget.IsAlive) return targets;

            FormationManager targetFormation = context.OpposingFormationOf(attacker);

            switch (weapon.Scope)
            {
                case AttackScope.SingleTarget:
                    targets.Add(primaryTarget);
                    break;

                case AttackScope.MultiTarget:
                    targets.AddRange(targetFormation.GetLivingInOrder());
                    break;

                case AttackScope.AdjacentAOE:
                    targets.Add(primaryTarget);
                    foreach (var neighbour in targetFormation.GetAdjacentSlots(primaryTarget))
                        if (!targets.Contains(neighbour)) targets.Add(neighbour);
                    break;
            }
            return targets;
        }
    }
}