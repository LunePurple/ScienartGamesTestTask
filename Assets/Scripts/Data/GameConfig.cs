#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int MaxPlayers;
        public float ReturnToMenuAfterSeconds;
    }
}