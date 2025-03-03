using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private AllLevelsData _allLevelsData;
    private ObjectPoolManager _objectPoolManager;
    private PlayerProgressManager _playerProgressManager;
    private EventManager _eventManager;
    private DiContainer _container;
    private ShelfController _shelfPrefab;

    private int _currentLevel;
    private List<ShelfController> _spawnedShelves = new List<ShelfController>();

    [Inject]
    public void Construct(AllLevelsData allLevelsData, 
        ObjectPoolManager objectPoolManager, PlayerProgressManager playerProgressManager, 
        EventManager eventManager, DiContainer container, ShelfController shelfPrefab)
    {
        _allLevelsData = allLevelsData;
        _objectPoolManager = objectPoolManager;
        _playerProgressManager = playerProgressManager;
        _eventManager = eventManager;
        _container = container;
        _shelfPrefab = shelfPrefab;
    }

    public void Initialize()
    {
        _currentLevel = _playerProgressManager.PlayerLevel;
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
            _objectPoolManager.InitializePool(item.Key, item.Value);
            Debug.Log($"Pool oluşturuluyor: {item.Key} -> {item.Value} adet");
        }
    }

    private void InitializeLevel()
    {
        foreach (var shelfData in _allLevelsData.GetLevel(_currentLevel).shelfsData)
        {
            // ShelfController prefabını Zenject üzerinden oluştur
            ShelfController shelfController = _container.InstantiatePrefabForComponent<ShelfController>(_shelfPrefab, transform);
            shelfController.SetShelf(shelfData, this);
            _spawnedShelves.Add(shelfController);
        }
    }

    private void ClearLevel()
    {
        foreach (var shelf in _spawnedShelves)
        {
            Destroy(shelf.gameObject);
        }
        _spawnedShelves.Clear();

        _objectPoolManager.ClearAllPools();
    }

    public void ControlGameStatus()
    {
        bool allShelvesEmpty = _spawnedShelves.All(shelf => shelf.AreAllFrontShelfSpacesEmpty());

        if (allShelvesEmpty)
        {
            _eventManager.Execute(GameEvents.OnLevelSuccessful);
        }
    }
}
