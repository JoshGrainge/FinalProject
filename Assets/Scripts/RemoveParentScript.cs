using UnityEngine;

public class RemoveParentScript : MonoBehaviour
{
    // Remove parent at start
    void Start()
    {
        transform.SetParent(null);
    }
}
