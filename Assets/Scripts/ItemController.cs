using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private ShelfSpaceController _attachedShelfSpace;

    public void SetShelfSpace(ShelfSpaceController shelfSpaceController)
    {
        _attachedShelfSpace = shelfSpaceController;
    }
    
}
