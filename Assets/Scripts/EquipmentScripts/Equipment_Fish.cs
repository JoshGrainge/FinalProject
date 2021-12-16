using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Fish : Equipment
{
    [SerializeField]
    float healthAddition;

    [SerializeField]
    float eatTimer = 2.5f;
    float eatStartTime = 0;

    bool eating = false;

    Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is eating fish
        if (Input.GetMouseButtonDown(1))
        {
            eating = true;
            anim.SetBool("eating", eating);
            eatStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            eating = false;
            anim.SetBool("eating", eating);
        }

        // Check if player has finished eating timer, if so replenish health and destroy fish
        if (eating)
        {
            if (Time.time >= eatStartTime + eatTimer)
            {
                Transform player = transform.root;
                player.GetComponent<HealthScript>().Heal(healthAddition);

                // Removes fish from inventory
                HotkeyPanel hotkeyPanel = GameObject.Find("HotkeyPanel").GetComponentInChildren<HotkeyPanel>();
                int slotIndex = hotkeyPanel.currentSlot - 1;
                hotkeyPanel.uiItems[slotIndex].DecrimentStackIndex(hotkeyPanel.uiItems[slotIndex].item);

                // If there is no more fish destroy fish equipment and remove hotkey item
                if (hotkeyPanel.uiItems[slotIndex].item.stackIndex < 1)
                {
                    hotkeyPanel.RemoveItemAtIndex(hotkeyPanel.currentSlot - 1);
                    Destroy(gameObject);
                }
                hotkeyPanel.UpdateItems();

            }
        }
    }

    public override void Attack() {}
}
