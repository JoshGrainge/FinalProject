using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftRecipeDatabase : MonoBehaviour
{
    private static CraftRecipeDatabase _instance;
    public static CraftRecipeDatabase Instance { get { return _instance; } }

    public List<CraftRecipe> recipes = new List<CraftRecipe>();

    public bool hasCraftableItem;

    // Assign static instance variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        BuildCraftRecipeDatabase();
    }

    /// <summary>
    /// Check if the attempted recipe is in recipe database and if so returns item
    /// </summary>
    /// <param name="recipeAttempt"></param>
    /// <returns></returns>
    public Item CheckRecipe(int[] recipeAttempt)
    {
        foreach (CraftRecipe recipe in recipes)
        {
            if(recipe.requiredItems.SequenceEqual(recipeAttempt))
            {
                Item recipeItem = ItemDatabase.Instance.GetItem(recipe.itemToCraft);

                return recipeItem;
            }
        }

        return null;
    }


    /// <summary>
    /// Initialize all recipe combinations based on the required items id and position
    /// </summary>
    void BuildCraftRecipeDatabase()
    {
        recipes = new List<CraftRecipe>()
        {
            #region Stick Crafting Options
            new CraftRecipe(2,
            new int[] {
                0, 0, 0,
                1, 0, 0,
                1, 0, 0
            }),
            new CraftRecipe(2,
            new int[] {
                0, 0, 0,
                0, 1, 0,
                0, 1, 0
            }),
            new CraftRecipe(2,
            new int[] {
                0, 0, 0,
                0, 0, 1,
                0, 0, 1
            }),
            new CraftRecipe(2,
            new int[] {
                1, 0, 0,
                1, 0, 0,
                0, 0, 0
            }),
            new CraftRecipe(2,
            new int[] {
                0, 1, 0,
                0, 1, 0,
                0, 0, 0
            }),
            new CraftRecipe(2,
            new int[] {
                0, 0, 1,
                0, 0, 1,
                0, 0, 0
            }),
            #endregion

            #region Rope Crafting Options
            new CraftRecipe(3,
            new int[] {
                2, 0, 0,
                2, 0, 0,
                2, 0, 0
            }),
            new CraftRecipe(3,
            new int[] {
                0, 2, 0,
                0, 2, 0,
                0, 2, 0
            }),
            new CraftRecipe(3,
            new int[] {
                0, 0, 2,
                0, 0, 2,
                0, 0, 2
            }),
            #endregion

            #region Equipment Crafting

            #region Torch combinations
            new CraftRecipe(9,
            new int[] {
                5, 0, 0,
                2, 0, 0,
                2, 0, 0
            }),

            new CraftRecipe(9,
            new int[] {
                0, 5, 0,
                0, 2, 0,
                0, 2, 0
            }),

            new CraftRecipe(9,
            new int[] {
                0, 0, 5,
                0, 0, 2,
                0, 0, 2
            }),

            #endregion

            // Wood Pickaxe
            new CraftRecipe(10,
            new int[] {
                1, 1, 1,
                0, 2, 0,
                0, 2, 0
            }),
            // Wood Axe
            new CraftRecipe(11,
            new int[] {
                0, 1, 1,
                0, 2, 1,
                0, 2, 0
            }),
            
            // Fishing rod
            new CraftRecipe(12,
            new int[] {
                0, 0, 2,
                0, 2, 3,
                2, 0, 3
            }),

            // Bow
            new CraftRecipe(13,
            new int[] {
                3, 2, 0,
                3, 0, 2,
                3, 2, 0
            }),

            // Arrow
            new CraftRecipe(14,
            new int[] {
                0, 0, 4,
                0, 2, 0,
                2, 0, 0
            })
            #endregion
        };
    }
}
