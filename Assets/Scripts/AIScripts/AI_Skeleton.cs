using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Skeleton : AI
{
    [SerializeField]
    GameObject arrow;

    [SerializeField]
    Transform arrowSpawn;

    [SerializeField]
    float arrowForce = 300;


    // Shoots arrow at player
    public override void AttackLogic()
    {
        Rigidbody arrowRb = Instantiate(arrow, arrowSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

        // This is the only way to create a new transform
        GameObject target = new GameObject();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        // Get projected player position as target
        target.transform.position = player.position;
        Vector3 playerVel = playerRb.velocity;
        playerVel.y = 0;    // Zero out upward velocity so that jumping doesn't cause unexpected tracking
        target.transform.position += (playerVel * playerRb.velocity.magnitude);

        // Aim arrow at players projected position then add forward force
        arrowRb.transform.LookAt(target.transform, arrowRb.transform.up);
        arrowRb.AddForce(arrowRb.transform.forward * arrowForce, ForceMode.Impulse);

        // Set arrow damage
        arrowRb.GetComponent<ArrowScript>().damage = damage;

        // Destroy target game object after use
        Destroy(target);
    }

    
    // TODO make this a charge attack instead of instance
    // Skeleton must be within attack range and in direct line of sight of player before shooting arrows
    public override bool CanAttackPlayer()
    {
        // Check if player is in attack range
        Vector3 dir = player.position - transform.position;
        if (dir.magnitude < attackRange)
        {
            // Then check if player is in direct line of sight, we do this second because raycasts are more expensive
            if(IsPlayerInLineOfSight())
            {
                // Player can attack
                return true;
            }
        }

        return false;
    }


}
