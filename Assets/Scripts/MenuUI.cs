#nullable enable

using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button CreateGameButton = null!;
    [SerializeField] private Button JoinGameButton = null!;
    [SerializeField] private Button ExitGameButton = null!;

    private void Start()
    {
        CreateGameButton.onClick.AddListener(OnCreateGameClicked);
        JoinGameButton.onClick.AddListener(OnJoinGameClicked);
        ExitGameButton.onClick.AddListener(OnExitGameClicked);
    }

    private void OnDestroy()
    {
        CreateGameButton.onClick.RemoveListener(OnCreateGameClicked);
        JoinGameButton.onClick.RemoveListener(OnJoinGameClicked);
        ExitGameButton.onClick.RemoveListener(OnExitGameClicked);
    }

    private void OnCreateGameClicked()
    {
        Loader.Load(Loader.Scene.MainScene, () =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }

    private void OnJoinGameClicked()
    {
        Loader.Load(Loader.Scene.MainScene, () =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
    
    private void OnExitGameClicked()
    {
        Application.Quit();
    }
}