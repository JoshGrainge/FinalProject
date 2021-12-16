using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(HealthScript))]
public abstract class AI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    public State currentState;

    [SerializeField]
    LayerMask visionBlockingLayers;

    [Header("Speed variables")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float turnSpeed = 1f;

    protected float visionDist = 20f;
    protected float visionHalfAngle = 55f;

    [Header("Attack variables")]
    public float attackSpeed = 1f;
    [SerializeField]
    protected float attackRange = 3f;
    [SerializeField]
    protected float damage = 20f;
    [SerializeField]
    protected float knockbackForce = 5f;

    protected HealthScript healthScript;

    [Header("DebugSettings")]
    [SerializeField]
    bool printStatus = false;

    protected CharacterSoundManager characterSoundManager;


    /// <summary>
    /// Give ai components
    /// </summary>
    protected virtual void Awake()
    {
        healthScript = GetComponent<HealthScript>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        characterSoundManager = GetComponent<CharacterSoundManager>();
    }

    /// <summary>
    /// Reset state to wander when AI gets respawned into world
    /// </summary>
    protected virtual void OnEnable()
    {
        // Start AI wandering
        currentState = new State_Wander(this, agent, player);
    }

    /// <summary>
    /// Plays death sound when character has no health and is being disabled
    /// </summary>
    protected virtual void OnDisable()
    {
        if (healthScript.health <= 0)
            characterSoundManager.PlayDeathSound();
    }

    protected virtual void Update()
    {
        // Stop updating when AI is dead
        if (healthScript.health <= 0 || !gameObject.activeInHierarchy)
        {
            agent.Stop();
            return;
        }

        // Process current state
        if (printStatus)
            print("current state: " + currentState.name);

        currentState = currentState.Process();
    }

    /// <summary>
    /// Check if player is visible by characters vision cone
    /// </summary>
    /// <param name="objectToSee"></param>
    /// <returns>whether player is visible</returns>
    public bool IsPlayerInViewCone()
    {
        Vector3 dir = player.position - transform.position;
        float angle = Vector3.Angle(dir, transform.forward);
        if (dir.magnitude <= visionDist && angle <= visionHalfAngle)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Check if player is in direct vision of ai, i.e there are no blocking volumes
    /// </summary>
    /// <returns>bool value of if player is in direct line of sight</returns>
    public bool IsPlayerInLineOfSight()
    {
        return !Physics.Linecast(transform.position, player.position, visionBlockingLayers);
    }

    /// <summary>
    /// Check if character can attack player
    /// </summary>
    /// <returns>if player is within attack distance</returns>
    public abstract bool CanAttackPlayer();

    /// <summary>
    /// Characters custom attack logic
    /// </summary>
    public abstract void AttackLogic();
}
