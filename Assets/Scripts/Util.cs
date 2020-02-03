using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /*
     * Converts an x, y tuple into a 3d vector in the xz-plane (zero y component)
     */
    public static Vector3 toVector3(float x, float y)
    {
        return new Vector3(x, 0, y);
    }

    /*
     * Clamps a vector elementwise between min and max
     */
    public static Vector3 clamp(Vector3 vector, float min, float max)
    {
        return new Vector3(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max));
    }

    /*
     * Rounds a vector elementwise to the nearest integer
     */
    public static Vector3 round(Vector3 vector)
    {
        return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    /*
     * Divides two vectors elementwise
     */
    public static Vector3 elementWiseDivide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}
