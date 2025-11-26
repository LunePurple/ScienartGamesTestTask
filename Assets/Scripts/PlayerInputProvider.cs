#nullable enable

using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : NetworkBehaviour
{
    public event Action OnPickupAction = delegate { };
    public event Action<int> OnSelectAction = delegate { };
    public event Action OnUseAction = delegate { };
    
    private InputActions _inputs = null!;

    private void Awake()
    {
        _inputs = new InputActions();
        
        _inputs.PlayerInteraction.Pickup.performed += Inputs_Pickup_Performed;
        _inputs.PlayerInteraction.Select.performed += Inputs_Select_Performed;
        _inputs.PlayerInteraction.Use.performed += Inputs_Use_Performed;
    }
    
    public override void OnDestroy()
    {
        _inputs.PlayerInteraction.Pickup.performed -= Inputs_Pickup_Performed;
        _inputs.PlayerInteraction.Select.performed -= Inputs_Select_Performed;
        _inputs.PlayerInteraction.Use.performed -= Inputs_Use_Performed;

        base.OnDestroy();
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _inputs.Enable();
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputs.PlayerMovement.Movement.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetLookVector()
    {
        return _inputs.PlayerMovement.Look.ReadValue<Vector2>();
    }
    
    private void Inputs_Pickup_Performed(InputAction.CallbackContext obj)
    {
        OnPickupAction.Invoke();
    }

    private void Inputs_Select_Performed(InputAction.CallbackContext obj)
    {
        int slotNumber = (int)obj.ReadValue<float>();
        OnSelectAction.Invoke(slotNumber);
    }
    
    private void Inputs_Use_Performed(InputAction.CallbackContext obj)
    {
        OnUseAction.Invoke();
    }
}