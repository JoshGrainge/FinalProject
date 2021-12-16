using UnityEngine;
using UnityEngine.AI;

// This class was modeled off of the tutorial series Penny de Byl's FSM model that can be found at: https://learn.unity.com/project/finite-state-machines-1?courseId=5dd851beedbc2a1bf7b72bed

public enum STATE { IDLE, WANDER, CHASE, ATTACK, DEAD };

public class State
{
    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    protected AI npcScript;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    public State(AI _npcScript, NavMeshAgent _agent, Transform _player)
    {
        npcScript = _npcScript;
        agent = _agent;
        player = _player;

        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    // Processes which event should be fired off and either returns next state if should transition states or return the same on continuously until there should be a transition
    public State Process()
    {
        if (stage == EVENT.ENTER)
            Enter();
        if (stage == EVENT.UPDATE)
            Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        else
            return this;
    }

    

}
