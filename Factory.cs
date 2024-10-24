using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Factory
{
    private Inventory _inventory = null;
    public List<string> charactersIds = new List<string>();
    // this is a factory example
    // so we have a set list of goods which we work on
    // to test this, you are gonna have to register your own scriptableObject using Item.cs
    public List<InventoryItem> goods = new List<InventoryItem>();
    private List<CraftingEntry> productionQueu = new List<CraftingEntry>();
    public int maxQueueSize = 3;
    private int lastGoodIndex = -1;

    public Factory(Inventory inventory)
    {
        _inventory = inventory;
        productionQueu = new();
    }

    public void Init(Inventory inventory)
    {
        _inventory = inventory;
        productionQueu = new();
    }

    public void AddWorker(string id)
    {
        charactersIds.Add(id);
    }

    public void Work()
    {
        if(_inventory == null)
        {
            return;
        }

        if (productionQueu.Count < maxQueueSize)
        {
            lastGoodIndex = (lastGoodIndex + 1) % goods.Count;

            if(!_inventory.HaveEnough(goods[lastGoodIndex]))
            {
                if(Craft.Can(1, goods[lastGoodIndex].item.Id, _inventory))
                {
                    CraftingEntry craftingEntry = Craft.Start(goods[lastGoodIndex].item.Id, _inventory);
                    productionQueu.Add(craftingEntry);
                }
            }
        }

        foreach (var craft in productionQueu)
        {
            if (craft.Completed())
            {
                InventoryItem result = Craft.Complete(craft);
                _inventory.AddItem(result);
            }
        }

        productionQueu.RemoveAll(x => x.Completed());
    }

}