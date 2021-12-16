using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectileScript : MonoBehaviour
{
    public float damage;
    public float knockbackForce;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<HealthScript>().DealDamage(damage);
            collision.rigidbody.AddForce(transform.forward * knockbackForce, ForceMode.Impulse);


            //DO DAMAGE
        }


        // TODO spawn debris
        //Destroy on hit and spawn debris
        Destroy(gameObject);
    }
}
