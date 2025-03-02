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
            loadingScreen.SetActive(true); // Loading UI aç
            await LoadSceneAsync("MainScene");
            SceneManager.UnloadSceneAsync("StartScene"); // StartScene'i kaldır
        }
    }

    public async void LoadGame()
    {
        loadingScreen.SetActive(true); // Loading UI aç
        await LoadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("MainScene"); // MainScene'i kaldır
    }

    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = true; // Direkt açılacak

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        // Yeni sahneyi aktif yap
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        loadingScreen.SetActive(false); // UI'yi kapat
    }
}