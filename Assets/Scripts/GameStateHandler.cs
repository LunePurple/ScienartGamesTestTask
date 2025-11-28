#nullable enable

using System;
using Data;
using Unity.Netcode;
using UnityEngine;

public class GameStateHandler : NetworkBehaviour
{
    public enum GameState
    {
        WaitingForStart = 0,
        Game = 1,
        Result = 2,
    }

    public static event Action<GameState> OnGameStateChanged = delegate { };

    [SerializeField] private GameConfig Config = null!;

    private readonly NetworkVariable<GameState> _currentGameState = new NetworkVariable<GameState>();

    public override void OnNetworkSpawn()
    {
        _currentGameState.OnValueChanged += CurrentGameState_OnValueChanged;
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            Health.OnPlayerDeath += Health_OnPlayerDeath;
        }
    }

    public override void OnNetworkDespawn()
    {
        _currentGameState.OnValueChanged -= CurrentGameState_OnValueChanged;
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
            Health.OnPlayerDeath -= Health_OnPlayerDeath;
        }
    }

    private void AttemptStartGameServer()
    {
        if (_currentGameState.Value != GameState.WaitingForStart) return;
        
        int connectedPlayers = NetworkManager.Singleton.ConnectedClients.Count;
        if (connectedPlayers < Config.MaxPlayers) return;

        _currentGameState.Value = GameState.Game;
    }

    private void StopGameServer()
    {
        if (_currentGameState.Value != GameState.Game) return;
        
        _currentGameState.Value = GameState.Result;
    }
    
    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        AttemptStartGameServer();
    }
    
    private void CurrentGameState_OnValueChanged(GameState previousValue, GameState newValue)
    {
        OnGameStateChanged.Invoke(newValue);
    }
    
    private void Health_OnPlayerDeath(ulong clientId)
    {
        StopGameServer();
    }
}