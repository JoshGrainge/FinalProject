using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Goblin : AI
{
    public override void AttackLogic()
    {
        player.GetComponent<HealthScript>().DealDamage(damage);

        // Knock player back
        Vector3 dir = agent.transform.forward;
        dir += Vector3.up;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        playerRb.AddForce(dir * knockbackForce, ForceMode.Impulse);
    }

    // Goblin only needs to be in range to attack player
    public override bool CanAttackPlayer()
    {
        Vector3 dir = player.position - transform.position;
        if (dir.magnitude < attackRange)
            return true;
        else
            return false;
    }

}
