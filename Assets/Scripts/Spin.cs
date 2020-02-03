using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 omega;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(omega);
    }
}
