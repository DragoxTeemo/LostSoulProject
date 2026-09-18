using System;
using System.Collections.Generic;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine
{
    public class CharacterActionEconomy
    {
        public bool HasUsedAction {get; set;} = false;
        public bool HasUsedBonusAction {get; set;} = false;

        //Specific tracker for Falcon's dual action
        public int FalconActionSpent {get; set;} = 0;
        public const int MaxFalconActions = 2;

        public void ResetTurn()
        {
            HasUsedAction = false;
            HasUsedBonusAction = false;
            FalconActionSpent = 0;
        }
    }
}