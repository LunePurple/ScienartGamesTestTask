#nullable enable

using System.Collections.Generic;
using Data;
using Unity.Netcode;
using UnityEngine;
using Visual;

public class PlayerStateInitializer : NetworkBehaviour
{
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] private List<PlayerVisualData> Visuals = new List<PlayerVisualData>();

    private readonly Dictionary<ulong, int> _clientIdToStateIndex = new Dictionary<ulong, int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnected;
        }
    }

    private int GetFreeStateIndex()
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (!_clientIdToStateIndex.ContainsValue(i))
                return i;
        }

        Debug.LogWarning("No indexes left! Reusing index 0");
        return 0;
    }

    [ClientRpc]
    private void AssignPositionClientRpc(Vector3 position, Quaternion rotation,
        // ReSharper disable once UnusedParameter.Local
        ClientRpcParams clientRpcParams = default)
    {
        NetworkManager.Singleton.LocalClient.PlayerObject.transform.SetPositionAndRotation(position, rotation);
    }
    
    [ClientRpc]
    private void UpdateColorForAllClientRpc(ulong targetClientId, Color color)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out NetworkClient? netClient))
        {
            NetworkObject playerObj = netClient.PlayerObject;
            PlayerVisual? playerVisual = playerObj.GetComponentInChildren<PlayerVisual>();
            playerVisual?.SetPlayerColor(color);
        }
    }

    [ClientRpc]
    // ReSharper disable once UnusedParameter.Local
    private void UpdateColorForClientClientRpc(ulong targetClientId, Color color, ClientRpcParams clientRpcParams)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out var netClient))
        {
            var playerObj = netClient.PlayerObject;
            if (playerObj == null) return;

            var visual = playerObj.GetComponentInChildren<PlayerVisual>();
            visual?.SetPlayerColor(color);
        }
    }

    private void NetworkManager_OnClientConnected(ulong clientId)
    {
        int index = GetFreeStateIndex();
        _clientIdToStateIndex[clientId] = index;

        Vector3 position = SpawnPoints[index].position;
        Quaternion rotation = SpawnPoints[index].rotation;
        Color color = Visuals[index].Color;

        AssignPositionClientRpc(position, rotation, new ClientRpcParams()
        {
            Send = new ClientRpcSendParams()
            {
                TargetClientIds = new[] { clientId }
            }
        });
        
        UpdateColorForAllClientRpc(clientId, color);
        
        foreach ((ulong existingClientId, int stateIndex) in _clientIdToStateIndex)
        {
            if (existingClientId == clientId) continue;

            Color existingColor = Visuals[stateIndex].Color;
            UpdateColorForClientClientRpc(existingClientId, existingColor, new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
            });
        }
    }
}