#nullable enable

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Visual/PlayerVisualData", fileName = "newPlayerVisual")]
    public class PlayerVisualData : ScriptableObject
    {
        public Color Color;
    }
}