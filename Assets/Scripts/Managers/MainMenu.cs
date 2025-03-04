using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _playButtonText;

    private LoadingManager _loadingManager;
    private PlayerProgressManager _playerProgressManager;

    [Inject]
    public void Construct(LoadingManager loadingManager, PlayerProgressManager playerProgressManager)
    {
        _loadingManager = loadingManager;
        _playerProgressManager = playerProgressManager;
    }
    
    private void OnEnable()
    {
        _playButtonText.SetText("LEVEL " + _playerProgressManager.PlayerLevel);
        _playButton.onClick.AddListener(() => _loadingManager.LoadGame());
    }    
    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(() => _loadingManager.LoadGame());
    }
}