using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public Equipment currentEquipment;
    public Transform equipmentSocket;

    Animator anim;

    float nextHitTime;
    [SerializeField]
    bool isCoolingDown = false;

    bool chargingattack = false;
    float chargeEndTime;

    Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        // Does not execute attack when inventory is open
        // When button clicked execute attack logic
        if (Input.GetButton("Attack") && !inventory.inventoryOpen)
        {
            if (currentEquipment != null && !isCoolingDown)
            {
                if (currentEquipment.hasChargeAttack)
                {
                    if(!chargingattack)
                    {
                        chargingattack = true;
                        chargeEndTime = Time.time + currentEquipment.chargeTime;
                    }
                }
                else
                {
                    AnimationLogic(true);

                    currentEquipment.Attack();
                    // Calculate next time weapon can swing
                    nextHitTime = Time.time + currentEquipment.attackSpeed;

                    isCoolingDown = true;
                }
            }
        }
        else if(chargingattack)
        {
            chargingattack = false;
            // If player has charged attack enough time then execute attack
            if (Time.time >= chargeEndTime)
                currentEquipment.Attack();
        }

        // Let player attack after attack cooldown time has passed
        if (isCoolingDown)
            if (Time.time >= nextHitTime)
            {
                isCoolingDown = false;
                AnimationLogic(false);
            }
        
    }

    void AnimationLogic(bool setTrigger)
    {
        if (currentEquipment == null)
            return;

        if(anim == null)
            anim = currentEquipment.GetComponent<Animator>();

        if (anim != null)
        {
            if (setTrigger)
                anim.SetTrigger("attack");
            else
                anim.ResetTrigger("attack");
        }
    }
}
