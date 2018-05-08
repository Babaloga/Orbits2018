using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Attracted : MonoBehaviour
{
    protected Rigidbody2D rb;
    private Vector2 position;
    public Vector2 velocity = Vector2.zero;
    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        position = transform.position;
        rb.AddForce(GravityHandler.SumFieldStrength(position) * rb.mass);
    }
}
