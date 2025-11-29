#nullable enable

using UnityEngine;

namespace Utility
{
    public class NetworkManagerSingleton : MonoBehaviour
    {
        private static NetworkManagerSingleton _instance = null!;
    
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}