using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyPanel : SlotPanel
{
    const int SLOTMAX = 6;

    [Range(1, SLOTMAX)]
    public int currentSlot = 1;

    Inventory inventory;
    PlayerEquipmentManager playerEquipmentManager;

    public List<Item> hotkeyItems = new List<Item>();
    float slotOffset;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerEquipmentManager = inventory.gameObject.GetComponent<PlayerEquipmentManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSlot = 1;
            EquipCurrentItem();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSlot = 2;
            EquipCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSlot = 3;
            EquipCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentSlot = 4;
            EquipCurrentItem();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentSlot = 5;
            EquipCurrentItem();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentSlot = 6;
            EquipCurrentItem();
        }

    }

    public void UpdateItems()
    {
        hotkeyItems.Clear();

        foreach (var uiItem in uiItems)
        {
            if (uiItem.item != null)
                hotkeyItems.Add(uiItem.item);
        }

        EquipCurrentItem();
    }

    void EquipCurrentItem()
    {
        // If the current slot has an item then set the new item as the players current equipment
        if (uiItems[currentSlot - 1].item != null)
        {
            // Destroy old equipment player is holding
            if (playerEquipmentManager.currentEquipment != null)
            {
                Destroy(playerEquipmentManager.currentEquipment.gameObject);
                playerEquipmentManager.currentEquipment = null;
            }

            // If the current slot is equipment then Spawn new equipment and set new equipment as players current equipment
            var equipment = EquipmentDatabase.Instance.GetEquipment(uiItems[currentSlot - 1].item.itemName);
            if (equipment.gameObject != null)
            {
                GameObject equipmentObject = Instantiate(equipment.gameObject,
                                                         playerEquipmentManager.equipmentSocket.position,
                                                         playerEquipmentManager.equipmentSocket.rotation,
                                                         playerEquipmentManager.equipmentSocket);

                playerEquipmentManager.currentEquipment = equipmentObject.GetComponent<Equipment>();
            }
        }
        // Remove current equipment when player selects empty hotkey slot
        else if(playerEquipmentManager.currentEquipment != null)
        {
            Destroy(playerEquipmentManager.currentEquipment.gameObject);
            playerEquipmentManager.currentEquipment = null;
        }
        
    }

    /// <summary>
    /// Removes hotkey item at index
    /// </summary>
    /// <param name="inventoryIndex"></param>
    public void RemoveItemAtIndex(int inventoryIndex)
    {
        hotkeyItems.RemoveAt(inventoryIndex);
        UpdateSlot(inventoryIndex, null);
    }

}
