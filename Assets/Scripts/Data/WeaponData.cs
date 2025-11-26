#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Weapons/_WeaponData", fileName = "_EmptyWeapon")]
    public class WeaponData : ScriptableObject
    {
        public virtual void Attack()
        {
            Debug.Log("Empty weapon...");
        }
    }
}