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
    private Camera _camera;
    private LevelData _levelData;

    private int _currentLevelIndex;
    private List<ShelfController> _spawnedShelves = new List<ShelfController>();

    [Inject]
    public void Construct(AllLevelsData allLevelsData, 
        ObjectPoolManager objectPoolManager, PlayerProgressManager playerProgressManager, 
        EventManager eventManager, DiContainer container, ShelfController shelfPrefab, Camera camera)
    {
        _allLevelsData = allLevelsData;
        _objectPoolManager = objectPoolManager;
        _playerProgressManager = playerProgressManager;
        _eventManager = eventManager;
        _container = container;
        _shelfPrefab = shelfPrefab;
        _camera = camera;
    }

    public void Initialize()
    {
        _currentLevelIndex = _playerProgressManager.PlayerLevel;
        _levelData = _allLevelsData.GetLevel(_currentLevelIndex);
        InitializeObjectPool();
        InitializeLevel();
        InitializeCamera();
    }

    public void ReloadCurrentLevel()
    {
        ClearLevel();
        Initialize(_currentLevelIndex);
    }

    public void Initialize(int levelNumber)
    {
        ClearLevel();
        _currentLevelIndex = levelNumber;
        _levelData = _allLevelsData.GetLevel(_currentLevelIndex);
        InitializeObjectPool();
        InitializeLevel();
        InitializeCamera();
    }

    private void InitializeObjectPool()
    {
        List<ItemType> allItems = _levelData.GetAllItemTypesList();

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
        foreach (var shelfData in _levelData.shelfsData)
        {
            // ShelfController prefabını Zenject üzerinden oluştur
            ShelfController shelfController = _container.InstantiatePrefabForComponent<ShelfController>(_shelfPrefab, transform);
            shelfController.SetShelf(shelfData, this);
            _spawnedShelves.Add(shelfController);
        }
    }
    
    private void InitializeCamera()
    {
        _camera.transform.position = new Vector3(_levelData.cameraPosition.x, _levelData.cameraPosition.y, _camera.transform.position.z);
        _camera.orthographicSize = _levelData.size;
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
