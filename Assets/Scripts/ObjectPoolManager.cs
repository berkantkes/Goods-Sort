using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private PoolData poolData;
    private Dictionary<ItemType, Queue<ItemController>> poolDictionary;

    public void Initialize()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        poolDictionary = new Dictionary<ItemType, Queue<ItemController>>();
    }

    public void InitializePool(ItemType itemType, int amount)
    {
        if (!poolDictionary.ContainsKey(itemType))
            poolDictionary[itemType] = new Queue<ItemController>();

        ItemController itemPrefab = poolData.GetItemController(itemType);
        if (itemPrefab == null)
        {
            Debug.LogError($"ItemType için prefab bulunamadı: {itemType}");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            ItemController obj = Instantiate(itemPrefab, transform); // Direkt ItemController'ı kullanıyoruz
            obj.gameObject.SetActive(false);
            poolDictionary[itemType].Enqueue(obj);
        }
    }

    public ItemController GetFromPool(ItemType itemType)
    {
        if (poolDictionary.ContainsKey(itemType) && poolDictionary[itemType].Count > 0)
        {
            ItemController obj = poolDictionary[itemType].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogWarning($"Poolda {itemType} kalmadı, yeni oluşturuluyor...");
            ItemController itemPrefab = poolData.GetItemController(itemType);
            if (itemPrefab != null)
            {
                ItemController obj = Instantiate(itemPrefab);
                obj.gameObject.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    public void ReturnToPool(ItemType itemType, ItemController obj)
    {
        obj.gameObject.SetActive(false);
        poolDictionary[itemType].Enqueue(obj);
    }
}