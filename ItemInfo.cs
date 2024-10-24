using UnityEngine;

[System.Serializable]
public partial struct ItemInfo
{
    public string Id;

    public ItemInfo(string id)
    {
        Id = id;
    }

    public Item data
    {
        get
        {
            return Item.Cache[Id] as Item;
        }
    }
}