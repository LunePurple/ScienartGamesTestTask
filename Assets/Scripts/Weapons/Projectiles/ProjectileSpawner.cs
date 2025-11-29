#nullable enable

using Data.Weapons.Projectiles;
using UnityEngine;

namespace Weapons.Projectiles
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public static ProjectileSpawner? Instance { get; private set; }

        private SimpleProjectilePool _simpleProjectilePool = null!;
        
        private void Awake()
        {
            Instance = this;
            _simpleProjectilePool = new SimpleProjectilePool();
        }

        public void SpawnProjectileServer(ProjectileData projectileData, Vector3 position, Vector3 direction,
            LayerMask targetLayerMask, ulong attackerClientId)
        {
            _simpleProjectilePool.Get(projectileData, position, direction, targetLayerMask, attackerClientId);
        }
    }
}