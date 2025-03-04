using DG.Tweening;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    
    private ShelfSpaceController _attachedShelfSpace;
    private ObjectPoolManager _objectPoolManager;
    
    
    private ShelfSpaceController _oldAttachedShelfSpace;

    public void Initialize(ObjectPoolManager objectPoolManager)
    {
        _objectPoolManager = objectPoolManager;
    }
    
    public ItemType GetItemType()
    {
        return _itemType;
    }

    public void SetShelfSpace(ShelfSpaceController shelfSpaceController)
    {
        _oldAttachedShelfSpace = _attachedShelfSpace;
        _attachedShelfSpace = shelfSpaceController;
        transform.SetParent(_attachedShelfSpace.transform);
        transform.localPosition = Vector3.zero;
    }

    public void GoPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public void ReleaseItem()
    {
        if (_attachedShelfSpace)
            _attachedShelfSpace.ReleaseAttachedItem();
    }

    public void OldShelfCheckIsEmpty()
    {
        if (_oldAttachedShelfSpace)
            _oldAttachedShelfSpace.CheckIsEmpty();
    }
    
    public Tween MatchItem()
    {
        Vector3 defaultScale = transform.localScale;
        return transform.DOScale(defaultScale*1.1f, 0.1f) 
            .OnComplete(() =>
            {
                transform.DOScale(defaultScale*.9f, 0.09f) 
                    .OnComplete(() =>
                    {
                        if (_attachedShelfSpace)
                            _attachedShelfSpace.ReleaseAttachedItem();
                        _objectPoolManager.ReturnToPool(_itemType, this);

                    });
            });
    }
}