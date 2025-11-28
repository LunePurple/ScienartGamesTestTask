#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;

namespace Weapons.Projectiles
{
    public class ProjectileSpawner : MonoBehaviour
    {
        public static ProjectileSpawner? Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnProjectileServer(ProjectileData projectileData, Vector3 position, Vector3 direction,
            LayerMask targetLayerMask, ulong attackerClientId)
        {
            Debug.Log("Projectile spawn!");
        
            GameObject spawnedObject = Instantiate(projectileData.Prefab, position, Quaternion.LookRotation(direction));
            if (spawnedObject.TryGetComponent(out Projectile projectile))
            {
                projectile.Initialize(projectileData, targetLayerMask, attackerClientId);
            }
            if(spawnedObject.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.Spawn();
            }
        }
    }
}