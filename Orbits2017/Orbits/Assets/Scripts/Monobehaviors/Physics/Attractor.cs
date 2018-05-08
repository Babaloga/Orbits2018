using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour{

    [HideInInspector]
    public float mass = 1f;

    public float baseMass = 1f;
    public bool scaleMass = true;

    private void Start()
    {
        if (scaleMass)
        {
            mass = baseMass * (transform.localScale.x + transform.localScale.y) / 2;
        }
        else
        {
            mass = baseMass;
        }
    }
}
