#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;

namespace Weapons.Projectiles
{
    public class Projectile : NetworkBehaviour
    {
        private const float LIFETIME_MAX = 10f;
        
        private ProjectileData? _data;
        private LayerMask _targetLayerMask;
        private ulong _attackerClientId;

        private bool _isInitialized;
        private float _lifetime;

        private void Update()
        {
            if (!IsServer) return;
            if (!_isInitialized) return;

            _lifetime += Time.deltaTime;
            if (_lifetime >= LIFETIME_MAX)
            {
                DestroySelf();
            }
            
            HandleMoveAndHit();
        }

        public void Initialize(ProjectileData projectileData, LayerMask targetLayerMask, ulong attackerClientId)
        {
            _isInitialized = true;

            _data = projectileData;
            _targetLayerMask = targetLayerMask;
            _attackerClientId = attackerClientId;
        }

        private void HandleMoveAndHit()
        {
            Vector3 move = transform.forward * (_data!.MovementSpeed * Time.deltaTime);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, move.magnitude,
                    _targetLayerMask))
            {
                if (hit.transform.TryGetComponent(out Health health))
                {
                    if (health.NetworkObject.IsPlayerObject && health.OwnerClientId == _attackerClientId) return;
                    
                    Debug.Log("Projectile attack!");
                    
                    health.TakeDamageServer(_data!.Damage);
                }

                DestroySelf();
            }
            
            transform.position += move;
        }

        private void DestroySelf()
        {
            NetworkObject.Despawn();
            Destroy(gameObject);
        }
    }
}