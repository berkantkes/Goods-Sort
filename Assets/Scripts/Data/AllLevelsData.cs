using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AllLevelsData", menuName = "Match3/All Levels Data")]
public class AllLevelsData : ScriptableObject
{
    public LevelData[] levels;

    public LevelData GetLevel(int levelNumber)
    {
        LevelData foundLevel = levels.FirstOrDefault(level => level.levelNumber == levelNumber);

        if (foundLevel != null)
            return foundLevel;

        if (levels.Length > 1)
        {
            return levels[Random.Range(1, levels.Length)];
        }

        return null; 
    }
}