#nullable enable

using Unity.Netcode;
using UnityEngine;

namespace Visual
{
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField] private GameObject ResultPanel = null!;
        [SerializeField] private GameObject WonPanel = null!;
        [SerializeField] private GameObject LostPanel = null!;
        
        private void Start()
        {
            ResultPanel.SetActive(false);
            WonPanel.SetActive(false);
            LostPanel.SetActive(false);
            
            GameStateHandler.OnGameStateChanged += GameStateHandler_OnGameStateChanged;
            GameStateHandler.OnPlayerLose += GameStateHandler_OnPlayerLose;
        }

        private void OnDestroy()
        {
            GameStateHandler.OnGameStateChanged -= GameStateHandler_OnGameStateChanged;
            GameStateHandler.OnPlayerLose -= GameStateHandler_OnPlayerLose;
        }
        
        private void GameStateHandler_OnGameStateChanged(GameStateHandler.GameState state)
        {
            ResultPanel.SetActive(state is GameStateHandler.GameState.Result);
        }
        
        private void GameStateHandler_OnPlayerLose(ulong clientId)
        {
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            
            WonPanel.SetActive(clientId != localClientId);
            LostPanel.SetActive(clientId == localClientId);
        }
    }
}