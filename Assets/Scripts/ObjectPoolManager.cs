using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance; // Singleton

    [SerializeField] private List<PoolItemData> itemPrefabs; // ItemType -> Prefab eşleşmeleri
    private Dictionary<ItemType, Queue<GameObject>> poolDictionary; // Pool

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        poolDictionary = new Dictionary<ItemType, Queue<GameObject>>();
    }

    // 💡 Pool'u başlat: Belirtilen itemType için belirli sayıda nesne oluştur
    public void InitializePool(ItemType itemType, int amount)
    {
        if (!poolDictionary.ContainsKey(itemType))
            poolDictionary[itemType] = new Queue<GameObject>();

        PoolItemData itemData = itemPrefabs.Find(x => x.itemType == itemType);
        if (itemData == null)
        {
            Debug.LogError($"ItemType için prefab bulunamadı: {itemType}");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(itemData.prefab);
            obj.SetActive(false);
            poolDictionary[itemType].Enqueue(obj);
        }
    }

    // 🟢 Pool'dan obje çek
    public GameObject GetFromPool(ItemType itemType)
    {
        if (poolDictionary.ContainsKey(itemType) && poolDictionary[itemType].Count > 0)
        {
            GameObject obj = poolDictionary[itemType].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogWarning($"Poolda {itemType} kalmadı, yeni oluşturuluyor...");
            PoolItemData itemData = itemPrefabs.Find(x => x.itemType == itemType);
            if (itemData != null)
            {
                GameObject obj = Instantiate(itemData.prefab);
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    // 🔴 Objeyi tekrar pool'a gönder
    public void ReturnToPool(ItemType itemType, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[itemType].Enqueue(obj);
    }
}
