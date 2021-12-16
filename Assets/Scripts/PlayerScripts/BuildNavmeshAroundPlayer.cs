using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public class BuildNavmeshAroundPlayer : MonoBehaviour
{
    //NavMeshSurface[] surface;

    [SerializeField]
    float radius;

    [SerializeField]
    LayerMask terrainLayers;

    [SerializeField]
    float distanceThreshold = 5f;

    [SerializeField]
    NavMeshSurface[] surfaces;

    Vector3 lastBuildPos;

    void Start()
    {
        UpdateNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, lastBuildPos) >= distanceThreshold)
        {
            UpdateNavMesh();
        }
    }

    void UpdateNavMesh()
    {
        // Clear nav mesh data from all terrain surfaces
        foreach (var surface in surfaces)
        {
            surface.RemoveData();
        }

        // Get terrain close to player, then build nav mesh on terrain around player
        Collider[] overlappingTerrains = Physics.OverlapSphere(transform.position, radius, terrainLayers);
        foreach (var terrain in overlappingTerrains)
        {
            NavMeshSurface surface = terrain.GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }

        lastBuildPos = transform.position;
    }
}
