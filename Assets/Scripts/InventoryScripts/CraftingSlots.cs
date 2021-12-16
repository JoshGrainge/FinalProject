using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlots : MonoBehaviour
{
    private List<UIItem> uiItems = new List<UIItem>();

    public UIItem outputSlot;

    void Start()
    {
        // Copy UI Items from game object component
        uiItems = GetComponent<SlotPanel>().uiItems;
        // Set all ui items in crafting panel to be crafting slots
        uiItems.ForEach(i => i.isCraftingSlot = true);
    }

    public void UpdateRecipe()
    {
        int[] itemsInCraftingSlots = new int[uiItems.Count];
        for (int i = 0; i < uiItems.Count; i++)
        {
            // If there is an item in the i th slot then get that item id and add it to items in crafting slots array
            if(uiItems[i].item != null)
            {
                itemsInCraftingSlots[i] = uiItems[i].item.id;
            }
        }

        // Check if there is a craft-able item 
        Item itemToCraft = CraftRecipeDatabase.Instance.CheckRecipe(itemsInCraftingSlots);

        // Keep boolean value of if there is a valid craftable item from recipe
        if (itemToCraft != null)
            CraftRecipeDatabase.Instance.hasCraftableItem = true;
        else
            CraftRecipeDatabase.Instance.hasCraftableItem = false;


        UpdateOutputSlot(itemToCraft);
    }

    void UpdateOutputSlot(Item itemToCraft)
    {
        outputSlot.UpdateItem(itemToCraft);

    }
}
