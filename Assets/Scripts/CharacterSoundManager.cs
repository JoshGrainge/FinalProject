using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip wakeSound, hitSound, deathSound;

    // TODO when continuing this project, level sound clips so that you do not have to do this!!
    // This is for volume control of hit volume, best (and fastest) option of bad options
    [SerializeField]
    float wakeVolume = 1, hitVolume = 1, deathVolume = 1;

    /// <summary>
    /// Subscribe to OnDamage delegate
    /// </summary>
    private void Start()
    {
        HealthScript healthScript = GetComponent<HealthScript>();
        healthScript.OnDamage += OnRecieveDamage;
    }

    /// <summary>
    /// Subscription to event of character taking damage
    /// </summary>
    /// <param name="damage">The parameter is irrelevant, just to appease delegate</param>
    void OnRecieveDamage(float damage)
    {
        PlayHitSound();
    }

    public void PlayWakeSound()
    {
        AudioSource.PlayClipAtPoint(wakeSound, transform.position, wakeVolume);
    }

    public void PlayHitSound()
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position, hitVolume);
    }

    public void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume);
    }

}
