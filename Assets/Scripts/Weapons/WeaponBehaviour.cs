#nullable enable

using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBehaviour
    {
        protected readonly WeaponData Data;

        protected WeaponBehaviour(WeaponData data)
        {
            Data = data;
        }

        public abstract void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir, Action onWeaponDestroy);
    }
}