#nullable enable

using System;
using Data.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace Weapons
{
    public class Pickable : NetworkBehaviour
    {
        [SerializeField] private WeaponData Data = null!;
    
        public void AttemptPickup(Action<WeaponData> onPickup)
        {
            onPickup(Data);
        
            if (TryGetComponent(out NetworkObject networkObject))
            { 
                networkObject.Despawn();
                Destroy(gameObject);
            }
        }
    }
}