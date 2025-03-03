using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    
    private int _currentLevel;
    public void Start()
    {
        _currentLevel = PlayerProgressManager.PlayerLevel;
        _levelText.SetText("LEVEL " + _currentLevel);
    }
    public void OnMainMenuButtonPressed()
    {
        LoadingManager.Instance.LoadMainMenu();
    }

}
