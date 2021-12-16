using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberScript : MonoBehaviour
{
    public bool hasFish = false;
    float loseFishTimer = 0.5f;

    float minFishBiteTimer = 5f;
    float maxFishBiteTimer = 15f;

    float bobberDropHeight = 0.2f;


    [SerializeField]
    AudioClip bobberLandSound, bobberBiteSound;

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 startPos = transform.position;

        float groundCheckDist = 0.5f;
        RaycastHit hit;
        Ray ray = new Ray(startPos, Vector3.down);

        if (Physics.Raycast(ray, out hit, groundCheckDist))
        {
            // Only start fish sequence when bobber is in water
            if (hit.transform.CompareTag("Water"))
            {
                // Play landing sound when bobber first hits water
                float bobberLandVolume = 1.6f;
                AudioSource.PlayClipAtPoint(bobberLandSound, transform.position, bobberLandVolume);
                StartFishBiteSequence();
            }

            // Freeze object and offset bobber to be half in object it collided with
            GetComponent<Rigidbody>().isKinematic = true;

            // place bobber on ground surface
            transform.position = hit.point;
        }
    }

    /// <summary>
    /// Start sequence to wait for fish to bite
    /// </summary>
    void StartFishBiteSequence()
    {
        StartCoroutine(FishBiteDelay(Random.Range(minFishBiteTimer, maxFishBiteTimer)));
    }

    IEnumerator FishBiteDelay(float biteDelay)
    {
        // After random amount of time fish pulls bobber down
        yield return new WaitForSeconds(biteDelay);
        hasFish = true;

        Vector3 bobberPullOffset =  -transform.up * (bobberDropHeight * 2);
        // Pull bobber under water
        transform.position = transform.position + bobberPullOffset;

        float pullDownVolume = 2;
        // Play pull down sound
        AudioSource.PlayClipAtPoint(bobberBiteSound, transform.position, pullDownVolume);

        yield return new WaitForSeconds(loseFishTimer);

        // Return bobber to original position
        transform.position = transform.position - bobberPullOffset;

        hasFish = false;

        // when fish is lost repeat fish bite sequence
        StartFishBiteSequence();
    }


}
