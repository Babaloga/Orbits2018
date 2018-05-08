using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFX : MonoBehaviour
{
    public ParticleSystem thrust1;
    public ParticleSystem thrust2;

    ParticleSystem pSys;
    ShipBehavior ship;

    // Use this for initialization
    void Start()
    {
        pSys = GetComponent<ParticleSystem>();
        ship = transform.parent.GetComponent<ShipBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ship.health == 2)
        {
            if(!thrust1.isPlaying) thrust1.Play();
            if (!thrust2.isPlaying) thrust2.Play();
        }
        else if (ship.health == 1)
        {
            if (!pSys.isPlaying) pSys.Play();
            if (thrust1.isPlaying) thrust1.Stop();
            if (thrust2.isPlaying) thrust2.Stop();

        }
    }
}
