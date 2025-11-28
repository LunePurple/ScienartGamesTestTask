#nullable enable

using UnityEngine;
using Weapons;

namespace Data
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