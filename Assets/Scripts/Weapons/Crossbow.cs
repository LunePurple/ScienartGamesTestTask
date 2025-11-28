#nullable enable

using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public class Crossbow : Weapon
    {
        public Crossbow(CrossbowData data) : base(data) { }

        public override void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir,
            Action? onWeaponDestroy = null)
        {
            Debug.Log("Crossbow attack!");
            onWeaponDestroy?.Invoke();
        }
    }
}