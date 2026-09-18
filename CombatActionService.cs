using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace MaskedRiderEngine.Systems
{
    public class CombatActionService
    {
        public static readonly Random _rng = new Random();
        public static bool TryReloadWeapon(CharacterWeapon weapon)
        {
            if (weapon.MaxAmmo == 0) return false; //Melee weapons

            int ammoNeeded = weapon.MaxAmmo - weapon.CurrentAmmo;
            if (ammoNeeded <= 0) return false; //Full clip already

            if (weapon.ReserveAmmo <= 0) return false; //Out of reserve ammo

            int ammoToReload = Math.Min(ammoNeeded, weapon.ReserveAmmo);
            weapon.CurrentAmmo += ammoToReload;
            weapon.ReserveAmmo -= ammoToReload;

            return true;

        }
        public static CombatExecutionEngine.TurnResult ExecuteVectorAction(
            CombatantState attacker,
            CombatantState target,
            List<CombatantState> allAllies, 
            List<CombatantState> allEnemies,
            CharacterActionEconomy economy)
        {
            if (economy.HasUsedAction) 
                return new CombatExecutionEngine.TurnResult {LogMessage = "Action already used this turn."};
            economy.HasUsedAction = true;
            return CombatExecutionEngine.ExecuteAttack(attacker, target, weapon, allAllies, allEnemies);
        }

        public static string ExecuteVectorRallyAction(List<CombatantState> allAllies, CharacterActionEconomy economy)
        {
            if (economy.HasUsedAction)
                return "Action already used this turn.";
            economy.HasUsedAction = true;
            int pvRestoreAmount = 15;

            foreach (var ally in allAllies)
            {
                if (ally.Profile.Blueprint.Codename != Nirvana && ally.Profile.CurrentResources > 0)
                {
                    ally.Profile.CurrentResources = Math.Min(
                        ally.Profile.MaxResource, ally.Profile.CurrentResources + pvRestoreAmount
                    );
                }
            }
            return "Vector used his action to rally the ream, recovering team PV.";
        }
        public static CombatExecutionEngine.TurnResult ExecuteFalconAttackAction(
            CombatantState attacker, 
            CombatantState target, 
            CharacterWeapon weapon, 
            List<CombatantState> allAllies, 
            List<CombatantState> allEnemies,
            CharacterActionEconomy economy)
        {
            if (economy.FalconActionSpent >= CharacterActionEconomy.MaxFalconActions)
            {
                return new CombatExecutionEngine.TurnResult {LogMessage = "Falcon has already exhausted her action."};
            }

            economy.FalconActionSpent++;
            return new CombatExecutionEngine.TurnResult { LogMessage = "Falcon has already exhausted her actions this turn."};
        }
    }
}