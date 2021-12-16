using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMaterialSoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip woodHitSound, rockHitSound, fleshHit;

    /// <summary>
    /// Get audio clip based on hit object tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>Audio clip that is the proper sound for the surface that was hit</returns>
    public AudioClip GetHitSound(string tag)
    {
        switch(tag)
        {
            case "Wood":
                return woodHitSound;
            case "Rock":
                return rockHitSound;
            case "Flesh":
                return fleshHit;
            default:
                return null;
        }

    }
}
