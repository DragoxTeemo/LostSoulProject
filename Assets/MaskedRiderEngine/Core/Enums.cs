namespace MaskedRiderEngine.Core
{
    /// Index 0 is frontline, higher values are further back
    public enum LineDepth
    {
        Frontline = 0,
        Depth1 = 1,
        Depth2 = 2,
        Depth3 = 3, // Max protagonist group (capacity 4)
        Depth4 = 4,
        Depth5 = 5  // Allows up to a 6-foe encounter
    }

    public enum SanityTier
    {
        Sane, // 0-20
        Stressed, // 21-80
        Manic //81-100
    }

    public enum ArmorWeightClass
    {
        None,   //Lily and all demons take the full damage and have no armor
        Light,  // Eva (40% absorbed by armor, 60% bleeds to PV)
        Medium, // Alvin and Elliot (60% absorbed, 40% bleed)
    }
    public enum AttackScope
    {
        SingleTarget,
        MultiTarget,
        AdjacentAOE
    }

    public enum ElementType
    {
        Physical, 
        Fire,
        Electric
    }

    public enum StatusEffectType
    {
        None, 
        Stun,
        Shock,
        Blind, 
        Burn,
        Sleep
    }

    public enum TargetProfile
    {
        Any,    //Nirvana, Circuit, Bosses, Horror Demonborn
        FrontlineOnly, //Vector, BasicDemonborn
        FirstTwo, // Falcon
        BacklineTwo, // ToxicDemonborn
    }

    public enum Faction
    {
        Players,
        Enemy
    }
}