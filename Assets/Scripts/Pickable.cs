#nullable enable

using System;
using Data;
using Unity.Netcode;
using UnityEngine;

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