using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// menu entry
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]

public abstract class Item : ScriptableObject
{
    [SerializeField]
    string id = "";
    public string ItemId { get { return id; } }
    public string itemName;
    public Sprite icon;
    public string description;
    public float volume;
    
    [Header("Crafting")]
    public List<InventoryItem> requiredMaterials;
    public float productionTime = 1f;

    static Dictionary<string, Item> _cache;
    public static Dictionary<string, Item> Cache
    {
        get
        {
            if (_cache == null)
            {
                Item[] items = Resources.LoadAll<Item>("");

                _cache = items.ToDictionary(item => item.ItemId, item => item);
            }
            return _cache;
        }
    }

    private void OnValidate()
    {
        if (ItemId == "")
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
#endif
        }
    }

}