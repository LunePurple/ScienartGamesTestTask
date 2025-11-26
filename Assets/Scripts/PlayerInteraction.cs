#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private PlayerConfig Config = null!;
    [Space]
    [SerializeField] private Transform Head = null!;
    [SerializeField] private LayerMask InteractionLayerMask;
    [Space]
    [SerializeField] private WeaponListData WeaponList = null!;

    private PlayerInputProvider _inputProvider = null!;

    private readonly NetworkVariable<int> _weaponIndex = new NetworkVariable<int>();

    private void Awake()
    {
        _inputProvider = GetComponent<PlayerInputProvider>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputProvider.OnPickupAction += InputProvider_OnPickupAction;
        _inputProvider.OnSelectAction += InputProvider_OnSelectAction;
        _inputProvider.OnUseAction += InputProvider_OnUseAction;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputProvider.OnPickupAction -= InputProvider_OnPickupAction;
        _inputProvider.OnSelectAction -= InputProvider_OnSelectAction;
        _inputProvider.OnUseAction -= InputProvider_OnUseAction;
    }

    [ServerRpc]
    private void AttemptPickupWeaponServerRpc()
    {
        if (Physics.Raycast(Head.position, Head.forward, out RaycastHit hit, Config.InteractionDistance,
                InteractionLayerMask))
        {
            if (hit.transform.TryGetComponent(out Weapon weapon))
            {
                int index = WeaponList.GetWeaponDataIndex(weapon.WeaponData);
                EquipWeaponWithIndex(index);
                
                if (weapon.TryGetComponent(out NetworkObject networkObject))
                { 
                    networkObject.Despawn();
                }
            }
        }
    }

    [ServerRpc]
    private void AttemptUseSelectedWeaponServerRpc()
    {
        if (WeaponList.TryGetWeaponDataFromIndex(_weaponIndex.Value, out WeaponData? weaponData))
        {
            weaponData?.Attack();
            EquipWeaponWithIndex();
        }
    }

    private void EquipWeaponWithIndex(int newWeaponIndex = 0)
    {
        _weaponIndex.Value = newWeaponIndex;
        
        WeaponList.TryGetWeaponDataFromIndex(_weaponIndex.Value, out WeaponData? newData);
        Debug.Log($"Changing weapon... " +
                  $"Weapon:{newData?.name}; " +
                  $"WeaponId:{_weaponIndex.Value};");
    }

    private void InputProvider_OnPickupAction()
    {
        AttemptPickupWeaponServerRpc();
    }

    private void InputProvider_OnSelectAction(int slotNumber)
    {
        Debug.Log($"Selecting slot #{slotNumber - 1}");
    }

    private void InputProvider_OnUseAction()
    {
        AttemptUseSelectedWeaponServerRpc();
    }
}