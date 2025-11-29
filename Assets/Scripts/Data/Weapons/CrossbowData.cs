#nullable enable

using Data.Weapons.Projectiles;
using UnityEngine;
using Weapons;

namespace Data.Weapons
{
    [CreateAssetMenu(menuName = "Data/Weapons/CrossbowData", fileName = "newCrossbow")]
    public class CrossbowData : WeaponData
    {
        public ProjectileData ProjectileData = null!;

        public override Weapon CreateInstance()
        {
            return new Crossbow(this);
        }
    }
}