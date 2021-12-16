using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockGolemAnimationScript : MonoBehaviour
{
    AI_Golem golem;
    NavMeshAgent agent;
    Animator anim;

    [SerializeField]
    GameObject projectileArmMesh;
    [SerializeField]
    Collider projectileArmCollider;

    [SerializeField]
    Collider slamArmCollider;

    [SerializeField]
    Transform skeletonBase;

    [SerializeField]
    ParticleSystem rockParticles;

    [SerializeField]
    GameObject ragdoll;

    void Start()
    {
        anim = GetComponent<Animator>();

        golem.StopArmCollisionWithPlayer(slamArmCollider);
    }

    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
    }
    
    public void InitializeVariables(AI_Golem _golem, NavMeshAgent _agent)
    {
        golem = _golem;
        agent = _agent;
    }

    public void WakeGolem()
    {
        anim.SetBool("sleeping", false);
    }

    /// <summary>
    /// Has event to call slam attack when animation hits time stamp
    /// </summary>
    public void PlaySlamAttackAnimation()
    {
        anim.SetTrigger("slam");
    }

    // Gets called by slam animation event
    public void CallSlamAttack()
    {
        golem.SlamAttack(slamArmCollider);
        anim.ResetTrigger("slam");
    }

    /// <summary>
    /// Has event to call rock throw attack when animation hits time stamp
    /// </summary>
    public void PlayRockThrowAnimation()
    {
        anim.SetTrigger("throw");
    }

    // Gets called by rock throw animation event
    public void CallRockThrowAttack()
    {
        // Make arm invisible and 
        int childCount = projectileArmCollider.transform.childCount;
        for(int i = 0; i < childCount; i++)
            Destroy(projectileArmCollider.transform.GetChild(i).gameObject);

        projectileArmCollider.enabled = false;
        projectileArmMesh.SetActive(false);
        
        golem.RockThrowAttack();
        anim.ResetTrigger("throw");
    }

    /// <summary>
    /// Reactivates arm when golem "grabs" it from the ground
    /// </summary>
    public void GetNewArm()
    {
        // Make arm invisible
        projectileArmCollider.enabled = true;
        projectileArmMesh.SetActive(true);
    }

    /// <summary>
    /// Spawn rock burst particlesW
    /// </summary>
    public void CallRockBurst()
    {
        rockParticles.Play();
    }

    /// <summary>
    /// Causes golem to rag doll
    /// </summary>
    public void Death()
    {
        // Spawn ragdoll, then destroy it after slight delay
        GameObject spawnedRagdoll = Instantiate(ragdoll, transform.position, transform.rotation);
        float ragdollDespawnDelay = 10f;
        Destroy(spawnedRagdoll, ragdollDespawnDelay);

        anim.SetBool("sleeping", true);
    }



}
