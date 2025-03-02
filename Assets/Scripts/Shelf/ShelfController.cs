using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [SerializeField] private LayerController _firstLayer;

    private int _currentLayerCount = 0;
    private LayerData[] _layers;
    private LevelManager _levelManager;

    public void SetShelf(ShelfData shelfData, LevelManager levelManager)
    {
        transform.position = shelfData.position;
        _firstLayer.SetLayerData(shelfData.layers);
        _levelManager = levelManager;
        _firstLayer.SetShelfController(this);
    }

    public bool SetItemToShelf(Vector3 point, ItemController item)
    {
        return _firstLayer.IsAvailable(point, item);
    }

    public void ControlGameStatus()
    {
        _levelManager.ControlGameStatus();
    }
    public bool AreAllFrontShelfSpacesEmpty()
    {
        return _firstLayer.AreAllFrontShelfSpacesEmpty();
    }

}
