using System.Collections.Generic;
using UnityEngine;

public static class Craft
{    
    // check inventory for items
    public static bool Can(int amount, string itemId, Inventory inventory)
    {
        ItemInfo item = new(itemId);
        return inventory.HaveEnoughList(item.data.requiredMaterials);
    }

    public static CraftingEntry Start(string itemId, Inventory inventory)
    {
        ItemInfo info = new(itemId);
        inventory.RemoveList(info.data.requiredMaterials);
        return new CraftingEntry(itemId);
    }

    public static InventoryItem Complete(CraftingEntry craft) => new(craft.item.data.ItemId, 1);

    public static List<InventoryItem> Cancel(CraftingEntry craft)
    {
        // destroy materials based on progress
        // comment and just return the mats from crafting entry if not desired

        List<InventoryItem> leftovers = new List<InventoryItem>();

        float elapsedTime = Time.time - craft.startAt;
        float completionPercentage = Mathf.Clamp01(elapsedTime / (craft.readyAt - craft.startAt));

        float destroyPercentage = completionPercentage / craft.item.data.requiredMaterials.Count * 0.5f;

        // comment the rest and just return this part which is the mats from crafting entry if destruction is not desired
        foreach (var material in craft.item.data.requiredMaterials)
        {
            InventoryItem leftover = new(material.item.Id, material.quantity - Mathf.RoundToInt(material.quantity * destroyPercentage));
            leftovers.Add(leftover);
        }

        return leftovers;
    }
}