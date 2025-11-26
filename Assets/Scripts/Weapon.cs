#nullable enable

using Data;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData Data = null!;

    public WeaponData WeaponData => Data;
}