using UnityEngine;
using UnityEngine.AI;

public class State_Idle : State
{
    float idleTime = 0;
    float elapsedTime = 0;


    // Continuous idle state, until interrupted
    public State_Idle(AI _npcScript, NavMeshAgent _agent, Transform _player)
                : base(_npcScript, _agent, _player)
    {
        name = STATE.IDLE;
    }

    // Makes idle state for predetermined time
    public State_Idle(AI _npc, NavMeshAgent _agent, Transform _player, float _idleTime)
                : base(_npc, _agent, _player)
    {
        name = STATE.IDLE;

        idleTime = _idleTime;
    }

    public override void Enter()
    {
        UpdateIsStopped(true);

        base.Enter();
    }

    public override void Update()
    {

        // If nothing is blocking vision then start chasing player
        if (npcScript.IsPlayerInViewCone() && npcScript.IsPlayerInLineOfSight())
        {
            nextState = new State_Chase(npcScript, agent, player);
            stage = EVENT.EXIT;
        }


        // If NPC has not seen player and the idle time has been reached then 
        if (elapsedTime >= idleTime)
        {
            nextState = new State_Wander(npcScript, agent, player);
            stage = EVENT.EXIT;
        }

        // Increment elapsed time 
        elapsedTime += Time.deltaTime;
    }
    public override void Exit()
    {
        // Return freedom to move to NPC
        UpdateIsStopped(false);
        base.Exit();
    }

    void UpdateIsStopped(bool newIsStopped)
    {
        agent.isStopped = newIsStopped;
    }
}
