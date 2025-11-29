#nullable enable

using UnityEngine;

namespace Data.Weapons.Projectiles
{
    [CreateAssetMenu(menuName = "Data/Weapons/Projectiles/ProjectileData", fileName = "newProjectile")]
    public class ProjectileData : ScriptableObject
    {
        public GameObject Prefab = null!;
        public float MovementSpeed;
        public int Damage;
    }
}