#nullable enable

using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public class Sword : WeaponBehaviour
    {
        private readonly Collider[] _hits = new Collider[10];

        public Sword(SwordData data) : base(data) { }

        public override void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir, Action onWeaponDestroy)
        {
            Debug.Log("Sword attack!");
            onWeaponDestroy();
        }
    }
}