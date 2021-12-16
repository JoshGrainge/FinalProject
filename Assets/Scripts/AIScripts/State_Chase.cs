using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Chase : State
{
    Vector3? lastKnownLocation = null;

    LayerMask layers;

    public State_Chase(AI _npcScript, NavMeshAgent _agent, Transform _player)
                : base(_npcScript, _agent, _player)
    {
        name = STATE.CHASE;
        agent.isStopped = false;

    }

    public override void Enter()
    {
        // Set speed to running
        agent.speed = npcScript.runSpeed;
        base.Enter();
    }

    public override void Update()
    {
        bool seePlayer = npcScript.IsPlayerInViewCone() && npcScript.IsPlayerInLineOfSight();

        if (!seePlayer && lastKnownLocation == null)
            lastKnownLocation = player.position;

        // Goto players position if NPC sees player else goto last known location
        if (seePlayer)
        {
            agent.SetDestination(player.position);
            lastKnownLocation = null; // Nullify last known location once player is see again
        }
        else if (lastKnownLocation != null)
            agent.SetDestination(lastKnownLocation.Value);

        // If agent reaches last known location set it to null
        if (lastKnownLocation != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            lastKnownLocation = null;
        }

        if (agent.hasPath)
        {
            // If in attack range goto attack state
            if (npcScript.CanAttackPlayer())
            {
                nextState = new State_Attack(npcScript, agent, player);
                stage = EVENT.EXIT;
            }
            // Once NPC cant see player and has gone to last location return to wander state
            else if (!seePlayer && lastKnownLocation == null)
            {
                nextState = new State_Wander(npcScript, agent, player);
                stage = EVENT.EXIT;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
