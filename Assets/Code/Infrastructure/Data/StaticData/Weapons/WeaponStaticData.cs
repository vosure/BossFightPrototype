using Code.Gameplay.WeaponSystem;
using UnityEngine;

namespace Code.Infrastructure.Data.StaticData.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/WeaponData", order = 0)]
    public class WeaponStaticData : ScriptableObject
    {
        public WeaponTypeID TypeID;
        
        public FireMode FireMode;
        public int BulletsPerMagazine;
        public float FireRate;
        public float ReloadTime;
        public float AccuracyErrorValue;
        public int Damage;
    }
}
