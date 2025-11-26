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
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputs = new InputActions();
        
        _inputs.PlayerInteraction.Pickup.performed += Inputs_Pickup_Performed;
        _inputs.PlayerInteraction.Select.performed += Inputs_Select_Performed;
        _inputs.PlayerInteraction.Use.performed += Inputs_Use_Performed;
            
        _inputs.Enable();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputs.PlayerInteraction.Pickup.performed -= Inputs_Pickup_Performed;
        _inputs.PlayerInteraction.Select.performed -= Inputs_Select_Performed;
        _inputs.PlayerInteraction.Use.performed -= Inputs_Use_Performed;
        
        _inputs.Dispose();
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