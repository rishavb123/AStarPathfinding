using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Vector3 velocity;
    public float distance;

    float distanceSoFar;

    /*
     * Called before the first update
     */
    void Start()
    {
        
    }

    /*
     * Moves the game object in the direction of the velocity and turns after it moves a distance
     */
    void Update()
    {
        Vector3 move = velocity * Time.deltaTime;
        transform.Translate(move);
        distanceSoFar += move.magnitude;
        if(distanceSoFar >= distance)
        {
            distanceSoFar = 0;
            velocity *= -1;
        }

    }
}
