using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Match3/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int fieldOfView = 40;
    public int columns = 1; // Kaç sütun
    public int rows = 1;    // Kaç satır
    public float spacing = 1.5f; // Çatıların birbirinden uzaklığı
    public List<ShelfData> shelfsData = new List<ShelfData>(); // Roof'lar List olarak tutuluyor

    private void OnValidate()
    {
        AdjustShelfs();
    }
    

    public void AdjustShelfs()
    {
        int totalShelfs = columns * rows;

        // Listeyi yeniden boyutlandırma
        if (shelfsData.Count > totalShelfs)
        {
            shelfsData.RemoveRange(totalShelfs, shelfsData.Count - totalShelfs); // Fazla roofları sil
        }
        else if (shelfsData.Count < totalShelfs)
        {
            int countToAdd = totalShelfs - shelfsData.Count;
            for (int i = 0; i < countToAdd; i++)
            {
                shelfsData.Add(new ShelfData()); // Eksik roofları ekle
            }
        }

        // Pozisyonları Güncelle
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                shelfsData[index].rowIndex = row;
                shelfsData[index].columnIndex = col;
                shelfsData[index].position = new Vector2(col * spacing, row * spacing);
            }
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

        // `None` olanları filtrele ve döndür
        return allItems.Where(item => item != ItemType.None).ToList();
    }
}