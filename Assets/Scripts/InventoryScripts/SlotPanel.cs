using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPanel : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();

    public int numberOfSlots;
    public GameObject slotPrefab;

    public bool craftingPanel = false;
    public bool hotkeyPanel = false;

    void Awake()
    {
        CreateInventorySlots();
    }

    void CreateInventorySlots()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            // Create new slot and set the game objects parent to be the slot transform
            // This will automatically be organized by the grid layout group on the slot panels game object
            GameObject slot = Instantiate(slotPrefab, transform);
            //slot.transform.SetParent(transform);

            // Get new slots UI Item component from the slot prefabs child object and add it to list of UI Items
            uiItems.Add(slot.GetComponentInChildren<UIItem>());
            uiItems[i].item = null;
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        uiItems[slot].UpdateItem(item);
    }

    /// <summary>
    /// Adds new item to UI items if there is a empty slot in the inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddNewItem(Item item)
    {
        int emptySlot = uiItems.FindIndex(uiItem => uiItem.item == null);
        UpdateSlot(emptySlot, item);
    }

    /// <summary>
    /// Looks for item in UI items that matches item parameter and removes it
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        //int removeSlot = uiItems.FindIndex(uiItem => uiItem.item.id == item.id);

        int removeSlot = -1;

        // Reverse loop to find last index
        for (int i = 0; i < uiItems.Count; i++)
        {
            int index = (uiItems.Count - 1) - i;
            if(uiItems[index].item != null)
            {
                if (uiItems[index].item.id == item.id)
                    removeSlot = index;
            }

        }

        // return when item is not found
        if (removeSlot == -1)
            return;

        // If removing an item from stack would result in 0, then set slot to be null
        if (uiItems[removeSlot].item.stackIndex - 1 == 0)
            UpdateSlot(removeSlot, null);
        // Else just remove one element from stack
        else
        {
            Item itemWithDeduction = new Item(uiItems[removeSlot].item);
            itemWithDeduction.stackIndex -= 1;
            UpdateSlot(removeSlot, itemWithDeduction);
        }
    }

    /// <summary>
    /// Removes all items in panel slots
    /// </summary>
    public void EmptyAllSlots()
    {
        foreach (UIItem uiItem in uiItems)
        {
            uiItem.UpdateItem(null);
        }
    }

    /// <summary>
    /// Removes items that craft-able item is comprised of
    /// </summary>
    public void CollectCraftingMaterials()
    {
        // Remove one index from each material in crafting slots
        foreach (var item in uiItems)
        {
            if(item.item != null)
            {
                Item deductedItem = new Item(item.item);
                deductedItem.stackIndex -= 1;

                // if when item is deducted the stack is empty, then remove stack
                if (deductedItem.stackIndex <= 0)
                    item.UpdateItem(null);
                else
                    item.UpdateItem(deductedItem);
            }
        }
    }

    /// <summary>
    /// Check if item can be stacked in inventory with current items, if there is no current stacks for that item then return null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void StackItem(ref Item stackItem)
    {
        for (int i = 0; i < numberOfSlots && stackItem.stackIndex != 0; i++)
        {
            if(uiItems[i].item != null && uiItems[i].item.id == stackItem.id)
            {
                // Add all of stack item to item slot
                if(uiItems[i].item.stackIndex + stackItem.stackIndex <= uiItems[i].item.stackMax)
                {
                    Item newSlotItem = new Item(uiItems[i].item);
                    newSlotItem.stackIndex += stackItem.stackIndex;

                    uiItems[i].UpdateItem(newSlotItem);

                    stackItem.stackIndex = 0;
                }
                // Add part of item stack to already existing item of same type in inventory
                else
                {
                    Item newSlotItem = new Item(uiItems[i].item);
                    Item newStackItem = new Item(stackItem);

                    int stackIndexesAvailable = newSlotItem.stackMax - newSlotItem.stackIndex;

                    newSlotItem.stackIndex = newSlotItem.stackMax;
                    newStackItem.stackIndex -= stackIndexesAvailable;

                    uiItems[i].UpdateItem(newSlotItem);
                    stackItem = newStackItem;
                }
            }
        }
    }

    /// <summary>
    /// Check if slot panel has an empty slot
    /// </summary>
    /// <returns></returns>
    public bool ContainsEmptySlot()
    {
        foreach (UIItem uiItem in uiItems)
        {
            if (uiItem.item == null)
                return true;
        }

        return false;
    }
}
