using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollowMouse : MonoBehaviour
{
    // Sets object to follow mouse position on late update so that there is no camera rendering jitter
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
