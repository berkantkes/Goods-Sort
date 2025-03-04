using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LayerController : MonoBehaviour
{
    [SerializeField] private List<ShelfSpaceController> _frontShelfSpaces;
    [SerializeField] private List<ShelfSpaceController> _backShelfSpaces;

    private ShelfController _shelfController;
    private List<LayerData> _layerData;
    private int _currentLayerCount = 0;

    private LevelManager _levelManager;
    private EventManager _eventManager;
    private ObjectPoolManager _objectPoolManager;

    private const int firstShelfIndex = 0;
    private const int secondShelfIndex = 1;
    private const int thirdShelfIndex = 2;
    private const int requiredMatchCount = 3;

    private const string ItemTag = "Item";
    private const string NoneTag = "Untagged";

    [Inject]
    public void Construct(LevelManager levelManager, EventManager eventManager, ObjectPoolManager objectPoolManager)
    {
        _levelManager = levelManager;
        _eventManager = eventManager;
        _objectPoolManager = objectPoolManager;
    }

    private void Awake()
    {
        InitializeShelfSpaces(_frontShelfSpaces);
        InitializeShelfSpaces(_backShelfSpaces);
    }

    private void InitializeShelfSpaces(List<ShelfSpaceController> shelfSpaces)
    {
        foreach (var shelfSpace in shelfSpaces)
        {
            shelfSpace.SetLayer(this);
        }
    }

    public void SetShelfController(ShelfController shelfController)
    {
        _shelfController = shelfController;
    }

    public void SetLayerData(List<LayerData> layerData)
    {
        if (layerData == null || layerData.Count == 0)
        {
            Debug.LogWarning("LayerData list is null or empty!");
            return;
        }

        _layerData = layerData;
        SetLayers();
    }

    private void SetLayers()
    {
        if (_layerData == null || _layerData.Count == 0)
            return;

        int lastLayerIndex = _layerData.Count - 1;

        if (_currentLayerCount <= lastLayerIndex)
        {
            SetLayerItems(_frontShelfSpaces, _layerData[_currentLayerCount]);
            UpdateTags(_frontShelfSpaces, ItemTag); // Front shelf'deki item'ların tag'ini güncelle
        }

        int nextLayerIndex = _currentLayerCount + 1;
        if (nextLayerIndex <= lastLayerIndex)
        {
            SetLayerItems(_backShelfSpaces, _layerData[nextLayerIndex]);
            UpdateTags(_backShelfSpaces, NoneTag); // Back shelf'deki item'ların tag'ini güncelle
        }
    }

    private void SetLayerItems(List<ShelfSpaceController> shelfSpaces, LayerData layerData)
    {
        if (shelfSpaces == null || shelfSpaces.Count != requiredMatchCount || layerData == null)
        {
            Debug.LogWarning("Invalid shelf spaces or layer data!");
            return;
        }

        shelfSpaces[firstShelfIndex].AttachItem(_objectPoolManager.GetFromPool(layerData.FirstItemType));
        shelfSpaces[secondShelfIndex].AttachItem(_objectPoolManager.GetFromPool(layerData.SecondItemType));
        shelfSpaces[thirdShelfIndex].AttachItem(_objectPoolManager.GetFromPool(layerData.ThirdItemType));
    }

    private void UpdateTags(List<ShelfSpaceController> shelfSpaces, string tag)
    {
        foreach (var shelfSpace in shelfSpaces)
        {
            ItemController item = shelfSpace.GetAttachedItem();
            if (item != null)
            {
                item.gameObject.tag = tag;
            }
        }
    }

    private void ChangeLayer()
    {
        _currentLayerCount++;

        for (int i = 0; i < _frontShelfSpaces.Count; i++)
        {
            _frontShelfSpaces[i].AttachItem(_backShelfSpaces[i].GetAttachedItem());
        }

        UpdateTags(_frontShelfSpaces, ItemTag); // Front shelf'deki item'ların tag'ini güncelle

        int nextLayerIndex = _currentLayerCount + 1;
        int lastLayerIndex = _layerData.Count - 1;

        if (nextLayerIndex <= lastLayerIndex)
        {
            SetLayerItems(_backShelfSpaces, _layerData[nextLayerIndex]);
            UpdateTags(_backShelfSpaces, NoneTag); // Back shelf'deki item'ların tag'ini güncelle
        }
    }

    public bool AreAllFrontShelfSpacesEmpty()
    {
        return _frontShelfSpaces.All(space => space.IsAvailableShelfSpace());
    }

    public void IsMatch()
    {
        ItemType matchedItemType = ItemType.None;
        int matchedCount = 0;

        foreach (var shelfSpace in _frontShelfSpaces)
        {
            ItemType currentItemType = shelfSpace.GetAttachedItemType();

            if (currentItemType == ItemType.None)
                continue;

            if (matchedItemType == ItemType.None)
            {
                matchedItemType = currentItemType;
            }
            else if (currentItemType != matchedItemType)
            {
                return;
            }

            matchedCount++;
        }

        if (matchedCount == requiredMatchCount)
        {
            ReleaseItemsAndNotifyMatch();
            ChangeLayer();
            _shelfController.ControlGameStatus();
        }
    }

    private async void ReleaseItemsAndNotifyMatch()
    {
        foreach (var shelfSpace in _frontShelfSpaces)
        {
            ItemController item = shelfSpace.GetAttachedItem();
            item.MatchItem();
            //_objectPoolManager.ReturnToPool(item.GetItemType(), item);
        }
        
        await UniTask.Delay(200);

        _eventManager.Execute<Vector3>(GameEvents.OnMatch, transform.position);
    }

    public void IsEmpty()
    {
        if (_frontShelfSpaces.All(space => space.IsAvailableShelfSpace()))
        {
            ChangeLayer();
        }
    }

    public bool IsAvailable(Vector3 point, ItemController item)
    {
        ShelfSpaceController closestShelfSpace = GetClosestShelf(point);

        if (closestShelfSpace != null && closestShelfSpace.IsAvailableShelfSpace())
        {
            closestShelfSpace.AttachItem(item);
            return true;
        }

        foreach (var shelfSpace in _frontShelfSpaces)
        {
            if (shelfSpace.IsAvailableShelfSpace())
            {
                shelfSpace.AttachItem(item);
                return true;
            }
        }

        return false;
    }

    private ShelfSpaceController GetClosestShelf(Vector3 targetPosition)
    {
        if (_frontShelfSpaces == null || _frontShelfSpaces.Count == 0)
        {
            Debug.LogWarning("Shelf list is null or empty!");
            return null;
        }

        ShelfSpaceController closestShelf = null;
        float shortestDistance = Mathf.Infinity;

        foreach (ShelfSpaceController shelf in _frontShelfSpaces)
        {
            float distance = Vector3.Distance(targetPosition, shelf.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestShelf = shelf;
            }
        }

        return closestShelf;
    }
}