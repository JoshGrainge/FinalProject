using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{

    private static WorldItemDatabase _instance;
    public static WorldItemDatabase Instance { get { return _instance; } }

    public List<WorldItemScript> worldItems = new List<WorldItemScript>();

    // Assign static instance variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public WorldItemScript GetWorldItem(string itemName)
    {
        return worldItems.Find(item => item.itemName == itemName);
    }
}
