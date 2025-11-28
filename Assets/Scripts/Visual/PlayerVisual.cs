#nullable enable

using UnityEngine;

namespace Visual
{
    public class PlayerVisual : MonoBehaviour
    {
        private MeshRenderer _renderer = null!;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void SetPlayerColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}