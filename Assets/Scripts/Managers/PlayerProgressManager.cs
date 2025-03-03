using UnityEngine;
using Zenject;

public class PlayerProgressManager
{
    private const string GOLD_KEY = "PlayerGold";
    private const string LEVEL_KEY = "PlayerLevel";

    public int PlayerGold { get; private set; }
    public int PlayerLevel { get; private set; }

    public PlayerProgressManager()
    {
        LoadProgress();
    }

    public void AddGold(int amount)
    {
        PlayerGold += amount;
        SaveProgress();
    }

    public void LevelUp()
    {
        PlayerLevel++;
        SaveProgress();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(GOLD_KEY, PlayerGold);
        PlayerPrefs.SetInt(LEVEL_KEY, PlayerLevel);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        PlayerGold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        PlayerLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public void ResetProgress()
    {
        PlayerGold = 0;
        PlayerLevel = 1;
        SaveProgress();
    }
}