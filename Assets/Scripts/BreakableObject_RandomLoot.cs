using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject_RandomLoot : BreakableObject
{
    [SerializeField]
    int minItems = 1, maxItems = 5;

    /// <summary>
    /// Drops random amount and variety of loot
    /// </summary>
    protected override void DropLoot()
    {
        int itemNum = Random.Range(minItems, maxItems);

        for (int i = 0; i < itemNum; i++)
        {
            // Get random item in list of random items
            GameObject randomLootItem = dropItems[Random.Range(0, dropItems.Count)];
            Rigidbody itemRb = Instantiate(randomLootItem, spawnTransforms[0].position, spawnTransforms[0].transform.rotation).GetComponent<Rigidbody>();

            AddImpulseToLootObject(itemRb);
        }
    }
}
