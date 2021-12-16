using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemScript : MonoBehaviour
{
    public string itemName;

    public Item item;

    Inventory inventory;

    AudioClip pickupSound;

    void Awake()
    {
        // I chose this implmentation because assigning a sound file to 20 files was gonna waste too much time
        // Load sound from resources folder (Resource folder is a special folder that anything inside it builds with the game)
        pickupSound = Resources.Load<AudioClip>("Sounds/ItemPickup");

        // Get item from item database
        inventory = FindObjectOfType<Inventory>();

        // Get item if world item does not have one assigned
        item = ItemDatabase.Instance.GetItem(itemName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PickupObject();
    }

    private void PickupObject()
    {
        float pickupSoundVolume = 1f;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);

        // TODO make sure item picked up
        inventory.GiveItem(item);
        Destroy(gameObject);

        //if (inventory.CheckForEmptySlot())
        //{
        //    inventory.GiveItem(item.id);
        //    Destroy(gameObject);
        //}
    }

}
