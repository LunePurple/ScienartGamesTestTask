#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Weapons/CrossbowData", fileName = "newCrossbow")]
    public class CrossbowData : WeaponData
    {
        public override void Attack()
        {
            Debug.Log("Crossbow attack!");
        }
    }
}