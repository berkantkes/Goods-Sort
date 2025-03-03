using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private AllLevelsData _allLevelsData;
    [SerializeField] private ShelfController _shelfController;

    private int _currentLevel;
    private List<ShelfController> _spawnedShelves = new List<ShelfController>();

    public void Initialize()
    {
        _currentLevel = PlayerProgressManager.PlayerLevel;
        InitializeObjectPool();
        InitializeLevel();
    }
    
    public void ReloadCurrentLevel()
    {
        ClearLevel();
        Initialize(_currentLevel);
    }

    public void Initialize(int levelNumber)
    {
        ClearLevel();
        _currentLevel = levelNumber;
        InitializeObjectPool();
        InitializeLevel();
    }

    private void InitializeObjectPool()
    {
        List<ItemType> allItems = _allLevelsData.GetLevel(_currentLevel).GetAllItemTypesList();

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
        foreach (var shelfData in _allLevelsData.GetLevel(_currentLevel).shelfsData)
        {
            ShelfController shelfController = Instantiate(_shelfController, transform);
            shelfController.SetShelf(shelfData, this);
            _spawnedShelves.Add(shelfController); // **Oluşturulan shelf'leri listeye ekle**
        }
    }

    private void ClearLevel()
    {
        foreach (var shelf in _spawnedShelves)
        {
            Destroy(shelf.gameObject);
        }
        _spawnedShelves.Clear();

        ObjectPoolManager.Instance.ClearAllPools();
    }

    public void ControlGameStatus()
    {
        bool allShelvesEmpty = _spawnedShelves.All(shelf => shelf.AreAllFrontShelfSpacesEmpty());

        if (allShelvesEmpty)
        {
            EventManager.Execute(GameEvents.OnLevelSuccessful);
        }
    }

}