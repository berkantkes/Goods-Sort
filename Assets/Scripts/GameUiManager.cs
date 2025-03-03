using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _successPanel;
    [SerializeField] private GameObject _failPanel;
    
    private int _currentLevel;  
    private PlayerProgressManager _playerProgressManager;
    private LoadingManager _loadingManager;
    private EventManager _eventManager;

    [Inject]
    public void Construct(PlayerProgressManager playerProgressManager, LoadingManager loadingManager, EventManager eventManager)
    {
        _playerProgressManager = playerProgressManager;
        _loadingManager = loadingManager;
        _eventManager = eventManager;
    }

    private void OnEnable()
    {
        _eventManager.Subscribe(GameEvents.OnLevelSuccessful, OpenSuccessPanel);
        _eventManager.Subscribe(GameEvents.OnLevelFail, OpenFailPanel);
    }

    private void OnDisable()
    {
        _eventManager.Unsubscribe(GameEvents.OnLevelSuccessful, OpenSuccessPanel);
        _eventManager.Unsubscribe(GameEvents.OnLevelFail, OpenFailPanel);
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

    private void OpenSuccessPanel()
    {
        _successPanel.SetActive(true);
    }
    private void OpenFailPanel()
    {
        _failPanel.SetActive(true);
    }
}