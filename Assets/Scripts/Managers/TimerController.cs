using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float levelDuration = 120f; 
    private float timer;
    private bool isTimerRunning = false;

    public TextMeshProUGUI timerText;

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
                EventManager.Execute(GameEvents.OnLevelFail);
                Debug.Log("Game Fail");
            }

            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            timer = levelDuration;
            isTimerRunning = true;
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; 
    }
}