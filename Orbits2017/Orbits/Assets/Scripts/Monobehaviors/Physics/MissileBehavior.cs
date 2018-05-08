using UnityEngine;
using System.Collections;

public class MissileBehavior : Attracted {

    //TO BE PHASED OUT, REPLACED WITH Projectile

    protected float facing;

    public string deathObjName = "DestroyedMissile";

    public GameObject death;

    void Start()
    {
        death = Resources.Load(deathObjName) as GameObject;
    }

    void FixedUpdate()
    {
        //rotation
        if (rb.velocity.x > 0)
        {
            facing = (Mathf.Atan(rb.velocity.y / rb.velocity.x)) * Mathf.Rad2Deg - 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else if (rb.velocity.x < 0)
        {
            facing = (Mathf.Atan(rb.velocity.y / rb.velocity.x)) * Mathf.Rad2Deg + 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else
            facing = 0;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Instantiate(death, transform.position, new Quaternion(0, 0, 0, 0));
        Destroy(gameObject);
    }
}