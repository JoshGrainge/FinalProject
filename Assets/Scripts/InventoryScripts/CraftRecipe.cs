using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe
{
    public int[] requiredItems;
    public int itemToCraft;

    public CraftRecipe(int ItemToCraft, int[] RequiredItems)
    {
        itemToCraft = ItemToCraft;
        requiredItems = RequiredItems;
    }
}
