using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Wander : State
{
    LayerMask layers;

    bool hasDestination = false;
    Vector3 currentPoint;

    bool isGuard = false;

    public State_Wander(AI _npcScript, NavMeshAgent _agent, Transform _player)
                : base(_npcScript, _agent, _player)
    {
        name = STATE.WANDER;
    }

    public override void Enter()
    {
        // Set speed to walking
        agent.speed = npcScript.walkSpeed;
        base.Enter();
    }
    public override void Update()
    {
        // When player is seen enter chase state
        if(npcScript.IsPlayerInViewCone() && npcScript.IsPlayerInLineOfSight())
        {
            nextState = new State_Chase(npcScript, agent, player);
            stage = EVENT.EXIT;
        }


        // When agent has reached stopping distance set has destination to false
        if (agent.remainingDistance < agent.stoppingDistance && hasDestination)
                hasDestination = false;

        // When there is no destination assign new destination
        if (!hasDestination)
        {
            // Assign new current point randomly from list of points
            currentPoint = GetRandomWaypoint();

            agent.destination = currentPoint;
            hasDestination = true;
        }

    }
    public override void Exit()
    {
        base.Exit();
    }


    /// <summary>
    /// Get random navigable point on navmesh
    /// </summary>
    /// <returns>Random reachable point on navmesh</returns>
    Vector3 GetRandomWaypoint()
    {
        float radius = 25f;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += npcScript.transform.position;
        NavMeshHit hit;
        // Set to npc position so that if there is not a valid point it will loop again and find another
        Vector3 finalPosition = npcScript.transform.position;
        int areaMask = 1;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, areaMask))
            finalPosition = hit.position;

        return finalPosition;
    }


}
