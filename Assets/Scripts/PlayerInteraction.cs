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
    [SerializeField] private Transform WeaponHoldPoint = null!;

    private PlayerInputProvider _inputProvider = null!;

    private Inventory<Weapon>? _inventory;

    private readonly NetworkVariable<int> _selectedInventorySlotIndex = new NetworkVariable<int>();

    private void Awake()
    {
        _inputProvider = GetComponent<PlayerInputProvider>();
        _inventory = new Inventory<Weapon>(Config.InventorySize);
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
        if (_inventory == null || !_inventory.HasEmptySlot(out _)) return;
        
        if (Physics.Raycast(Head.position, Head.forward, out RaycastHit hit, Config.InteractionDistance,
                InteractionLayerMask))
        {
            if (hit.transform.TryGetComponent(out Pickable pickable))
            {
                pickable.AttemptPickup(data =>
                {
                    _inventory?.TryPickupItem(data.CreateInstance());
                });
            }
        }
    }

    [ServerRpc]
    private void ChangeSelectedSlotServerRpc(int slotNumber)
    {
        _selectedInventorySlotIndex.Value = slotNumber - 1;
        Debug.Log($"Selecting slot #{slotNumber}");
    }

    [ServerRpc]
    private void AttemptUseSelectedWeaponServerRpc()
    {
        if (_inventory != null && _inventory.TryGetFromSlot(_selectedInventorySlotIndex.Value, out Weapon? weapon))
        {
            weapon?.Attack(OwnerClientId, WeaponHoldPoint.position, Head.forward, () =>
            {
                _inventory.TryRemoveFromSlot(_selectedInventorySlotIndex.Value);
            });
        }
    }

    private void InputProvider_OnPickupAction()
    {
        AttemptPickupWeaponServerRpc();
    }

    private void InputProvider_OnSelectAction(int slotNumber)
    {
        ChangeSelectedSlotServerRpc(slotNumber);
    }

    private void InputProvider_OnUseAction()
    {
        AttemptUseSelectedWeaponServerRpc();
    }
}