using System;
using System.Collections.Generic;
using MaskedRiderEngine.Core;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public static class EncounterBuilder
    {
        public static CombatantState CreateRider(string codename, string instanceId = null)
        {
            var blueprint = RiderRegistry.Get(codename);
            var entity = new MaskedRiderEntity(blueprint, instanceId ?? Guid.NewGuid().ToString());
            return new CombatantState(entity);
        }

        public static CombatantState CreateDemon(string codename, string instanceId = null)
        {
            var blueprint = DemonRegistry.Get(codename);
            var entity = new MaskedRiderEntity(blueprint, instanceId ?? Guid.NewGuid().ToString());
            return new CombatantState(entity);
        }

        public static BattleContext BuildEncounter(IList<string> partyCodenames,
                                                   IList<string> demonCodenames)
        {
            if (partyCodenames.Count > GameConfig.PlayerFormationCapacity)
                throw new ArgumentException(
                    $"Party cannot exceed {GameConfig.PlayerFormationCapacity} members.");

            if (demonCodenames.Count > GameConfig.MaxFormationCapacity)
                throw new ArgumentException(
                    $"Encounters cannot exceed {GameConfig.MaxFormationCapacity} foes.");

            var context = new BattleContext();

            foreach (var codename in partyCodenames)
                context.PlayerFormation.AssignNextFree(CreateRider(codename));

            foreach (var codename in demonCodenames)
                context.EnemyFormation.AssignNextFree(CreateDemon(codename));

            return context;
        }

        public static (int xp, int ash) CalculateRewards(IEnumerable<CombatantState> defeatedEnemies)
        {
            int xp = 0, ash = 0;
            foreach (var enemy in defeatedEnemies)
            {
                xp += enemy.Profile.Blueprint.XpReward;
                ash += enemy.Profile.Blueprint.AshReward;
            }
            return (xp, ash);
        }
    }
}