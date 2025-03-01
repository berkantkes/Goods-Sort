using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayerData
{
    public ItemType FirstItemType;
    public ItemType SecondItemType;
    public ItemType ThirdItemType;

    public List<ItemType> GetItemTypesList()
    {
        return new List<ItemType> { FirstItemType, SecondItemType, ThirdItemType };
    }
}
