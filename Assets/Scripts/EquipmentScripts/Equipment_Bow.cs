using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Bow : Equipment
{

    [Header("Bow variables")]
    [SerializeField]
    GameObject arrowPrefab;

    [SerializeField]
    GameObject arrowInString;

    [SerializeField]
    Transform arrowSpawn;

    [SerializeField]
    float arrowForceMin = 50f,arrowForceMax = 300f;

    [SerializeField]
    LineRenderer lr;

    [SerializeField]
    Transform topPosition, pullPosition, bottomPosition;

    float pullStarX;
    float pullEndX = -0.37f;

    [SerializeField]
    float fullChargeTime = 1.5f;

    float timeElapsed = 0;

    Inventory inventory;

    bool hasArrowInInventory;

    AudioSource drawAudioSource;

    [SerializeField]
    AudioClip drawSound, drawReleaseSound, arrowShootSound;
    bool startedPlayingDrawSound = false;

    private void Awake()
    {
        pullStarX = pullPosition.localPosition.x;

        cam = Camera.main;

        inventory = GameObject.FindObjectOfType<Inventory>();
        drawAudioSource = GetComponentInChildren<AudioSource>();

        UpdateLineRenderer();
    }

    private void Update()
    {
        // Check if player has arrow in inventory
        hasArrowInInventory = inventory.CheckForItem("Arrow") != null;

        arrowInString.SetActive(hasArrowInInventory);

        // Update the draw string and play bow sounds when attack button is down and inventory is not open
        if (Input.GetButton("Attack") && !inventory.inventoryOpen)
        {
            // Start playing draw string sound when initially pulling bowstring back
            if (!drawAudioSource.isPlaying && !startedPlayingDrawSound)
            {
                startedPlayingDrawSound = true;
                drawAudioSource.clip = drawSound;
                drawAudioSource.Play();
            }

            float newX = Mathf.Lerp(pullStarX, pullEndX, timeElapsed / fullChargeTime);

            Vector3 newPos = pullPosition.localPosition;
            newPos.x = newX;

            pullPosition.localPosition = newPos;

            timeElapsed += Time.deltaTime;
        }
        else
        {
            Vector3 newPos = pullPosition.localPosition;
            newPos.x = pullStarX;

            pullPosition.localPosition = newPos;
        }

        // When releasing attack button or inventory is opened and player is drawing an arrow then reset bow draw
        if (Input.GetButtonUp("Attack") || inventory.inventoryOpen && startedPlayingDrawSound)
        {
            timeElapsed = 0;

            // Play bowstring release sound
            float drawReleaseVolume = 0.8f;
            AudioSource.PlayClipAtPoint(drawReleaseSound, pullPosition.position, drawReleaseVolume);
            // Stop drawing sound
            drawAudioSource.Stop();
            startedPlayingDrawSound = false;

        }


    }

    private void LateUpdate()
    {
        UpdateLineRenderer();
    }

    public override void Attack()
    {
        // When there is no arrow in inventory don't execute attack
        if (!hasArrowInInventory)
            return;

        float arrowShootVolume = 1f;
        AudioSource.PlayClipAtPoint(arrowShootSound, transform.position, arrowShootVolume);

        // Remove an arrow
        inventory.RemoveItem("Arrow");

        print($"Pull percentage is {timeElapsed / fullChargeTime}");

        float arrowForce = Mathf.Lerp(arrowForceMin, arrowForceMax, timeElapsed / fullChargeTime);

        Rigidbody arrowRb = Instantiate(arrowPrefab, cam.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        arrowRb.AddForce(cam.transform.forward * arrowForce, ForceMode.Impulse);

        // Set arrows damage
        arrowPrefab.GetComponent<ArrowScript>().damage = damage;
    }

    void UpdateLineRenderer()
    {
        Vector3[] positions = new Vector3[] { topPosition.position, pullPosition.position, bottomPosition.position};
        lr.SetPositions(positions);
    }

   
}
