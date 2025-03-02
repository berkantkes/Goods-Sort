using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField] private ShelfSpaceController _firstShelfSpace;
    [SerializeField] private ShelfSpaceController _secondShelfSpace;
    [SerializeField] private ShelfSpaceController _thirdShelfSpace;

    [SerializeField] private List<ShelfSpaceController> _shelfSpaces;

    private void Awake()
    {
        foreach (var shelfSpace in _shelfSpaces)
        {
            shelfSpace.SetLayer(this);
        }
    }

    public void SetLayer(LayerData layerData)
    {
        _shelfSpaces[0].AttachItem(ObjectPoolManager.Instance.GetFromPool(layerData.FirstItemType));
        _shelfSpaces[1].AttachItem(ObjectPoolManager.Instance.GetFromPool(layerData.SecondItemType));
        _shelfSpaces[2].AttachItem(ObjectPoolManager.Instance.GetFromPool(layerData.ThirdItemType));
    }

    public void IsMatch()
    {
        ItemType matchedItemType = ItemType.None;
        int matchedCount = 0;

        foreach (var shelfSpace in _shelfSpaces)
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

            matchedCount++; // Eşleşen öğe sayısını artır
        }

        if (matchedCount == 3) 
        {
            Debug.Log("Matced True");
        }
    }
    
    public void IsEmpty()
    {
        int emptyCount = 0;

        foreach (var shelfSpace in _shelfSpaces)
        {
            if (shelfSpace.IsAvailableShelfSpace()) 
            {
                emptyCount++;
            }
        }

        if (emptyCount == _shelfSpaces.Count) 
        {
            Debug.Log("empty");
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
            foreach (var shelfSpace in _shelfSpaces)
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
        if (_shelfSpaces == null || _shelfSpaces.Count == 0)
        {
            Debug.LogWarning("Shelf listesi boş!");
            return null;
        }

        ShelfSpaceController closestShelf = null;
        float shortestDistance = Mathf.Infinity;

        foreach (ShelfSpaceController shelf in _shelfSpaces)
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
