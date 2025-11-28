#nullable enable

using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public class Sword : Weapon
    {
        private readonly Collider[] _hits = new Collider[10];

        public Sword(SwordData data) : base(data) { }

        public override void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir,
            Action? onWeaponDestroy = null)
        {
            Debug.Log("Sword attack!");

            SwordData swordData = (SwordData)Data;
            int hitsNumber = Physics.OverlapSphereNonAlloc(holdPoint, swordData.AttackRadius, _hits,
                swordData.TargetLayerMask);

            for (int i = 0; i < hitsNumber; i++)
            {
                if (_hits[i].transform.TryGetComponent(out Health health))
                {
                    if (health.NetworkObject.IsPlayerObject && health.OwnerClientId == attackerClientId) continue;

                    health.TakeDamageServer(swordData.Damage);
                }
            }

            onWeaponDestroy?.Invoke();
        }
    }
}