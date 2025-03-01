using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField] private ShelfSpaceController _firstShelfSpace;
    [SerializeField] private ShelfSpaceController _secondShelfSpace;
    [SerializeField] private ShelfSpaceController _thirdShelfSpace;

    [SerializeField] private List<ShelfSpaceController> _shelfSpaces;
    public void SetLayer(LayerData layerData)
    {
        
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
    
    public ShelfSpaceController GetClosestShelf(Vector3 targetPosition)
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
