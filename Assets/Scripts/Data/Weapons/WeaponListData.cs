#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Data.Weapons
{
    [CreateAssetMenu(menuName = "Data/Lists/WeaponListData", fileName = "WeaponList")]
    public class WeaponListData : ScriptableObject
    {
        public List<WeaponData> Weapons = new List<WeaponData>();

        public int GetWeaponDataIndex(WeaponData weaponData)
        {
            return Weapons.IndexOf(weaponData);
        }
        
        public bool TryGetWeaponDataFromIndex(int weaponDataIndex, out WeaponData? weaponData)
        {
            if (weaponDataIndex < 0 || weaponDataIndex >= Weapons.Count)
            {
                weaponData = null;
                return false;
            }
            
            weaponData = Weapons[weaponDataIndex];
            return true;
        }
    }
}