using System;
using UnityEngine;

[Serializable]
public class ShelfData
{
    public int rowIndex;   // Hangi satırda
    public int columnIndex; // Hangi sütunda
    public Vector2 position; // X ve Y pozisyonu
    public LayerData[] layers; // Bu çatının içindeki katmanlar
}