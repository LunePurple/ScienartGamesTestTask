#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Base")]
        public float Height;
        public float Radius;

        [Header("Movement")]
        public float WalkSpeed;

        [Header("Ground Check")]
        public float GroundCheckDistance;

        [Header("Look")]
        public float LookRotationSpeedX;
        public float LookRotationSpeedY;

        [Header("Interaction")]
        public float InteractionDistance;
    }
}