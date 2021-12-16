using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private SlotPanel[] slotPanels;

    bool inventoryOn = false;

    Vector3 startVector;

    [SerializeField]
    UIItem selectedItem;

    PlayerController player;

    Inventory playerInventory;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Get inventory initial position
        startVector = transform.position;

        playerInventory = GameObject.FindObjectOfType<Inventory>();
    }


    private void Update()
    {
        // Toggle inventory when button is pressed
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryOn = !inventoryOn;
            ToggleInventoryVisibility(inventoryOn);
            playerInventory.inventoryOpen = inventoryOn;


            // Cursor inventory logic
            Cursor.lockState = inventoryOn ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = inventoryOn;
        }
    }

    public void AddItemToInventory(Item item)
    {
        foreach (SlotPanel panel in slotPanels)
        {
            // stop loop when item has been added to inventory, added this because items were being added twice
            if (item.stackIndex == 0)
                break;

            // Attempt to add item to non crafting panels
            if(!panel.craftingPanel)
            {
                // Stack item if player has items of same type
                panel.StackItem(ref item);
                
                
                if (item.stackIndex > 0 && panel.ContainsEmptySlot())
                {
                    panel.AddNewItem(item);
                    break;
                }
            }

        }

        // Update players inventory
        UpdatePlayerInventory();
    }

    /// <summary>
    /// Remove element from stack, if the last element is removed, then set inventory slot to be empty
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItemFromInventory(Item item)
    {
        foreach (SlotPanel panel in slotPanels)
        {
            if (!panel.craftingPanel)
            {
                if (item.stackIndex == 0)
                    break;

                panel.RemoveItem(item);
            }
        }

        // Update players inventory
        UpdatePlayerInventory();
    }

    public bool HasEmptySlot()
    {
        foreach (SlotPanel panel in slotPanels)
        {
            // Attempt to add item to non crafting panels
            if (!panel.craftingPanel && panel.ContainsEmptySlot())
            {
                return true;
            }

        }

        return false;
    }

    /// <summary>
    /// Set visible state of inventory
    /// </summary>
    /// <param name="newActive"></param>
    public void ToggleInventoryVisibility(bool newActive)
    {
        Vector3 endPos = new Vector3(0, 10000, 0);

        // move inventory to its starting position
        if (newActive)
            transform.position = startVector;
        // Move inventory off screen
        else
        {
            // When player has selected item drop selected item in the world
            if (selectedItem.item != null)
                DropSelectedItem();

            transform.position = endPos;
        }

        
    }

    /// <summary>
    /// Drops selected item when inventory is closed
    /// </summary>
    void DropSelectedItem()
    {
        float forwardOffset = 20f;
        Vector3 spawnOffset = player.transform.forward * forwardOffset;
        GameObject worldItem = WorldItemDatabase.Instance.GetWorldItem(selectedItem.item.itemName).gameObject;
        WorldItemScript spawnItemScript = Instantiate(worldItem, player.transform.position + spawnOffset, player.transform.rotation).GetComponent<WorldItemScript>();

        spawnItemScript.item = selectedItem.item;

        selectedItem.tooltip.TurnOffTooltip();
        selectedItem.UpdateItem(null);
    }


    /// <summary>
    /// Syncs the inventory slots in UI with player inventory slots
    /// </summary>
    public void UpdatePlayerInventory()
    {
        foreach (SlotPanel slot in slotPanels)
        {
            if(!slot.craftingPanel)
            {
                // Add 
                int hotkeyOffset = 0;
                if (slot.hotkeyPanel)
                    hotkeyOffset = playerInventory.playerItems.Count - slot.numberOfSlots;

                for (int i = 0; i < slot.uiItems.Count; i++)
                {
                    int playerItemIndex = i + hotkeyOffset;
                    playerInventory.playerItems[playerItemIndex] = slot.uiItems[i].item;
                }
            }
        }
    }

    
}
