#nullable enable

using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInputProvider))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private PlayerConfig Config = null!;
    [Space]
    [SerializeField] private Transform Head = null!;
    [SerializeField] private LayerMask CollisionLayerMask;

    private PlayerInputProvider _inputProvider = null!;
    
    private float _yaw;
    private float _pitch;
    private bool _grounded;
    private float _verticalVelocity;

    private Vector3 CapsulePoint1 => transform.position + Vector3.up * Config.Radius;
    private Vector3 CapsulePoint2 => transform.position + Vector3.up * (Config.Height - Config.Radius);

    private void Awake()
    {
        _inputProvider = GetComponent<PlayerInputProvider>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        HandleLookDirection();
        HandleMovement();
    }
    
    private void HandleLookDirection()
    {
        Vector2 look = _inputProvider.GetLookVector();
        
        _yaw += look.x * Config.LookRotationSpeedX * Time.deltaTime;
        _pitch -= look.y * Config.LookRotationSpeedY * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        Head.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        CheckGround();
        ApplyGravity();
        
        Vector2 input = _inputProvider.GetMovementVectorNormalized();
        const float COLLISION_OFFSET = 0.05f;

        Vector3 horizontalDirection = (transform.forward * input.y + transform.right * input.x).normalized;
        Vector3 horizontalMovement = horizontalDirection * (Config.WalkSpeed * Time.deltaTime);
        
        Vector3 verticalMovement = Vector3.up * (_verticalVelocity * Time.deltaTime);
        
        Vector3 targetMovement = horizontalMovement + verticalMovement;

        bool overlapSomething = Physics.CapsuleCast(
            CapsulePoint1,
            CapsulePoint2,
            Config.Radius,
            targetMovement.normalized,
            out RaycastHit movementHit,
            targetMovement.magnitude,
            CollisionLayerMask
        );
        if (overlapSomething)
        {
            Vector3 slide = Vector3.ProjectOnPlane(targetMovement, movementHit.normal);

            bool canMoveAlongTangent = !Physics.CapsuleCast(
                CapsulePoint1,
                CapsulePoint2,
                Config.Radius,
                slide.normalized,
                out RaycastHit slideMovementHit,
                slide.magnitude,
                CollisionLayerMask
            );

            if (canMoveAlongTangent)
            {
                transform.position += slide;
            }
            else
            {
                float adjustedDistance = Mathf.Max(slideMovementHit.distance - COLLISION_OFFSET, 0f);
                transform.position += targetMovement.normalized * adjustedDistance;
            }
        }
        else
        {
            transform.position += targetMovement;
        }
    }
    
    private void CheckGround()
    {
        _grounded = Physics.SphereCast(CapsulePoint1, Config.Radius,
            Vector3.down, out _, Config.GroundCheckDistance, CollisionLayerMask);
    }

    private void ApplyGravity()
    {
        if (_grounded)
        {
            if (_verticalVelocity < 0f)
            {
                _verticalVelocity = 0f;
            }
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
