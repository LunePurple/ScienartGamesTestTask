#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;

public class Pickable : NetworkBehaviour
{
    [SerializeField] private WeaponData Data = null!;

    public bool TryPickup(out WeaponData? data)
    {
        if (TryGetComponent(out NetworkObject networkObject))
        { 
            networkObject.Despawn();
        }

        data = Data;
        return true;
    }
}