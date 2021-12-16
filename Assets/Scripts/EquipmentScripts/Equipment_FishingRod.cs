using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_FishingRod : Equipment
{
    bool isCasted = false;
    
    [SerializeField]
    float castPower = 1000f;

    [SerializeField]
    Transform bobberSpawn;
    [SerializeField]
    GameObject bobber;

    GameObject currentBobber = null;

    LineRenderer lr;

    [SerializeField]
    Transform tip;

    // TODO make this list that will randomly choose certain fish
    [SerializeField]
    GameObject fishObject;


    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (isCasted)
            UpdateLineRenderer();
        else
            lr.enabled = false;
    }

    public override void Attack()
    {
        if(isCasted)
        {
            Uncast();
        }
        else
        {
            Cast();
        }

        isCasted = !isCasted;
        print("Fishing rod hit");
    }

    void Cast()
    {
        currentBobber = Instantiate(bobber, bobberSpawn.position, bobberSpawn.rotation);

        // Apply cast force to bobber
        Vector3 dir = cam.transform.up/2 + cam.transform.forward;
        currentBobber.GetComponent<Rigidbody>().AddForce(dir * castPower, ForceMode.Impulse);
    }

    void Uncast()
    {
        // If player has fish then launch fish towards player
        if(currentBobber.GetComponent<BobberScript>().hasFish)
        {
            Transform bobberTransform = currentBobber.transform;
            Rigidbody fishRb = Instantiate(fishObject, bobberTransform.position, bobberTransform.rotation).GetComponent<Rigidbody>();

            float launchSpeed = 40;
            fts.solve_ballistic_arc(fishRb.position,
                                    launchSpeed,
                                    transform.position,
                                    Mathf.Abs(Physics.gravity.y),
                                    out Vector3 lowAngleSolution,
                                    out Vector3 highAngleSolution);

            fishRb.velocity = lowAngleSolution;

            // Launch fish at player
            //float launchHeightMax = 10f;
            //fishRb.velocity = PhysicsEquations.CalculateLaunchVelocity(fishRb, transform, launchHeightMax);
        }

        // Destroy reference to bobber and bobbers gameobject
        Destroy(currentBobber);
        currentBobber = null;
    }

    /// <summary>
    /// Updates fishing lines end to be at the current bobbers position
    /// </summary>
    void UpdateLineRenderer()
    {
        lr.enabled = true;

        // make array of rods tip position and current bobbers position
        Vector3[] positions = new Vector3[] { tip.position, currentBobber.transform.position };
        lr.SetPositions(positions);
    }

    
}
