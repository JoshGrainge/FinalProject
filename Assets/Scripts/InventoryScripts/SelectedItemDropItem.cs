using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedItemDropItem : MonoBehaviour
{
    UIItem uiItem;

    Transform player;


    private void Awake()
    {
        uiItem = GetComponent<UIItem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Drop item when player clicks off of inventory
        if (Input.GetMouseButtonDown(0))
        {
            bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
            if (!isMouseOverUI)
            {
                if (uiItem.item != null)
                {
                    DropCurrentObject();
                }
            }
        }
    }

    void DropCurrentObject()
    {
        float forwardOffset = 3f;
        Vector3 spawnOffset = player.forward * forwardOffset;
        GameObject worldItem = WorldItemDatabase.Instance.GetWorldItem(uiItem.item.itemName).gameObject;
        WorldItemScript spawnItemScript = Instantiate(worldItem, player.position + spawnOffset, player.rotation).GetComponent<WorldItemScript>();

        spawnItemScript.item = uiItem.item;

        uiItem.tooltip.TurnOffTooltip();
        uiItem.UpdateItem(null);
    }
}
