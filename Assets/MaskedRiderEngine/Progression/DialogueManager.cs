using System;
using System.Collections.Generic;
using MaskedRiderEngine.Models;

namespace MaskedRiderEngine.Progression
{
    public class DialogueNode
    {
        public string NodeId {get; set;}
        public string Speaker {get; set;}
        public string DialogueText {get; set;}
        public List<DialogueChoice> Choices { get; set; } = new List<DialogueChoice>();
    }

    public class DialogueChoice
    {
        public string ChoiceText {get; set;}
        public string NextNodeId {get; set;}
        public int BaseSocialLinkPointsAwarded {get; set;} 
    }
    public class DialogueManager
    {
        public int AwardSocialLinkPoints(GameSession session, CombatantState lilyCombatState, DialogueChoice chosen)
        {
            if (session == null || chosen == null) return 0;
            int penalty = session.MentalHealth.GetSocialLinkModifier(lilyCombatState);
            return Math.Max(0, chosen.BaseSocialLinkPointsAwarded + penalty);
        }

    }
}