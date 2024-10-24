using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CraftingEntry
{
    public ItemInfo item;
    public float startAt;
    public float readyAt;

    public CraftingEntry(string itemId)
    {
        item = new(itemId);
        startAt = Time.time;
        readyAt = Time.time + item.data.productionTime;
    }

    public bool Completed() => Time.time >= readyAt;
}