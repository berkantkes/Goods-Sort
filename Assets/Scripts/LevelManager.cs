using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    private void Start()
    {
        InitializeObjectPool();
    }

    private void InitializeObjectPool()
    {
        // 1️⃣ LevelData'dan tüm itemleri al
        List<ItemType> allItems = levelData.GetAllItemTypesList();

        // 2️⃣ Aynı türden kaç tane olduğunu hesapla
        var itemCounts = allItems.GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());

        // 3️⃣ ObjectPoolManager'a bildir
        foreach (var item in itemCounts)
        {
            ObjectPoolManager.Instance.InitializePool(item.Key, item.Value);
            Debug.Log($"Pool oluşturuluyor: {item.Key} -> {item.Value} adet");
        }
    }
}