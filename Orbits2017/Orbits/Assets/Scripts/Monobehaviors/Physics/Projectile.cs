using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attracted
{
    protected float facing;

    public string deathObjName = "DestroyedMissile";

    public GameObject death;
    public bool colliding = false;

    protected override void Start()
    {
        base.Start();
        death = Resources.Load(deathObjName) as GameObject;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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

    protected virtual void Update()
    {
        if (colliding)
        {
            Instantiate(death, transform.position, new Quaternion(0, 0, 0, 0));
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("hit");
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        print("unhit");
        colliding = false;
    }
}
