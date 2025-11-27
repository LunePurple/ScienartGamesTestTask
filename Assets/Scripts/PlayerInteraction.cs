#nullable enable

using Data;
using Unity.Netcode;
using UnityEngine;
using Weapons;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private PlayerConfig Config = null!;
    [Space]
    [SerializeField] private Transform Head = null!;
    [SerializeField] private LayerMask InteractionLayerMask;
    [Space]
    [SerializeField] private WeaponListData WeaponList = null!;
    [SerializeField] private Transform WeaponHoldPoint = null!;

    private PlayerInputProvider _inputProvider = null!;

    private WeaponBehaviour? _currentWeapon;
    
    private readonly NetworkVariable<int> _currentWeaponDataIndex = new NetworkVariable<int>();

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
            if (hit.transform.TryGetComponent(out Weapon pickable))
            {
                if (pickable.TryPickup(out WeaponData? weaponData))
                {
                    EquipWeaponServer(weaponData);
                }
            }
        }
    }

    [ServerRpc]
    private void AttemptUseSelectedWeaponServerRpc()
    {
        _currentWeapon?.Attack(OwnerClientId, WeaponHoldPoint.position, Head.forward, () =>
        {
            EquipWeaponServer();
        });
    }

    private void EquipWeaponServer(WeaponData? data = null)
    {
        if (data is null)
        {
            _currentWeapon = null;
            _currentWeaponDataIndex.Value = -1;
        }
        else
        {
            _currentWeapon = data.CreateInstance();
            _currentWeaponDataIndex.Value = WeaponList.GetWeaponDataIndex(data);
        }
        
        Debug.Log($"Changing weapon... " +
                  $"Weapon:{data?.name}; " +
                  $"WeaponId:{_currentWeaponDataIndex.Value};");
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