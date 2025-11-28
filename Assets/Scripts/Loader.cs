#nullable enable

using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        Menu,
        MainScene
    }

    private static Action? _onLoaded;

    public static void Load(Scene scene, Action? onLoaded = null)
    {
        _onLoaded = onLoaded;
        
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneLoaded += SceneLoaded;
        
        SceneManager.LoadScene(scene.ToString());
    }

    private static void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        
        _onLoaded?.Invoke();
        _onLoaded = null;
    }
}