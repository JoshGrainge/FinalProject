using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    [Header("Equipment variables")]
    public int equipmentId;
    public string equipmentName;
    [SerializeField]
    protected float damage;
    public float attackSpeed;

    public bool hasChargeAttack;
    public float chargeTime;

    protected Camera cam;

    /// <summary>
    /// Execute custom weapon attack
    /// </summary>
    public abstract void Attack();

}
