#nullable enable

using System.Collections.Generic;
using Data.Weapons.Projectiles;
using Unity.Netcode;
using UnityEngine;

namespace Weapons.Projectiles
{
    public class SimpleProjectilePool
    {
        private readonly Dictionary<ProjectileData, Queue<Projectile>> _pools = new();

        public Projectile Get(ProjectileData projectileData, Vector3 position, Vector3 direction, LayerMask targetLayerMask,
            ulong attackerClientId)
        {
            if (!_pools.TryGetValue(projectileData, out Queue<Projectile> queue))
            {
                queue = new Queue<Projectile>();
                _pools[projectileData] = queue;
            }

            Projectile projectile;
            if (queue.Count > 0)
            {
                projectile = queue.Dequeue();
                projectile.transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
                projectile.gameObject.SetActive(true);
            }
            else
            {
                GameObject obj = Object.Instantiate(projectileData.Prefab, position, Quaternion.LookRotation(direction));
                projectile = obj.GetComponent<Projectile>();
            }
        
            projectile.Initialize(this, targetLayerMask, attackerClientId);
            if (projectile.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Spawn();
            }

            return projectile;
        }

        public void Release(Projectile projectile)
        {
            if (projectile.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Despawn(false);
            }

            projectile.gameObject.SetActive(false);

            _pools[projectile.Data].Enqueue(projectile);
        }
    }
}