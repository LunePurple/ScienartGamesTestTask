#nullable enable

using UnityEngine;
using Weapons;

namespace Data.Weapons
{
    public abstract class WeaponData : ScriptableObject
    {
        public GameObject PickablePrefab = null!;
        public LayerMask TargetLayerMask;

        public abstract Weapon CreateInstance();
    }
}