using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterKnockbackScript : MonoBehaviour
{
    [SerializeField]
    float characterMass;

    Vector3 impact = Vector3.zero;

    CharacterController controller;

    float impactThreshold = 0.2f;

    [SerializeField]
    float impactDecaySpeed = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Move character controller when impact is greater than threshold
        if (impact.magnitude > impactThreshold)
            controller.Move(impact * Time.deltaTime);

        // Decrease impact over time
        impact = Vector3.Lerp(impact, Vector3.zero, impactDecaySpeed * Time.deltaTime);
    }

    /// <summary>
    /// Add force to players character controller
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void Knockback(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) 
            dir.y = -dir.y;

        impact += dir.normalized * force / characterMass;
    }
}
