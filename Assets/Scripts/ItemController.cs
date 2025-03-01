using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private ShelfSpaceController _attachedShelfSpace;

    public void SetShelfSpace(ShelfSpaceController shelfSpaceController)
    {
        _attachedShelfSpace = shelfSpaceController;
    }

    public void GoPosition()
    {
        transform.DOMove(_attachedShelfSpace.transform.position, 1.5f);
    }
    
}
