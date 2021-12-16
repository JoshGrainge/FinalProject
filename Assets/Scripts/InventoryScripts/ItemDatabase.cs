using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase _instance;
    public static ItemDatabase Instance { get { return _instance; } }

    public List<Item> items = new List<Item>();

    int equipmentStackMax = 1;
    int resourceStackMax = 32;

    // Assign static instance variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        BuildItemDatabase();
    }

    // Loop through all items until the passed id is found, then return item with that id
    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    // Return item from item database based on name
    public Item GetItem(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }

    void BuildItemDatabase()
    {
        items = new List<Item>()
        {
            new Item(1, "Wood", "Wood log that is useful for crafting items", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(2, "Stick", "Wooden stick that can be used in crafting", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(3, "Rope", "Strong twisted strands of bark braided together, great for crafting", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(4, "Rock", "Stone chunk that can be used for tools", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(5, "Coal", "Can be used for fueling a fire", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(7, "Fish", "Can be eaten to restore health", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(9, "Torch", "Can be eaten to restore health", resourceStackMax,
            new Dictionary<string, int> {
            }),

            new Item(10, "Pickaxe", "Pickaxe forged from diamonds", equipmentStackMax,
            new Dictionary<string, int> {
                { "Power", 8},
                { "Durability", 7},
                { "Block", 4}
            }),

            // Axe item
            new Item(11, "Axe", "Axe made of steel", equipmentStackMax,
            new Dictionary<string, int>
            {
                { "Power", 6},
                { "Durability", 6},
                { "Block", 3}
            }),

            // Fishing Rod
            new Item(12, "Fishing Rod", "Teach a man to fish...", equipmentStackMax,
            new Dictionary<string, int>
            {
                { "Power", 0},
                { "Durability", 10},
                { "Block", 0}
            }),

            // Bow
            new Item(13, "Bow", "For dispatching enemies from a distance", equipmentStackMax,
            new Dictionary<string, int>
            {
                { "Power", 0},
                { "Durability", 10},
                { "Block", 0}
            }),

            // Arrow
            new Item(14, "Arrow", "Stick them in any opposition", resourceStackMax,
            new Dictionary<string, int> {
            }),
        };
    }
}
