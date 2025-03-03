using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    public Button playButton;

    private LoadingManager _loadingManager;

    [Inject]
    public void Construct(LoadingManager loadingManager)
    {
        _loadingManager = loadingManager;
    }
    
    private void Start()
    {
        playButton.onClick.AddListener(() => _loadingManager.LoadGame());
    }
}