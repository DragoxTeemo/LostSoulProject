using System.Collections.Generic;

namespace MaskedRiderEngine.Models
{
    public enum AttackScope
    {
        SingleTarget,
        MultiTarget,
        AdjacentAOE
    }

    public enum ElementType
    {
        Physical,
        Electric,
        Fire
    }

    public class CharacterWeapon
    {
        public string Name {get; set;}
        public int BasePower {get; set;} 
        public AttackScope Scope {get; set;}
        public ElementType Element {get; set; }

    }
}