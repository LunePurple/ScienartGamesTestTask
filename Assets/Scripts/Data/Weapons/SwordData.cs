#nullable enable

using UnityEngine;
using Weapons;

namespace Data.Weapons
{
    [CreateAssetMenu(menuName = "Data/Weapons/SwordData", fileName = "newSword")]
    public class SwordData : WeaponData
    {
        public int Damage;
        public float AttackRadius;
        public float AttackDistance;

        public override Weapon CreateInstance()
        {
            return new Sword(this);
        }
    }
}