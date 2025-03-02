using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShelfData
{
    public int rowIndex;  
    public int columnIndex;
    public Vector2 position; 
    public List<LayerData> layers; 
}