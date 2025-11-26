#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Weapons/SwordData", fileName = "newSword")]
    public class SwordData : WeaponData
    {
        public override void Attack()
        {
            Debug.Log("Sword attack!");
        }
    }
}