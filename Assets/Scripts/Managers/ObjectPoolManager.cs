using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPoolManager : MonoBehaviour
{
    private PoolData _poolData;
    private DiContainer _container;
    private Dictionary<ItemType, Queue<ItemController>> _poolDictionary = new Dictionary<ItemType, Queue<ItemController>>();


    [Inject]
    public void Construct(PoolData poolData, DiContainer container)
    {
        _poolData = poolData;
        _container = container;
    }

    public void Initialize()
    {
        // Eğer başlangıçta bir şeyler yüklemek istiyorsan buraya ekleyebilirsin
    }

    public void InitializePool(ItemType itemType, int amount)
    {
        if (_poolDictionary == null)
        {
            _poolDictionary = new Dictionary<ItemType, Queue<ItemController>>();
        }
        if (!_poolDictionary.ContainsKey(itemType))
            _poolDictionary[itemType] = new Queue<ItemController>();

        ItemController itemPrefab = _poolData.GetItemController(itemType);
        if (itemPrefab == null)
        {
            Debug.LogError($"ItemType için prefab bulunamadı: {itemType}");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            ItemController obj = _container.InstantiatePrefabForComponent<ItemController>(itemPrefab);
            obj.gameObject.SetActive(false);
            _poolDictionary[itemType].Enqueue(obj);
        }
    }

    public ItemController GetFromPool(ItemType itemType)
    {
        if (_poolDictionary.ContainsKey(itemType) && _poolDictionary[itemType].Count > 0)
        {
            ItemController obj = _poolDictionary[itemType].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"Poolda {itemType} kalmadı, yeni oluşturuluyor...");
        ItemController itemPrefab = _poolData.GetItemController(itemType);
        if (itemPrefab != null)
        {
            ItemController obj = _container.InstantiatePrefabForComponent<ItemController>(itemPrefab);
            obj.gameObject.SetActive(true);
            return obj;
        }

        //Debug.LogError($"Yeni {itemType} oluşturulamadı! PoolData'da prefab bulunmuyor.");
        return null;
    }

    public void ReturnToPool(ItemType itemType, ItemController obj)
    {
        if (!_poolDictionary.ContainsKey(itemType))
            _poolDictionary[itemType] = new Queue<ItemController>();

        obj.gameObject.SetActive(false);
        _poolDictionary[itemType].Enqueue(obj);
    }

    public void ClearAllPools()
    {
        foreach (var pool in _poolDictionary)
        {
            while (pool.Value.Count > 0)
            {
                ItemController obj = pool.Value.Dequeue();
                obj.gameObject.SetActive(false);
            }
        }

        _poolDictionary.Clear();
        Debug.Log("Tüm object pool'lar temizlendi!");
    }
}
