using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private ShelfController _shelfController;

    private void Start()
    {
        InitializeObjectPool();
        InitializeLevel();
    }

    private void InitializeObjectPool()
    {
        List<ItemType> allItems = levelData.GetAllItemTypesList();

        var itemCounts = allItems.GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());
        
        foreach (var item in itemCounts)
        {
            ObjectPoolManager.Instance.InitializePool(item.Key, item.Value);
            Debug.Log($"Pool oluşturuluyor: {item.Key} -> {item.Value} adet");
        }
    }

    private void InitializeLevel()
    {
        foreach (var shelfData in levelData.shelfsData)
        {
            ShelfController shelfController = Instantiate(_shelfController, transform);
            shelfController.SetShelf(shelfData);
        }
    }
}