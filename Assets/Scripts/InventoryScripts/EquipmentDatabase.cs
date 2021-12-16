using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDatabase : MonoBehaviour
{
    private static EquipmentDatabase _instance;
    public static EquipmentDatabase Instance { get { return _instance; } }

    public List<Equipment> equipmentList = new List<Equipment>();

    // Assign static instance variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public Equipment GetEquipment(string equipmentName)
    {
        return equipmentList.Find(equipment => equipment.equipmentName == equipmentName);
    }
}
