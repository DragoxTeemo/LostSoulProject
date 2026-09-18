using System.Collections.Generic;
using MaskedRiderEngine.Core;

namespace MaskedRiderEngine.Models
{
    public class RiderForm
    {
        public string FormName {get; set;} 
        public string element {get; set;}
        public Dictionary<string, double> StatWeight {get; set;} = new Dictionary<string, double>();
    }
    public class RegistryData
    {
        public string Id {get; set;} 
        public string Name {get; set;}
        public string Codename {get; set;}
        public Faction Faction {get; set;} = Faction.Enemy;

        public int BaseResourceMax {get; set;}
        public bool UsesHealthPoints {get; set;} //True for only Lily
        public bool HasPassiveRegen {get; set;}
        public int RegenAmount {get; set;}
        public ArmorWeightClass ArmorClass {get; set;}

        //Combat rules
        public TargetingProfile TargetingProfile {get; set;} = TargetingProfile.Any;
        public int ActionsPerTurn {get; set;} = 1;
        public int BonusActionsPerTurn {get; set;} = 1;
        public List<string> WeaponIds {get; set;} = new List<string>();

        // Information for Circuit 
        public bool HasEngineeringDevices {get; set;}
        public int MaxDevices {get; set;} = 0;

        //Progression/drops
        public int XpReward {get; set;} = 0;
        public int AshReward {get; set;} = 0;

        public Dictionary<string, RiderForm> Forms {get; set;} = new Dictionary<string, RiderForm>();
        public Dictionary<string, double> StatAffinities {get; set;} = new Dictionary<string, double>();

    }
}