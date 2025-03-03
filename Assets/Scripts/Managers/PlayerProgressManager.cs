using UnityEngine;

public static class PlayerProgressManager
{
    private const string GOLD_KEY = "PlayerGold";
    private const string LEVEL_KEY = "PlayerLevel";

    public static int PlayerGold { get; private set; }
    public static int PlayerLevel { get; private set; }

    static PlayerProgressManager()
    {
        LoadProgress();
    }

    public static void AddGold(int amount)
    {
        PlayerGold += amount;
        SaveProgress();
    }

    public static void LevelUp()
    {
        PlayerLevel++;
        SaveProgress();
    }

    private static void SaveProgress()
    {
        PlayerPrefs.SetInt(GOLD_KEY, PlayerGold);
        PlayerPrefs.SetInt(LEVEL_KEY, PlayerLevel);
        PlayerPrefs.Save();
    }

    private static void LoadProgress()
    {
        PlayerGold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        PlayerLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public static void ResetProgress()
    {
        PlayerGold = 0;
        PlayerLevel = 1;
        SaveProgress();
    }
}