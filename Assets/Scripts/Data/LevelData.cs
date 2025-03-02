using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Match3/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int fieldOfView = 40;
    public int columns = 1; 
    public int rows = 1;   
    public float spacing = 1.5f; 
    public List<ShelfData> shelfsData = new List<ShelfData>();

    private void OnValidate()
    {
        AdjustShelfs();
        
        if (Application.isPlaying)
        {
            FindObjectOfType<LevelManager>()?.ReloadCurrentLevel();
        }
    }

    public void AdjustShelfs()
    {
        int totalShelfs = columns * rows;

        if (shelfsData.Count > totalShelfs)
        {
            shelfsData.RemoveRange(totalShelfs, shelfsData.Count - totalShelfs);
        }

        for (int i = 0; i < shelfsData.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;
            shelfsData[i].rowIndex = row;
            shelfsData[i].columnIndex = col;
            //shelfsData[i].position = new Vector2(col * spacing, row * spacing);
        }
    }

    public List<ItemType> GetAllItemTypesList()
    {
        List<ItemType> allItems = new List<ItemType>();

        foreach (var shelf in shelfsData)
        {
            if (shelf.layers != null)
            {
                foreach (var layer in shelf.layers)
                {
                    allItems.AddRange(layer.GetItemTypesList());
                }
            }
        }

        return allItems.Where(item => item != ItemType.None).ToList();
    }
}