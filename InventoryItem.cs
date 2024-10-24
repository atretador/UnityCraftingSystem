using UnityEngine;
using System;

[Serializable]
public class InventoryItem
{
    public ItemInfo item;

    public int quantity;

    public InventoryItem(){}

    public InventoryItem(string itemId, int itemQuantity)
    {
        item = new(itemId);
        quantity = itemQuantity;
    }
}