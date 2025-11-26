#nullable enable

using UnityEngine;

public class PlayerInputProvider : MonoBehaviour
{
    private InputActions _inputs = null!;

    private void Awake()
    {
        _inputs = new InputActions();
        _inputs.Enable();
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