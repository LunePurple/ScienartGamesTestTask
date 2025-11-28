#nullable enable

using System;
using Data;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public struct HealthChangedData
    {
        public readonly int Current;
        public readonly int Max;

        public HealthChangedData(int current, int max)
        {
            Current = current;
            Max = max;
        }
    }

    public static event Action<HealthChangedData> OnLocalPlayerHealthChanged = delegate { };
    public static event Action<ulong> OnPlayerDeath = delegate { };

    [SerializeField] private PlayerConfig Config = null!;

    private bool _isDead;

    private readonly NetworkVariable<int> _health = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            RestoreHealthServer();
        }
        
        if (IsOwner)
        {
            _health.OnValueChanged += Health_OnValueChanged;
            Health_OnValueChanged(_health.Value, _health.Value);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            _health.OnValueChanged -= Health_OnValueChanged;
        }
    }

    private void RestoreHealthServer()
    {
        _health.Value = Config.MaxHealth;
        _isDead = false;
    }

    public void TakeDamageServer(int damage)
    {
        if (_health.Value == 0) return;

        Debug.Log($"Taking {damage} damage!");

        _health.Value = Math.Max(0, _health.Value - damage);

        if (!_isDead && _health.Value == 0)
        {
            DieServer();
        }
    }

    private void DieServer()
    {
        Debug.Log($"Client {OwnerClientId} died!");

        _isDead = true;

        NotifyPlayerDeathClientRpc(OwnerClientId);
    }

    [ClientRpc]
    private void NotifyPlayerDeathClientRpc(ulong deadPlayerClientId)
    {
        OnPlayerDeath.Invoke(deadPlayerClientId);
    }

    private void Health_OnValueChanged(int previousValue, int newValue)
    {
        OnLocalPlayerHealthChanged.Invoke(new HealthChangedData(newValue, Config.MaxHealth));
    }
}