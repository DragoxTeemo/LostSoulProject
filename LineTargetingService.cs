using System.Collections.Generic;
using System.Linq;
using MaskedRiderEngine.Configuration;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Systems
{
    public class LineTargetService
    {
        // Validates if an attacker can legally target a specific enemy based on position rules and weapon scope
        public static bool IsTargetValid(
            CombatantState attacker, 
            CombatantState target, 
            CharacterWeapon weapon,
            List<CombatantState> allAllies,
            List<CombatantState> allEnemies)
        {
            // Ensures program doesn't target dead targets
            if (target.IsDefeated || target.Profile.CurrentResources <= 0) return false;
            string attackerCodename = attacker.Profile.Blueprint.Codename;
            
            //Ensures that if enemies die, the remaining ones shift up in priority
            var livingEnemies = allEnemies
                .Where(e => !e.IsDefeated && e.Profile.CurrentResources > 0)
                .OrderBy(e => e.PositionOrder)
                .ToList();

            var livingAllies = allAllies
                .Where(a => !a.IsDefeated && a.Profile.CurrentResources > 0)
                .OrderBy(a => a.PositionOrder)
                .ToList();
            
            //string targetCodename = target.Profile.Blueprint.Codename;

            
            if (attackerCodename == "Vector" || attackerCodename == "BasicDemonborn")
            {
                var frontlineTarget = livingEnemies.FirstOrDefault();
                return frontlineTarget == target;
            }

            if (attackerCodename == "Falcon")
            {
                int targetIndex = livingEnemies.IndexOf(target);
                
                //Valid if they are index 0 and 1 (first and second enemy)
                // If there is 1 threat left, targetIndex will target 0
                return targetIndex >= 0 && targetIndex <= 1;
            }

            if (attackerCodename == "RangedDemonborn")
            {
                // Find the index of the target in the living allies queue
                int targetAllyIndex = livingAllies.IndexOf(target);

                // If there are fewer than 2 allies left, target all remaining. Other target last 2
                int totalLivingAllies = livingAllies.Count;
                if (totalLivingAllies <= 2)
                {
                    return targetAllyIndex >= 0; 
                }

                //Valid if they are among the last two
                return targetAllyIndex >= totalLivingAllies - 2;
            }
            // Default for Nirvana, Circuit and boss, targets anyone on the board.
            return true;
        }
    }    
}