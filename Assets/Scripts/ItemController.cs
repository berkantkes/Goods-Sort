using DG.Tweening;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    private ShelfSpaceController _attachedShelfSpace;

    public ItemType GetItemType()
    {
        return _itemType;
    }

    public void SetShelfSpace(ShelfSpaceController shelfSpaceController)
    {
        _attachedShelfSpace = shelfSpaceController;
        transform.SetParent(_attachedShelfSpace.transform);
        transform.localPosition = Vector3.zero;
    }

    public void GoPosition()
    {
        transform.DOMove(_attachedShelfSpace.transform.position, 1.5f);
    }
}