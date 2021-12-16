using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftResult : MonoBehaviour
{
    [SerializeField]
    private SlotPanel craftingSlotPanel;
    
    [SerializeField]
    private Inventory inventory;

    /// <summary>
    /// Removes current recipes crafting materials
    /// </summary>
    public void CollectCraftResult()
    {
        craftingSlotPanel.CollectCraftingMaterials();
    }

    /// <summary>
    /// Loops and removes current recipe crafting materials until the recipe cannot be made, or the stack is full
    /// </summary>
    /// <param name="craftableItemSlot"></param>
    public void CollectAllAvailableCraftableItems(UIItem craftableItemSlot)
    {
        int i = 0;
        while(CraftRecipeDatabase.Instance.hasCraftableItem && craftableItemSlot.item.stackIndex < craftableItemSlot.item.stackMax)
        {
            CollectCraftResult();

            // ignores crafting slot addition for fist loop, because item was already added outside of function
            if (i != 0)
            {
                Item newCraftableItemSlot = new Item(craftableItemSlot.item);
                ++newCraftableItemSlot.stackIndex;

                craftableItemSlot.UpdateItem(newCraftableItemSlot);
            }


            i++;
        }
    }
}
