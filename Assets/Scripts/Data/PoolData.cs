using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolData", menuName = "Match3/Pool Data")]
public class PoolData : ScriptableObject
{
    public List<ItemController> itemControllers; // Artık direkt ItemController listesi var

    public ItemController GetItemController(ItemType itemType)
    {
        return itemControllers.Find(item => item.GetItemType() == itemType);
    }
}