using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LayerController : MonoBehaviour
{
    [SerializeField] private List<ShelfSpaceController> _frontShelfSpaces;
    [SerializeField] private List<ShelfSpaceController> _backShelfSpaces;

    private ShelfController _shelfController;
    private List<LayerData> _layerData;
    private int _currentLayerCount = 0;

    private void Awake()
    {
        foreach (var shelfSpace in _frontShelfSpaces)
        {
            shelfSpace.SetLayer(this);
        }
        foreach (var shelfSpace in _backShelfSpaces)
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
        _layerData = layerData;
        SetLayers();
    }

    private void SetLayers()
    {
        if (_currentLayerCount <= _layerData.Count-1)
        {
            SetFrontLayer(_layerData[_currentLayerCount]);
        }

        if (_currentLayerCount+1 <= _layerData.Count-1)
        {
            SetBackLayer(_layerData[_currentLayerCount+1]);
        }
    }
    
    public void SetFrontLayer(LayerData frontLayerData)
    {
        _frontShelfSpaces[0].AttachItem(ObjectPoolManager.Instance.GetFromPool(frontLayerData.FirstItemType));
        _frontShelfSpaces[1].AttachItem(ObjectPoolManager.Instance.GetFromPool(frontLayerData.SecondItemType));
        _frontShelfSpaces[2].AttachItem(ObjectPoolManager.Instance.GetFromPool(frontLayerData.ThirdItemType));
    }
    public void SetBackLayer(LayerData backLayerData)
    {
        _backShelfSpaces[0].AttachItem(ObjectPoolManager.Instance.GetFromPool(backLayerData.FirstItemType));
        _backShelfSpaces[1].AttachItem(ObjectPoolManager.Instance.GetFromPool(backLayerData.SecondItemType));
        _backShelfSpaces[2].AttachItem(ObjectPoolManager.Instance.GetFromPool(backLayerData.ThirdItemType));
    }

    private void ChangeLayer()
    {
        _currentLayerCount++;
        
        _frontShelfSpaces[0].AttachItem(_backShelfSpaces[0].GetAttachedItem());
        _frontShelfSpaces[1].AttachItem(_backShelfSpaces[1].GetAttachedItem());
        _frontShelfSpaces[2].AttachItem(_backShelfSpaces[2].GetAttachedItem());
        
        if (_currentLayerCount+1 <= _layerData.Count-1)
        {
            SetBackLayer(_layerData[_currentLayerCount+1]);
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

        if (matchedCount == 3) 
        {
            foreach (var shelfSpace in _frontShelfSpaces)
            {
                ItemController item = shelfSpace.GetAttachedItem();
                item.ReleaseItem();
                ObjectPoolManager.Instance.ReturnToPool(item.GetItemType(), item);
            }
            EventManager<Vector3>.Execute(GameEvents.OnMatch, transform.position);
            ChangeLayer();
            _shelfController.ControlGameStatus();
        }
    }
    
    public void IsEmpty()
    {
        int emptyCount = 0;

        foreach (var shelfSpace in _frontShelfSpaces)
        {
            if (shelfSpace.IsAvailableShelfSpace()) 
            {
                emptyCount++;
            }
        }

        if (emptyCount == _frontShelfSpaces.Count)
        {
            ChangeLayer();
        }
    }

    public bool IsAvailable(Vector3 point, ItemController item)
    {
        ShelfSpaceController closestShelfSpace = GetClosestShelf(point);

        if (closestShelfSpace.IsAvailableShelfSpace())
        {
            closestShelfSpace.AttachItem(item);
            return true;
        }
        else
        {
            foreach (var shelfSpace in _frontShelfSpaces)
            {
                if (shelfSpace.IsAvailableShelfSpace())
                {
                    shelfSpace.AttachItem(item);
                    return true;
                }
            }
        }

        return false;
    }
    
    private ShelfSpaceController GetClosestShelf(Vector3 targetPosition)
    {
        if (_frontShelfSpaces == null || _frontShelfSpaces.Count == 0)
        {
            Debug.LogWarning("Shelf listesi boş!");
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
