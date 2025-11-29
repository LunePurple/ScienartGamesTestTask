#nullable enable

using System.Collections.Generic;
using Data;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSpawner : NetworkBehaviour
{
    [SerializeField] private List<WeaponData> Weapons = new List<WeaponData>();
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [Space]
    [SerializeField] private float FillPercent;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GameStateHandler.OnGameStateChanged += GameStateHandler_OnGameStateChanged;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            GameStateHandler.OnGameStateChanged -= GameStateHandler_OnGameStateChanged;
        }
    }

    private void SpawnWeapons()
    {
        if (Weapons.Count == 0) return;

        List<Transform> availablePoints = new List<Transform>(SpawnPoints);

        int weaponsCount = Mathf.RoundToInt(availablePoints.Count * Mathf.Max(FillPercent, 1f));

        for (int i = 0; i < weaponsCount; i++)
        {
            Transform point = availablePoints[Random.Range(0, availablePoints.Count)];
            GameObject weaponPrefab = Weapons[Random.Range(0, Weapons.Count)].PickablePrefab;
            
            GameObject spawnedObject = Instantiate(weaponPrefab, point.position, quaternion.identity);
            spawnedObject.GetComponent<NetworkObject>().Spawn();

            availablePoints.Remove(point);
        }
    }

    private void GameStateHandler_OnGameStateChanged(GameStateHandler.GameState state)
    {
        if (state is GameStateHandler.GameState.Game)
        {
            SpawnWeapons();
        }
    }
}