using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    // Probably going to get rid of this, I dont see it's purpose
    public List<Item> playerItems = new List<Item>();

    // Create array of inventory items the size of inventory + hotkeys
    //public Item[] playerItems;

    private UIInventory uiInventory;

    public Equipment currentEquipment;

    public bool inventoryOpen;

     private void Awake()
     {
         uiInventory = GameObject.FindObjectOfType<UIInventory>();
     }

    // Test add items
    private void Start()
    {
        GiveItem(1);
        GiveItem(1);
        GiveItem(1);
        for (int i = 0; i < 32 * 2; i++)
        {
            GiveItem(1);
        }


        GiveItem(2);
        GiveItem(2);
        GiveItem(2);
        GiveItem(2);
        GiveItem(2);
        GiveItem(2);

        GiveItem(3);
        GiveItem(3);
        GiveItem(3);
        GiveItem(3);
        GiveItem(3);


        GiveItem(3);

        GiveItem(4);
        GiveItem(5);

        GiveItem("Torch");


        GiveItem("Bow");
        GiveItem("Bow");
        GiveItem("Bow");

        GiveItem("Axe");

        Item fishStack = new Item(ItemDatabase.Instance.GetItem("Fish"));
        fishStack.stackIndex = 5;
        GiveItem(fishStack);


        // Turn off inventory after giving initial test items
        uiInventory.ToggleInventoryVisibility(false);

        //         for (int i = 0; i < 23; i++)
        //         {
        //             GiveItem(12);
        //         }

    }

    /// <summary>
    /// Add item to players inventory by item id
    /// </summary>
    /// <param name="id"></param>
    public void GiveItem(int id)
    {
        // TODO check if there is room in players inventory before giving item
        Item itemToAdd = ItemDatabase.Instance.GetItem(id);

        uiInventory.AddItemToInventory(new Item(itemToAdd));
        //playerItems.Add(new Item(itemToAdd));
    }

    /// <summary>
    /// Add item to players inventory by item name
    /// </summary>
    /// <param name="itemName"></param>
    public void GiveItem(string itemName)
    {
        Item itemToAdd = ItemDatabase.Instance.GetItem(itemName);

        uiInventory.AddItemToInventory(new Item(itemToAdd));

        //playerItems.Add(new Item(itemToAdd));
    }

    /// <summary>
    /// Add preexisting item to players inventory
    /// </summary>
    /// <param name="itemToAdd"></param>
    public void GiveItem(Item itemToAdd)
    {
        uiInventory.AddItemToInventory(new Item(itemToAdd));

        //playerItems.Add(new Item(itemToAdd));
    }

    public Item CheckForItem(int id)
    {
        // reverse
        //         int arrayEnd = playerItems.Length;
        //         for (int i = 0; i < arrayEnd; i++)
        //         {
        //             if (playerItems[arrayEnd - i].id == id)
        //                 return playerItems[i];
        //         }
        //return playerItems.Find(item => item.id == id);

        Item? foundItem = playerItems.FindLast(item => item.id == id);

        if (foundItem != null)
            return foundItem;
        else
            return null;
    }

    public Item CheckForItem(string itemName)
    {
        int arrayEnd = playerItems.Count;
        
        // Loop through inventory items in reverse order
        for (int i = 0; i < arrayEnd; i++)
        {
            int index = (arrayEnd - 1) - i;
            if (playerItems[index] != null && playerItems[index].itemName == itemName)
                return playerItems[index];
        }
        //Item? foundItem = playerItems.FindLast(item => item.itemName == itemName);

        return null;
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);

        //         for (int i = 0; i < playerItems.Length; i++)
        //         {
        //             if (playerItems[i].id == id)
        //                 playerItems[i] = null;
        //         }

        uiInventory.RemoveItemFromInventory(new Item(itemToRemove));
    }

    public void RemoveItem(string itemName)
    {
        Item itemToRemove = CheckForItem(itemName);

        //         for (int i = 0; i < playerItems.Length; i++)
        //         {
        //             if (playerItems[i].id == id)
        //                 playerItems[i] = null;
        //         }

        uiInventory.RemoveItemFromInventory(new Item(itemToRemove));
    }

    public bool CheckForEmptySlot()
    {
        return uiInventory.HasEmptySlot();
    }

    
    
    
}
