using UnityEngine;
using System.Collections;

public class AsteroidBehavior : MonoBehaviour {
    private Rigidbody2D spaceRock;
    private Vector2 position;
    private float facing;

    public GameObject death;
    public Vector2 velocity;
    public CircleCollider2D missCollider;

    void Start()
    {
        missCollider = GetComponent<CircleCollider2D>();
        missCollider.enabled = false;
        spaceRock = GetComponent<Rigidbody2D>();
        death = Resources.Load("Destroyed") as GameObject;
        StartCoroutine("CollisionBuffer");
        spaceRock.velocity = velocity;
    }

    IEnumerator CollisionBuffer()
    {
        yield return new WaitForSeconds(0.5f);
        missCollider.enabled = true;
    }


    void FixedUpdate()
    {

        position = this.transform.position;
        spaceRock.AddForce(GravityHandler.SumFieldStrength(position) * spaceRock.mass);

        //rotation
        velocity = spaceRock.velocity;
        if (spaceRock.velocity.x > 0)
        {
            facing = (Mathf.Atan(spaceRock.velocity.y / spaceRock.velocity.x)) * Mathf.Rad2Deg - 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else if (spaceRock.velocity.x < 0)
        {
            facing = (Mathf.Atan(spaceRock.velocity.y / spaceRock.velocity.x)) * Mathf.Rad2Deg + 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else
            facing = 0;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Object.Instantiate(death, position, new Quaternion(0, 0, 0, 0));
        Object.Destroy(gameObject);
    }
}