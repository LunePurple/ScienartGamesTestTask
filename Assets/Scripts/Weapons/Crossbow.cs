#nullable enable

using System;
using Data;
using UnityEngine;
using Weapons.Projectiles;

namespace Weapons
{
    public class Crossbow : Weapon
    {
        public Crossbow(CrossbowData data) : base(data) { }

        public override void Attack(ulong attackerClientId, Vector3 holdPoint, Vector3 lookDir,
            Action? onWeaponDestroy = null)
        {
            Debug.Log("Crossbow attack!");

            CrossbowData crossbowData = (CrossbowData)Data;
            ProjectileSpawner.Instance?.SpawnProjectileServer(crossbowData.ProjectileData, holdPoint, 
                lookDir, Data.TargetLayerMask, attackerClientId);

            onWeaponDestroy?.Invoke();
        }
    }
}