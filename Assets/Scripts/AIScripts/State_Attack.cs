using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Attack : State
{

    float attackCooldown;
    float elapsedTime = 0;

    public State_Attack(AI _npcScript, NavMeshAgent _agent, Transform _player)
                : base(_npcScript, _agent, _player)
    {
        name = STATE.ATTACK;
        attackCooldown = npcScript.attackSpeed;
    }

    public override void Enter()
    {

        base.Enter();

        // Execute enemy specific attack
        npcScript.AttackLogic();
    }

    public override void Update()
    {
        // Make npc look at player over time
        Vector3 lookVector = player.transform.position - npcScript.transform.position;
        lookVector.y = player.transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        float t = npcScript.turnSpeed * Time.deltaTime;
        npcScript.transform.rotation = Quaternion.Slerp(npcScript.transform.rotation, rot, t);

        // Stop AI to swipe attack at player
        agent.isStopped = true;

        // If still in attack state and attack cooldown has finished execute hit again
        if (elapsedTime >= attackCooldown)
        {
            // If AI can attack player still create new attack state
            if (npcScript.CanAttackPlayer())
            {
                nextState = new State_Attack(npcScript, agent, player);
                stage = EVENT.EXIT;
            }
            // else return to chase state
            else
            {
                nextState = new State_Chase(npcScript, agent, player);
                stage = EVENT.EXIT;
            }
        }

        elapsedTime += Time.deltaTime;
    }
    public override void Exit()
    {
        // Return movement after attack is finished
        agent.isStopped = false;
        base.Exit();
    }
}
