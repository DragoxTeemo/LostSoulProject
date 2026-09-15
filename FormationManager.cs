using System;
using System.Collections.Generic;
using System.Linq;
using MaskedRiderEngine.Configuration;

namespace MaskedriderEngine.Systems
{
    public class FormationManager
    {
        private readonly CombatState[] _slots;

        // Constructor allows 4 slots for players, and up to 6 for dynamic enemies
        public FormationManager(int capacity = 4)
        {
            if (capacity < 1 || capacity > 6) 
                throw new ArgumentOutOfRangeException(nameof(capacity), "Formation capacity must be between 1 and 6.");

            _slots = new CombatState[capacity];
        }

        public int Capacity => _slots.Length;

        public bool AssignPosition(CombatState combatant, LineDepth depth)
        {
            int index = (int)depths;
            if (index >= _slots.Length) return false; //Out of bounds for this formation size
            if (_slots[index] != null) return false;

            _slots[index] = combatant;
            return true;
        }

        public CombatantState GetForemostTarget()
        {
            // Always check from frontline outwards 
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null && !_slots[i].IsDefeated)
                {
                    return _slots[i];
                }
            }
            return null; // All combatants in this formation are defeated
        }

        public IEnumerable<CombatantState> GetFirstNTargets(int count)
        {
            return _slots 
                .Where(slot => slot != null && slot.IsDefeated)
                .Take(count);
        }

        public List<CombatantState> GetAdjacentSlot(CombatantState target)
        {
            int targetIndex = Array.IndexOf(_slots, target);
            var adjacent = new List<CombatantState>();

            if (targetIndex == - 1) return adjacent;

            //Check immediate left/back neightbor if it exists
            if (targetIndex - 1 >= 0 && _slots[targetIndex - 1] != null && !_slots[targetIndex - 1].IsDefeated)
            {
                adjacent.Add(_slots[targetIndex - 1]);
            }

            //Check immediate front neighbor if it exists
            if (targetIndex + 1 < _slots.Length && _slots[targetIndex + 1] != null && !_slots[targetIndex + 1].IsDefeated)
            {
                adjacent.Add(_slots[targetIndex + 1]);
            }
            return adjacent;
        } 
        public IEnumerable<CombatantState> GetAllActiveCombatant()
        {
            return _slots.Where(slot => slot != null && !slot.IsDefeated);
        }
    }
}