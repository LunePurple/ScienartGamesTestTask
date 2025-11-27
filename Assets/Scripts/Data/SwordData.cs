#nullable enable

using UnityEngine;
using Weapons;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Weapons/SwordData", fileName = "newSword")]
    public class SwordData : WeaponData
    {
        public int Damage;
        public float AttackRadius;
        public float AttackDistance;

        public override WeaponBehaviour CreateInstance()
        {
            return new Sword(this);
        }
    }
}