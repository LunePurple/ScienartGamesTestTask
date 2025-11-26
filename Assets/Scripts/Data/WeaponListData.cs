#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Lists/WeaponListData", fileName = "WeaponList")]
    public class WeaponListData : ScriptableObject
    {
        public List<WeaponData> Weapons = new List<WeaponData>();

        public int GetWeaponDataIndex(WeaponData weaponData)
        {
            int index = Weapons.IndexOf(weaponData);
            return index != -1 ? index : 0; // fallback index
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