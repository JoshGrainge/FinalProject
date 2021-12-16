using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> dropItems = new List<GameObject>();

    [SerializeField]
    protected List <Transform> spawnTransforms = new List<Transform>();

    /// <summary>
    /// Spawn loot when on object being broken
    /// </summary>
    public void Break()
    {
        DropLoot();
        Destroy(gameObject);
    }

    /// <summary>
    /// Drop loot that is a fixed pool
    /// </summary>
    protected virtual void DropLoot()
    {
        for (int i = 0; i < dropItems.Count; i++)
        {
            Rigidbody itemRb = Instantiate(dropItems[i], spawnTransforms[i].position, spawnTransforms[i].rotation).GetComponent<Rigidbody>();
            AddImpulseToLootObject(itemRb);
        }
    }

    /// <summary>
    /// Sends loot items in random directions
    /// </summary>
    /// <param name="itemRb"></param>
    protected void AddImpulseToLootObject(Rigidbody itemRb)
    {
        float impulseSpeedMin = 0.25f;
        float impulseSpeedMax = 1.25f;
        float impulseSpeed = Random.Range(impulseSpeedMin, impulseSpeedMax);
        itemRb.AddRelativeForce(Random.onUnitSphere * impulseSpeed);
    }

}
