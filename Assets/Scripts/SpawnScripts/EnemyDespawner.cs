using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawner : MonoBehaviour
{
    [SerializeField]
    EnemyTypes enemyType;

    EnemyManager spawnManager;

    Transform player;

    float despawnChance = 0.247f;
    float despawnChanceDistanceThreshold = 64;
    float despawnGaurenteeDistanceThreshold = 128;

    bool inDespawnRange = false;

    float despawnRangeTimestamp;
    float despawnTimeDelay = 30f;

    HealthScript healthScript;

    // TODO instead of making this destroy the body, make material that fades over time and then despawn instead
    [SerializeField]
    float bodyFadeTimer = 3f;   // This was for death animations timers , but I didn't have time to get to implement them
    bool calledDespawn = false;

    private void Awake()
    {
        spawnManager = FindObjectOfType<EnemyManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthScript = GetComponent<HealthScript>();
    }

    private void Update()
    {
        // When game object is disabled stop logic
        if (!gameObject.activeInHierarchy)
            return;

        // Despawn mob when their health is 0 or below
        if (healthScript.health <= 0 && !calledDespawn)
            StartDespawn(bodyFadeTimer);

        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

        // If player is out of guarantee threshold automatically despawn mob
        if(distanceFromPlayer >= despawnGaurenteeDistanceThreshold)
        {
            StartDespawn(bodyFadeTimer);
        }
        // Check if distance to player is greater than despawn threshold
        else if (distanceFromPlayer >= despawnChanceDistanceThreshold && !inDespawnRange)
        {
            inDespawnRange = true;
            despawnRangeTimestamp = Time.time;
        }
        // in range to keep living
        else
        {
            inDespawnRange = false;
        }

        // Once mob has been out of range for despawn timer delay mob has a slight chance to despawn every second
        if (inDespawnRange)
        {
            if (Time.time >= despawnRangeTimestamp + despawnTimeDelay)
            {
                if (Random.value <= despawnChance * Time.deltaTime)
                {
                    // Despawns with no delay because delay has already happened
                    StartDespawn(0);
                }
            }
        }
    }

    /// <summary>
    /// Calls function to adds mob to spawn manager pool
    /// </summary>
    void StartDespawn(float despawnDelay)
    {
        calledDespawn = true;
        StartCoroutine(DespawnLogic(despawnDelay));
    }

    /// <summary>
    /// Logic to add to spawn manager enemy pool and to restore ai to full health
    /// </summary>
    IEnumerator DespawnLogic(float despawnDelay)
    {
        yield return new WaitForSeconds(despawnDelay);

        spawnManager.AddToPool(enemyType, gameObject);

        // return ai to full health
        healthScript.Heal(healthScript.maxHealth);
        calledDespawn = false;
    }
}
