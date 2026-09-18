using System.Collections.Generic;
using MaskedRiderEngine.Core;
 
namespace MaskedRiderEngine.Models
{
    public class MaskedRiderEntity
    {
        public RegistryData Blueprint {get;}
        public string InstanceId {get; set;}
        public int Level {get; set;} = 1;
        public int Xp {get; set;} = 0;

        public int CurrentResources {get; set;} // HP for Lily, PV for everyone else
        public int ArmorIntegrityState { get; set; } = GameConfig.MaxArmorIntegrity;
        public List<WeaponInstance> Weapons { get; } = new List<WeaponInstance>();
        public MaskedRiderEntity(RegistryData blueprint, string InstanceId)
        {
            Blueprint = blueprint;
            InstanceId = instanceId;
            CurrentResources = blueprint.BaseResourceMax;
            ArmorIntegrityState = blueprint.RequiresArmorRepair? GameConfig.MaxArmorIntegrity: 0;
 
            foreach (var weaponId in blueprint.WeaponIds)
                Weapons.Add(new WeaponInstance(WeaponCatalog.Get(weaponId)));

        }
        public string Codename => Blueprint.Codename;
        public string Name => Blueprint.Name;
        public int MaxResource => Blueprint.BaseResourceMax;
        public bool UsesHealthPoints => Blueprint.UsesHealthPoints;
        public bool HasPassiveRegen => Blueprint.HasPassiveRegen;
        public int RegenAmount => Blueprint.RegenAmount;
        public bool RequiresArmorRepair => Blueprint.RequiresArmorRepair;
        public ArmorWeightClass ArmorClass => Blueprint.ArmorClass;
        public Faction Faction => Blueprint.Faction;
 
        public WeaponInstance GetWeapon(string weaponId)
            => Weapons.Find(w => w.Definition.Id == weaponId);

    }
}