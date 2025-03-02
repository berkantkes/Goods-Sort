using UnityEngine;

[CreateAssetMenu(fileName = "AllLevelsData", menuName = "Match3/All Levels Data")]
public class AllLevelsData : ScriptableObject
{
    public LevelData[] levels;

    public LevelData GetLevel(int levelNumber)
    {
        foreach (var level in levels)
        {
            if (level.levelNumber == levelNumber)
                return level;
        }
        return null;
    }
}