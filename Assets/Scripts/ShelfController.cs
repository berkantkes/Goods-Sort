using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [SerializeField] private LayerController _firstLayer;
    [SerializeField] private LayerController _secondLayer;

    private int _currentLayerCount = 0;
    private LayerData[] _layers;

    public void SetShelf(ShelfData shelfData)
    {
        transform.position = shelfData.position;
        _layers = shelfData.layers;

        SetLayers();
    }

    private void SetLayers()
    {
        if (_currentLayerCount <= _layers.Length-1)
        {
            _firstLayer.SetLayer(_layers[_currentLayerCount]);
        }

        if (_currentLayerCount+1 <= _layers.Length-1)
        {
            _secondLayer.SetLayer(_layers[_currentLayerCount+1]);
        }
    }

    public bool SetItemToShelf(Vector3 point, ItemController item)
    {
        return _firstLayer.IsAvailable(point, item);
    }
}
