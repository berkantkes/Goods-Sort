using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [SerializeField] private LayerController _firstLayer;

    private int _currentLayerCount = 0;
    private LayerData[] _layers;

    private void Awake()
    {
        //_firstLayer.
    }

    public void SetShelf(ShelfData shelfData)
    {
        transform.position = shelfData.position;
        _firstLayer.SetLayerData(shelfData.layers);
        //_layers = shelfData.layers;

        //SetLayers();
    }

    // private void SetLayers()
    // {
    //     if (_currentLayerCount <= _layers.Length-1)
    //     {
    //         _firstLayer.SetFrontLayer(_layers[_currentLayerCount]);
    //     }
    //
    //     if (_currentLayerCount+1 <= _layers.Length-1)
    //     {
    //         _secondLayer.SetBackLayer(_layers[_currentLayerCount+1]);
    //     }
    // }

    public bool SetItemToShelf(Vector3 point, ItemController item)
    {
        return _firstLayer.IsAvailable(point, item);
    }
}
