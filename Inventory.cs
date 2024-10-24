using UnityEngine;
using System.Collections.Generic;

// volume based inventory system

[System.Serializable]
public class Inventory
{
    public float maxInventoryVolume = 50f;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public bool HaveEnoughList(List<InventoryItem> toCheck)
    {
        bool value = false;

        foreach (var check in toCheck)
        {
            value = HaveEnough(check);
            if(!value)
            {
                break;
            }
        }
        return value;
    }
    
    public bool HaveEnough(InventoryItem check)
    {
        var itemFound = inventory.Find(invItem => invItem.item.data.ItemId == check.item.Id);

        if(itemFound != null)
        {
            if(itemFound.quantity >= check.quantity)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveList(List<InventoryItem> toRemove)
    {
        foreach (var remove in toRemove)
        {
            RemoveItem(remove);
        }
    }

    public void RemoveItem(InventoryItem toRemove)
    {
        var existingItem = inventory.Find(invItem => invItem.item.data == toRemove.item.data);

        if (existingItem != null)
        {
            if(existingItem.quantity <= toRemove.quantity)
            {
                inventory.Remove(existingItem);
            }
            else
            {
                existingItem.quantity -= toRemove.quantity;
            }
        }
    }

    public void AddList(List<InventoryItem> toAdd)
    {
        foreach (var add in toAdd)
        {
            AddItem(add);
        }
    }

    public int AddItem(InventoryItem toAdd)
    {
        float requiredVolume = toAdd.item.data.volume * toAdd.quantity;

        if (CanFitItemVolume(requiredVolume))
        {
            // Check if the item already exists in the inventory
            InventoryItem existingItem = inventory.Find(invItem => invItem.item.data == toAdd.item.data);

            if (existingItem != null)
            {
                // Calculate the available space in the inventory
                float availableSpace = maxInventoryVolume - GetCurrentInventoryVolume();

                if (availableSpace >= requiredVolume)
                {
                    // There is enough space in the inventory
                    existingItem.quantity += toAdd.quantity;
                    return 0; // No leftover items
                }
                else
                {
                    // Add as much as possible to the existing stack, and calculate leftover items
                    int addedQuantity = Mathf.FloorToInt(availableSpace / toAdd.item.data.volume);
                    existingItem.quantity += addedQuantity;
                    return toAdd.quantity - addedQuantity; // Return the leftover quantity
                }
            }
            else
            {
                // Check if the new item can fit in the remaining inventory space
                if (requiredVolume <= maxInventoryVolume - GetCurrentInventoryVolume())
                {
                    // Create a new stack in the inventory
                    inventory.Add(new InventoryItem(toAdd.item.Id, toAdd.quantity));
                    return 0; // No leftover items
                }
                else
                {
                    return toAdd.quantity; // Return the full quantity as leftover items
                }
            }
        }
        else
        {
            return toAdd.quantity; // Return the full quantity as leftover items
        }
    }

    public List<InventoryItem> GetItems()
    {
        return inventory;
    }

    private bool CanFitItemVolume(float requiredVolume)
    {
        // Check if adding the new items exceeds the inventory volume limit
        return GetCurrentInventoryVolume() + requiredVolume <= maxInventoryVolume;
    }

    public bool CanFitItemList(List<InventoryItem> toAdd)
    {
        float required = 0;
        foreach (var item in toAdd)
        {
            required += item.quantity * item.item.data.volume;
        }

        return CanFitItemVolume(required);
    }

    public bool IsFull()
    {
        return GetCurrentInventoryVolume() == maxInventoryVolume;
    }

    private float GetCurrentInventoryVolume()
    {
        float totalVolume = 0f;
        foreach (var invItem in inventory)
        {
            totalVolume += invItem.item.data.volume * invItem.quantity;
        }
        return totalVolume;
    }
}