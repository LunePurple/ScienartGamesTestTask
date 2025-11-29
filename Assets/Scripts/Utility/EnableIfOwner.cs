#nullable enable

using Unity.Netcode;
using UnityEngine;

namespace Utility
{
    public class EnableIfOwner : NetworkBehaviour
    {
        [SerializeField] private GameObject Object = null!;
    
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Object.SetActive(true);
            }
        }
    }
}