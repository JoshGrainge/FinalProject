using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    private Image spriteImage;
    private UIItem selectedItem;

    private Text stackText;

    public bool isCraftingSlot = false;
    private CraftingSlots craftingSlots;

    public bool isOutputSlot = false;

    public Tooltip tooltip;

    UIInventory uiInventory;

    private void Awake()
    {

        spriteImage = GetComponent<Image>();
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();

        stackText = GetComponentInChildren<Text>();

        craftingSlots = FindObjectOfType<CraftingSlots>();
        tooltip = FindObjectOfType<Tooltip>();

        uiInventory = GameObject.FindObjectOfType<UIInventory>();

        // Set image to be transparent on spawn
        UpdateItem(null);
    }

    /// <summary>
    /// Update UI Item corresponding with item slot
    /// </summary>
    /// <param name="newSelectedItem"></param>
    public void UpdateItem(Item newSelectedItem)
    {
        item = newSelectedItem;

        if (item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;

            // Only show stack index when index is greater than 1
            if (item.stackIndex > 1)
            {
                stackText.text = item.stackIndex.ToString();
                stackText.color = Color.white;
            }
            else
            {
                stackText.color = Color.clear;
            }

        }
        // If there is no item passed then set the slots item icon to be clear
        else
        {
            spriteImage.color = Color.clear;
            stackText.color = Color.clear;
        }
        // If this is a crafting slot then update and check if there is a valid recipe 
        if (isCraftingSlot)
            craftingSlots.UpdateRecipe();

    }

    /// <summary>
    /// Remove an element from item stack
    /// </summary>
    /// <param name="stackItem"></param>
    public void DecrimentStackIndex(Item stackItem)
    {
        Item newStackItem = new Item(stackItem);
        newStackItem.stackIndex--;
        UpdateItem(newStackItem);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            // Check if the player has clicked on this item and the item reference is not null 
            if (item != null)
            {
                // If player has selected item, and selected item is the item in current slot, then merge item stacks
                if (selectedItem.item != null && selectedItem.item.id == item.id)
                {
                    // Merge stacks when both stacks don't exceed stack max
                    if (item.stackIndex + selectedItem.item.stackIndex <= item.stackMax)
                    {
                        Item newSlotItem = new Item(item);
                        newSlotItem.stackIndex = item.stackIndex + selectedItem.item.stackIndex;
                        UpdateItem(newSlotItem);
                        selectedItem.UpdateItem(null);
                    }
                    // When the sum of both stacks are greater than items stack max then add max amount possible from selected item
                    else
                    {
                        Item newSlotItem = new Item(item);
                        Item newSelectedItem = new Item(selectedItem.item);
                        
                        int remainder = (item.stackIndex + selectedItem.item.stackIndex) - item.stackMax;
                        
                        newSlotItem.stackIndex = newSlotItem.stackMax;
                        newSelectedItem.stackIndex = remainder;

                        UpdateItem(newSlotItem);
                        selectedItem.UpdateItem(newSelectedItem);
                    }
                }
                // If the selected item is not null and the slot is not the output slot, then swap items
                else if (selectedItem.item != null && !isOutputSlot)
                {
                    Item clone = new Item(selectedItem.item);

                    selectedItem.UpdateItem(item);
                    UpdateItem(clone);

                    tooltip.GenerateTooltip(selectedItem.item);
                }
                // There is no current selected item so just take the item that is stored in this slot
                else if (selectedItem.item == null)
                {
                    selectedItem.UpdateItem(item);
                    UpdateItem(null);

                    // When output slot is clicked remove the resources from inventory
                    if (isOutputSlot)
                    {
                        UICraftResult craftingResults = GetComponent<UICraftResult>();

                        // Grab all item when left shift is held
                        if (Input.GetKey(KeyCode.LeftShift))
                            craftingResults.CollectAllAvailableCraftableItems(selectedItem);
                        else
                            craftingResults.CollectCraftResult();
                    }
                }
            }
            // When this slot is empty and there is a selected object, then place the selected object in this slot
            else if (selectedItem.item != null && !isOutputSlot)
            {
                UpdateItem(selectedItem.item);
                selectedItem.UpdateItem(null);

            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isOutputSlot)
            {
                bool slotItemNull = item == null;

                // If player has selected item, and selected item is the item in current slot, then increment item index if adding does not surpass item stack max
                if (selectedItem.item != null && (slotItemNull || selectedItem.item.id == item.id))
                {
                    if (slotItemNull || item.stackIndex + 1 <= item.stackMax)
                    {
                        Item newSlotItem = null;
                        if (!slotItemNull)
                            newSlotItem = new Item(item);

                        Item newSelectedItem = new Item(selectedItem.item);

                        if (!slotItemNull)
                        {
                            newSlotItem.stackIndex = item.stackIndex + 1;
                            newSelectedItem.stackIndex = selectedItem.item.stackIndex - 1;  // Remove item from selected stack
                        }
                        else
                        {
                            // When slot item is empty then copy item and set stack index to 1
                            newSlotItem = new Item(selectedItem.item);
                            newSlotItem.stackIndex = 1;
                            newSelectedItem.stackIndex = selectedItem.item.stackIndex - 1;  // Remove item from selected stack
                        }

                        // If there is no remaining items in stack then set selected item to null
                        if (newSelectedItem.stackIndex <= 0)
                            newSelectedItem = null;

                        UpdateItem(newSlotItem);
                        selectedItem.UpdateItem(newSelectedItem);
                    }
                }
            }
        }
        

        // Update the items in players hot key bar every time a new item is added to hot key panel
        UpdateHotkeysItems(this);

        UpdateInventory();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null && selectedItem.item == null)
        {
            tooltip.GenerateTooltip(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectedItem.item == null)
            tooltip.TurnOffTooltip();
    }

    public void UpdateHotkeysItems(UIItem uiItem)
    {
        HotkeyPanel hotkeyPanel = uiItem.transform.parent.parent.transform.GetComponent<HotkeyPanel>();
        if (hotkeyPanel != null)
        {
            hotkeyPanel.UpdateItems();
        }
    }

    void UpdateInventory()
    {
        if (!isOutputSlot)
        {
            uiInventory.UpdatePlayerInventory();
        }
    }

    
}
