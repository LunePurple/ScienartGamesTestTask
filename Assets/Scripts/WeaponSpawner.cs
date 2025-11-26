#nullable enable

using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawner : NetworkBehaviour
{
    [SerializeField] private Transform Prefab = null!;
    [SerializeField] private Transform SpawnPoint = null!;
    
    [SerializeField] private Transform Prefab2 = null!;
    [SerializeField] private Transform SpawnPoint2 = null!;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Transform testObject1 = Instantiate(Prefab, SpawnPoint.position, quaternion.identity);
            testObject1.GetComponent<NetworkObject>().Spawn();
            
            Transform testObject2 = Instantiate(Prefab2, SpawnPoint2.position, quaternion.identity);
            testObject2.GetComponent<NetworkObject>().Spawn();
        }
    }
}