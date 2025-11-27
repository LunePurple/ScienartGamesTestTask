#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private PlayerConfig Config = null!;

    private readonly NetworkVariable<int> _health = new NetworkVariable<int>();
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            RestoreHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking {damage} damage!");
    }
    
    private void RestoreHealth()
    {
        _health.Value = Config.MaxHealth;
    }
}