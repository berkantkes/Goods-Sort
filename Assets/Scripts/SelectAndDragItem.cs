using UnityEngine;

public class SelectAndDragItem : MonoBehaviour
{
    private Camera _mainCamera;
    private ItemController _selectedItem;
    private Vector3 _offset;
    private float _fixedZ; 
    private Transform _lastHoveredShelf = null;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SelectItem();
        }
        else if (Input.GetMouseButton(0) && _selectedItem != null)
        {
            MoveItem();
        }
        else if (Input.GetMouseButtonUp(0) && _selectedItem != null)
        {
            DetectShelf();
            ReleaseItem();
        }
    }

    void SelectItem()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Item"))
            {
                if (hit.collider.TryGetComponent<ItemController>(out ItemController item))
                {
                    _selectedItem = item;
                    _fixedZ = _selectedItem.transform.position.z; 
                    _offset = _selectedItem.transform.position - GetWorldPosition();
                }
            }
        }
    }

    void MoveItem()
    {
        Vector3 newPosition = GetWorldPosition() + _offset;
        _selectedItem.transform.position = new Vector3(newPosition.x, newPosition.y, _fixedZ);
    }

    void ReleaseItem()
    {
        _selectedItem = null;
    }

    Vector3 GetWorldPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, _fixedZ));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
    
    void DetectShelf()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int shelfLayerMask = LayerMask.GetMask("ShelfLayer"); 

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, shelfLayerMask))
        {
            if (hit.collider.TryGetComponent<ShelfController>(out ShelfController shelf)) 
            {
                _lastHoveredShelf = hit.collider.transform;
                Vector3 lastHitPoint = hit.point;
                if (!shelf.SetItemToShelf(lastHitPoint, _selectedItem))
                {
                    // eski yerine dön
                }
                
            }
            else
            {
                _lastHoveredShelf = null;
                //eski eyrine dön
            }
        }
    }

}