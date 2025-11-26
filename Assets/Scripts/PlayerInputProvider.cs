#nullable enable

using Unity.Netcode;
using UnityEngine;

public class PlayerInputProvider : NetworkBehaviour
{
    private InputActions _inputs = null!;

    private void Awake()
    {
        _inputs = new InputActions();
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
}