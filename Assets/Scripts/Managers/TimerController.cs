using System;
using UnityEngine;
using TMPro;
using Zenject;

public class TimerController : MonoBehaviour
{
    private EventManager _eventManager;
    public float levelDuration = 10f; 
    private float timer;
    private bool isTimerRunning = false;

    public TextMeshProUGUI timerText;
    
    [Inject]
    public void Construct(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void OnEnable()
    {
        _eventManager.Subscribe(GameEvents.OnLevelSuccessful, StopTimer);
    }
    private void OnDisable()
    {
        _eventManager.Unsubscribe(GameEvents.OnLevelSuccessful, StopTimer);
    }

    private void Update()
    { 
        if (Input.GetMouseButtonDown(0)) 
        {
            StartTimer();
        }
        
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                isTimerRunning = false;
                timer = 0f;
                _eventManager.Execute(GameEvents.OnLevelFail);
                Debug.Log("Game Fail");
            }

            UpdateTimerUI();
        }
    }

    private void StartTimer()
    {
        if (!isTimerRunning)
        {
            timer = levelDuration;
            isTimerRunning = true;
        }
    }
    private void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; 
    }
}