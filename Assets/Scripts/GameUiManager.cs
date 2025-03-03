using TMPro;
using UnityEngine;
using Zenject;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    
    private int _currentLevel;  
    private PlayerProgressManager _playerProgressManager;
    private LoadingManager _loadingManager;

    [Inject]
    public void Construct(PlayerProgressManager playerProgressManager)
    {
        _playerProgressManager = playerProgressManager;
        //_loadingManager = loadingManager;
    }

    private void Start()
    {
        _currentLevel = _playerProgressManager.PlayerLevel;
        _levelText.SetText("LEVEL " + _currentLevel);
    }

    public void OnMainMenuButtonPressed()
    {
        _loadingManager.LoadMainMenu();
    }
}