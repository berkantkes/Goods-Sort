using UnityEngine;

[System.Serializable]
public class PoolItemData
{
    public ItemController itemControllerPrefab;

    public ItemType ItemType => itemControllerPrefab != null ? itemControllerPrefab.GetItemType() : ItemType.None;
}