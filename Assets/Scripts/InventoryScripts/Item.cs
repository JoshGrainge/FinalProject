using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public int stackIndex;
    public int stackMax;
    public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public Item(int Id, string ItemName, string Description, int StackMax, Dictionary<string, int> Stats)
    {
        id = Id;
        itemName = ItemName;
        description = Description;
        stackIndex = 1;
        stackMax = StackMax;
        stats = Stats;

        // Looks in the resources folder for sprite matching items name, then sets that sprite to be items icon
        icon = Resources.Load<Sprite>("Items/" + itemName);
        
    }

    public Item(Item item)
    {
        id = item.id;
        itemName = item.itemName;
        description = item.description;
        stackIndex = item.stackIndex;
        stackMax = item.stackMax;
        icon = item.icon;
        stats = item.stats;
    }

    
}
