#nullable enable

using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon
    {
        protected readonly WeaponData Data;

        protected Weapon(WeaponData data)
        {
            Data = data;
        }

        public abstract void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir,
            Action? onWeaponDestroy = null);
    }
}