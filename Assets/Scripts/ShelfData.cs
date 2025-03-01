using System;
using UnityEngine;

[Serializable]
public class ShelfData
{
    public int rowIndex;  
    public int columnIndex;
    public Vector2 position; 
    public LayerData[] layers; 
}