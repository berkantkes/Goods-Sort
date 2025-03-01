using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    private ItemController _attachedItem;

    public void AttachItem(ItemController item)
    {
        if (!item)
            return;
        
        _attachedItem = item;
        _attachedItem.SetShelfSpace(this);
    }

    public void ReleaseItem()
    {
        _attachedItem = null;
    }

    public bool IsAvailableShelfSpace()
    {
        return _attachedItem == null;
    }
}
