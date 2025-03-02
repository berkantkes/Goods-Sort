using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    private ItemController _attachedItem;
    private LayerController _layerController;

    public void SetLayer(LayerController layer)
    {
        _layerController = layer;
    }

    public void AttachItem(ItemController item)
    {
        if (!item)
            return;
        
        item.ReleaseItem();
        _attachedItem = item;
        _attachedItem.SetShelfSpace(this);
        _layerController.IsMatch();
    }

    public void ReleaseItem()
    {
        _attachedItem = null;
        _layerController.IsEmpty();
    }

    public bool IsAvailableShelfSpace()
    {
        return _attachedItem == null;
    }
    
    public ItemType GetAttachedItemType()
    {
        return _attachedItem != null ? _attachedItem.GetItemType() : ItemType.None;
    }    
    public ItemController GetAttachedItem()
    {
        return _attachedItem;
    }

}
