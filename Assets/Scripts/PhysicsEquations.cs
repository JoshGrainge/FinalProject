using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// slightly modified version of Sebastian Lague's solution found here: https://www.youtube.com/watch?v=IvT8hjy6q4o&list=PLFt_AvWsXl0eMryeweK7gc9T04lJCIg_W&index=3

public static class PhysicsEquations
{

    /// <summary>
    /// Find velocity to launch object at target
    /// </summary>
    /// <param name="launchObject"></param>
    /// <param name="target"></param>
    /// <returns>Launch vector towards target</returns>
    public static Vector3 CalculateLaunchVelocity(Rigidbody launchObject, Transform target, float heightMax)
    {
        // Get highest height between target and object, then add peak height value
        float h = Mathf.Max(target.position.y, launchObject.position.y);
        h += heightMax;
        float gravity = Physics.gravity.y;

        float displacementY = target.position.y - launchObject.position.y;

        Vector3 displacementXZ = new Vector3(target.position.x - launchObject.position.x, 0, target.position.z - launchObject.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        Vector3 returnVector = new Vector3(velocityXZ.x, velocityY.y, velocityXZ.z);
        return returnVector;
    }
}
