using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    Transform tip;

    [SerializeField]
    LayerMask layers;

    public float damage;

    float despawnTimer = 15f;

    [SerializeField]
    AudioClip impactSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Destroy game object after certain time
        Destroy(gameObject, despawnTimer);

    }

    // Fixed update happens every physics step
    void FixedUpdate()
    {
        // Make arrow
        transform.forward = rb.velocity;

        // cast ray the amount arrow will travel between frames to check if arrow will hit object, this is to account for in between frame collision that will be lost
        RaycastHit hit;
        Vector3 startPos = tip.position;
        Vector3 endPos = tip.forward * rb.velocity.magnitude * Time.deltaTime;

        if (Physics.Linecast(startPos, tip.position + endPos, out hit, layers, QueryTriggerInteraction.Ignore))
        {
            // If the root of the hit component has health script, then deal damage
            hit.transform.root.GetComponent<HealthScript>()?.DealDamage(damage);

            // stick arrow to object when object has been hit
            transform.position = hit.point - endPos;    // This sets arrow to be at hit point with tip offset
            transform.SetParent(hit.collider.transform, true);

            // Play impact sound
            float impactSoundVolume = 1;
            AudioSource.PlayClipAtPoint(impactSound, tip.transform.position,impactSoundVolume);

            // Destroy unneeded components
            Destroy(rb);
            Destroy(this);
        }

        
    }

}
