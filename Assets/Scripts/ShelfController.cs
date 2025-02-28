using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [SerializeField] private LayerController _firstLayer;
    [SerializeField] private LayerController _secondLayerLayer;

    private int _layerCount;
    private LayerData[] _layers;

    public void SetShelf(ShelfData shelfData)
    {
        transform.position = shelfData.position;
        _layers = shelfData.layers;
        
    }

}
