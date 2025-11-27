#nullable enable

using UnityEngine;
using Weapons;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Weapons/CrossbowData", fileName = "newCrossbow")]
    public class CrossbowData : WeaponData
    {
        public GameObject ArrowPrefab = null!;

        public override WeaponBehaviour CreateInstance()
        {
            return new Crossbow(this);
        }
    }
}