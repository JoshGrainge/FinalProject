using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { Skeleton, Goblin, Golem };


public class EnemyManager : MonoBehaviour
{
    Transform player;
    
    Queue<GameObject> skeletons = new Queue<GameObject>();
    Queue<GameObject> goblins = new Queue<GameObject>();
    Queue<GameObject> golems = new Queue<GameObject>();

    float skeletonSpawnChance = 0.20f;
    int currentSkeletonsSpawned = 0;
    int skeletonSpawnCap = 5;

    float goblinSpawnChance = 0.30f;
    int currentGoblinsSpawned = 0;
    int goblinSpawnCap = 8;

    float golemSpawnChance = 0.08f;
    int currentGolemSpawned = 0;
    int golemsSpawnCap = 2;

    [SerializeField]
    GameObject skeletonPrefab;
    [SerializeField]
    GameObject goblinPrefab;
    [SerializeField]
    GameObject golemPrefab;

    Vector3 skeletonSpawnOffset = new Vector3(0, 1, 0);
    Vector3 goblinSpawnOffset = new Vector3(0, 1, 0);

    [SerializeField]
    Transform initialSpawn;   // This is a position under the map that will make the player not see the enemies spawn

    [SerializeField]
    Transform rayStart;

    [SerializeField]
    LayerMask groundCheckLayers;
    float minSpawnRadius = 40, maxSpawnRadius = 128;

    /// <summary>
    /// Initialize pool and player variable, then starts spawning algorithm
    /// </summary>
    private void Awake()
    {
        // Spawn pool of enemies 
        for (int i = 0; i < skeletonSpawnCap; i++)
        {
            CreateEnemy(EnemyTypes.Skeleton, initialSpawn.position, Quaternion.identity);
        }
        for (int i = 0; i < goblinSpawnCap; i++)
        {
            CreateEnemy(EnemyTypes.Goblin, initialSpawn.position, Quaternion.identity);
        }
        for (int i = 0; i < golemsSpawnCap; i++)
        {
            CreateEnemy(EnemyTypes.Golem, initialSpawn.position, Quaternion.identity);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnCheck());
    }


    /// <summary>
    /// Continuously spawns enemies around player
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCheck()
    {
        while(true)
        {
            float currentChance = skeletonSpawnChance;
            float randomValue = Random.value;
            if(randomValue <= currentChance && currentSkeletonsSpawned < skeletonSpawnCap)
            {
                // Spawn skeleton
                currentSkeletonsSpawned++;
                SpawnEnemy(EnemyTypes.Skeleton, RandomPosition(), Quaternion.identity);

            }

            currentChance += goblinSpawnChance;
            if(randomValue <= currentChance && currentGoblinsSpawned < goblinSpawnCap)
            {
                // Spawn goblin
                currentGoblinsSpawned++;
                SpawnEnemy(EnemyTypes.Goblin, RandomPosition(), Quaternion.identity);
            }

            currentChance += golemSpawnChance;
            if(randomValue <= currentChance && currentGolemSpawned < golemsSpawnCap)
            {
                // Spawn golem
                currentGolemSpawned++;
                SpawnEnemy(EnemyTypes.Golem, RandomPosition(), Quaternion.identity);

            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Spawn initial pool of enemies that will be manipulated in the game world
    /// </summary>
    /// <param name="enemyType"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="spawnRotation"></param>
    void CreateEnemy(EnemyTypes enemyType, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject enemyPrefab = null;
        switch(enemyType)
        {
            case EnemyTypes.Skeleton:
                enemyPrefab = skeletonPrefab;
                break;

            case EnemyTypes.Goblin:
                enemyPrefab = goblinPrefab;
                break;

            case EnemyTypes.Golem:
                enemyPrefab = golemPrefab;
                break;
        }

        GameObject enemyCreated = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
        AddToPool(enemyType, enemyCreated);
    }

    /// <summary>
    /// Places enemy in world, then removes the enemy from that particular enemy's queue
    /// </summary>
    /// <param name="enemyType"></param>
    /// <param name="spawnPos"></param>
    /// <param name="spawnRot"></param>
    void SpawnEnemy(EnemyTypes enemyType, Vector3 spawnPos, Quaternion spawnRot)
    {
        GameObject enemy = null;
        switch(enemyType)
        {
            case EnemyTypes.Skeleton:
                enemy = skeletons.Dequeue();
                MoveEnemyToWorld(enemy, spawnPos + skeletonSpawnOffset, spawnRot);
                break;

            case EnemyTypes.Goblin:
                enemy = goblins.Dequeue();
                MoveEnemyToWorld(enemy, spawnPos + goblinSpawnOffset, spawnRot);
                break;

            case EnemyTypes.Golem:
                enemy = golems.Dequeue();
                MoveEnemyToWorld(enemy, spawnPos, spawnRot);
                break;
        }
    }

    /// <summary>
    /// Manipulate enemy game objects position and rotation
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="spawnPos"></param>
    /// <param name="spawnRot"></param>
    void MoveEnemyToWorld(GameObject enemy, Vector3 spawnPos, Quaternion spawnRot)
    {
        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(spawnPos);

        //enemy.transform.position = spawnPos;
        enemy.transform.rotation = spawnRot;

        enemy.SetActive(true);

    }

    /// <summary>
    /// Removes enemy from game world, and adds enemy game object to pool
    /// </summary>
    /// <param name="enemyType"></param>
    /// <param name="enemy"></param>
    public void AddToPool(EnemyTypes enemyType, GameObject enemy)
    {
        enemy.SetActive(false);

        switch (enemyType)
        {
            case EnemyTypes.Skeleton:
                skeletons.Enqueue(enemy);
                currentSkeletonsSpawned = Mathf.Clamp(currentSkeletonsSpawned -1 ,0, skeletonSpawnCap);
                break;

            case EnemyTypes.Goblin:
                goblins.Enqueue(enemy);
                currentGoblinsSpawned = Mathf.Clamp(currentGoblinsSpawned - 1, 0, goblinSpawnCap);
                break;

            case EnemyTypes.Golem:
                golems.Enqueue(enemy);
                currentGolemSpawned = Mathf.Clamp(currentGolemSpawned - 1, 0, golemsSpawnCap);
                break;
        }
    }

    /// <summary>
    /// Get random position in radius of player
    /// </summary>
    /// <returns></returns>
    Vector3 RandomPosition()
    {
        Vector3 directionVector = Vector3.zero;

        while (directionVector == Vector3.zero)
        {
            directionVector = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        }

        directionVector.Normalize();

        float magnitude = Random.Range(minSpawnRadius, maxSpawnRadius);

        Vector3 spawnVector = directionVector * magnitude;

        int worldMaxHeight = 600;

        Vector3 playerPositionAdditive = player.position;
        playerPositionAdditive.y = 0;

        RaycastHit hit;
        Physics.Raycast(rayStart.position + spawnVector + playerPositionAdditive, Vector3.down, out hit, worldMaxHeight * 2, groundCheckLayers);

        //if (hit.collider != null)
        //    print($"Ground check hit: {hit.collider.name}");

        spawnVector.y = hit.point.y;

        return hit.point;
        //return spawnVector + playerPositionAdditive;
    }


}
