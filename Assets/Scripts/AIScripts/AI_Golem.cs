using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Golem : AI
{
    [SerializeField]
    GameObject rock;
    [SerializeField]
    Transform rockSpawn;
    [SerializeField]
    float rockThrowForce;

    [SerializeField]
    float closeAttackDistanceThreshold;

    [SerializeField]
    RockGolemAnimationScript animationScript;

    bool sleeping = true;

    bool playedWakeSound = false;

    protected override void Awake()
    {
        base.Awake();

        animationScript.InitializeVariables(this, agent);
    }

    /// <summary>
    /// Initialize variables when golem is disabled in game world
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();

        // Return sleeping state for despawner to work properly
        healthScript.health = healthScript.maxHealth;
        sleeping = true;
        playedWakeSound = false;
    }

    protected override void Update()
    {
        // Destroy rock golem after death
        if (healthScript.health <= 0)
        {
            animationScript.Death();
        }

        // Wake up golem on hit
        if (sleeping)
        {
            if (healthScript.health != healthScript.maxHealth)
            {
                animationScript.WakeGolem();
                float wakeDelay = 1f;
                Invoke("WakeGolem", wakeDelay);
            }
            else
                return;
        }

        base.Update();



    }

    /// <summary>
    /// Sets bool after delay
    /// </summary>
    void WakeGolem()
    {
        // Play golem wake sound
        if (!playedWakeSound)
        {
            playedWakeSound = true;
            characterSoundManager.PlayWakeSound();
        }

        sleeping = false;
    }

    /// <summary>
    /// Stops slam arm from colliding with player, this is because it was pushing player underneath the world
    /// </summary>
    /// <param name="slamArmCollider"></param>
    public void StopArmCollisionWithPlayer(Collider slamArmCollider)
    {
        Collider playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        Physics.IgnoreCollision(playerCol, slamArmCollider);
    }

    public override void AttackLogic()
    {
        // get distance to player
        float distance = Vector3.Distance(transform.position, player.position);

        // Check if golem should use long or close range attack, then execute attack
        if (distance <= closeAttackDistanceThreshold)
            animationScript.PlaySlamAttackAnimation();
        else
            animationScript.PlayRockThrowAnimation();
    }

    public override bool CanAttackPlayer()
    {
        // Check if player is in attack range
        Vector3 dir = player.position - transform.position;
        if (dir.magnitude < attackRange)
        {
            // Then check if player is in direct line of sight, we do this second because raycasts are more expensive
            if (IsPlayerInLineOfSight())
            {
                // Player can attack
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Executes close range attacks damage check logic
    /// </summary>
    /// <param name="slamArmCollider"></param>
    public void SlamAttack(Collider slamArmCollider)
    {
        float hitDistanceThreshold = 2;
        float dist = Vector3.Distance(slamArmCollider.bounds.center, player.position);

        // When player is within hit distance threshold do damage
        if (dist <= hitDistanceThreshold)
        {
            // Deal damage
            player.GetComponent<HealthScript>().DealDamage(damage);

            // Knock player back
            Vector3 dir = player.position - slamArmCollider.transform.position;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            playerRb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Golem rock attack
    /// </summary>
    public void RockThrowAttack()
    {
        Rigidbody rockRb = Instantiate(rock, rockSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

        // Get projected player position as target
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        Vector3 playerVel = playerRb.velocity;
        playerVel.y = 0;    // Zero out upward velocity so that jumping doesn't cause unexpected tracking
        Vector3 targetPos = player.position + (playerVel * playerRb.velocity.magnitude);

        // Find low and high angles then set rocks velocity to angle solution
        fts.solve_ballistic_arc(rockSpawn.position,
                                rockThrowForce,
                                targetPos,
                                Mathf.Abs(Physics.gravity.y),
                                out Vector3 lowAngleSolution,
                                out Vector3 highAngleSolution);

        rockRb.velocity = lowAngleSolution;

        // Set rock projectile variables
        RockProjectileScript rockProjectile = rockRb.GetComponent<RockProjectileScript>();
        rockProjectile.damage = damage;
        rockProjectile.knockbackForce = knockbackForce;
    }
}
