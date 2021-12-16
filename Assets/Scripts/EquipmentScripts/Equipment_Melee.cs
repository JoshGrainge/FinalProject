using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Melee : Equipment
{
    float attackDistance = 7;

    [SerializeField]
    protected float damageMultipler = 1.5f;

    HitMaterialSoundManager hitSoundManager;

    [SerializeField]
    protected string damageIncreaseTag;

    [SerializeField]
    AudioClip swingSound;

    // Get components
    private void Awake()
    {
        cam = Camera.main;

        hitSoundManager = transform.root.GetComponent<HitMaterialSoundManager>();
    }

    /// <summary>
    /// Swing melee weapon and plays sound
    /// </summary>
    public override void Attack()
    {
        float swingVolume = 0.85f;
        AudioSource.PlayClipAtPoint(swingSound, transform.position, swingVolume);
        Melee(damageIncreaseTag);
    }

    /// <summary>
    /// Deal damage to hit object and does damage calculations
    /// </summary>
    /// <param name="damageIncreaseTag">The tag that is on objects that this equipment deals extra damage to</param>
    protected virtual void Melee(string damageIncreaseTag)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            print($"Hit object: {hit.transform.gameObject.name}");

            HealthScript hitObjectHealth = hit.transform.root.gameObject.GetComponent<HealthScript>();

//             print($"Health script: {hit.transform.gameObject.GetComponent<HealthScript>() != null}");
            if (hitObjectHealth != null)
            {
                // Check if weapon deals extra damage to material type
                float damageMod = hitObjectHealth.CompareTag(damageIncreaseTag) ? damageMultipler : 1;

                hitObjectHealth.DealDamage(damage * damageMod);

                // Get which audio clip should be played based on surface, then play audio at hit position
                AudioClip hitClip = hitSoundManager.GetHitSound(hit.transform.tag);
                if (hitClip != null)
                    AudioSource.PlayClipAtPoint(hitClip, hit.point);

                // Object death logic
                if (hitObjectHealth.health <= 0)
                {
                    // if there is a breakable object script then call break on object
                    hitObjectHealth.transform.GetComponent<BreakableObject>()?.Break();
                }
            }
        }
    }
}
