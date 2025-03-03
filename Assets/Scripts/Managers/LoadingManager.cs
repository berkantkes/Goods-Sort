using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Zenject;

public class LoadingManager
{
    //private readonly GameObject _loadingScreen;

    public async void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            //_loadingScreen.SetActive(true); 
            await LoadSceneAsync("MainScene");
            SceneManager.UnloadSceneAsync("StartScene");
        }
    }
    
    public async Task LoadSceneAsync(string sceneName)
    {
        //_loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        //_loadingScreen.SetActive(false);
    }

    public async void LoadGame()
    {
        await LoadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("MainScene");
    }

    public async void LoadMainMenu()
    {
        await LoadSceneAsync("MainScene");
        SceneManager.UnloadSceneAsync("GameScene");
    }
}