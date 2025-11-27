#nullable enable

using UnityEngine;
using Weapons;

namespace Data
{
    public abstract class WeaponData : ScriptableObject
    {
        public LayerMask TargetLayerMask;

        public abstract WeaponBehaviour CreateInstance();
    }
}