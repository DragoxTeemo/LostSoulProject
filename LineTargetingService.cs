using System.Collections.Generic;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public class LineTargetService
    {
        // Validates if an attacker can legally target a specific enemy based on position rules and weapon scope
        public static bool IsTargetValid(CombatantState attacker, CombatantState target, CharacterWeapon weapon)
        {
            if (target.IsDefeated || target.Profile.CurrentResources <= 0) return false;
            string attackerCodename = attacker.Profile.Blueprint.Codename;
            string targetCodename = target.Profile.Blueprint.Codename;

            // Nirvana and Circuit target from any position
            if (attackerCodename == "Nirvana" || attackerCodename == "Circuit")
            {
                if (weapon.Scope == AttackScope.AdjacentAOE)
                {
                    ///<summary>
                    /// We need to create a program that checks if there are adjacent to the target, if yes, they take damage
                    /// if there are none, prevent an overflow/underflow from occuring when Lily attacks the far most left or right Demon
                    ///</summary>
                }
                return true;
            }

            if (attackerCodename == "Vector")
            {
                return target.PositionOrder == 1;
            }

            if (attackerCodename == "Falcon")
            {
                return target.PositionOrder <= 2;
            }

            // Create Demons where Basic Demon attacks only front, Horror Demonborn attacks any target, 
            // Ranged Demon attacks the last two positions
        }
    }    
}