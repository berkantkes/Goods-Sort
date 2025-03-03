using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    public GameObject loadingScreen; // UI için referans

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            loadingScreen.SetActive(true); 
            await LoadSceneAsync("MainScene");
            SceneManager.UnloadSceneAsync("StartScene");
        }
    }

    public async void LoadGame()
    {
        loadingScreen.SetActive(true); 
        await LoadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("MainScene");
    }

    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = true; 

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        loadingScreen.SetActive(false);
    }
    
    public async void LoadMainMenu()
    {
        loadingScreen.SetActive(true); 
        await LoadSceneAsync("MainScene");
        SceneManager.UnloadSceneAsync("GameScene"); 
    }

}