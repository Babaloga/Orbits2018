using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrajectoryPredict))]
public class TurretBehavior : MonoBehaviour {
    public float fireSpeed = 50f;
    public float barrelLength;
    public float coolTime = 1f;

    public bool firingDisabled = false;
    public bool rotationDisabled = false;

    public InputManager manager;

    GameObject projectile;
    AudioSource fireSound;
    TrajectoryPredict turretLineDraw;

    private Vector2 barrelPos;
    private Vector2 fireVelocity;

    public int lineDistanceMax = 200;
    public int lineResMax = 10;

    bool cooling = false;
    bool triggerDown = false;

    float timeFired = 0;

    private void Update()
    {
        //cooldown
        if (cooling && Time.time - timeFired >= coolTime)
            cooling = false;

        //rotation
        if (!rotationDisabled)
        {
            //print(manager.AimDir());
            transform.up = Quaternion.Euler(new Vector3(0, 0, manager.AimDir())) * Vector2.up;
        }

        //firing
        if (!firingDisabled && !cooling)
        {
            if (manager.Fire())
            {
                ProjectileTrajectory();
                triggerDown = true;
            }
            else if (!manager.Fire() && triggerDown == true)
            {
                ProjectileFire();
                triggerDown = false;
                cooling = true;
                timeFired = Time.time;
            }
        }
    }

    public void ProjectileTrajectory () {
        barrelPos = transform.position + (transform.up * barrelLength);
        fireVelocity = (fireSpeed * transform.up) + new Vector3(transform.parent.GetComponent<Ship>().velocity.x, transform.parent.GetComponent<Ship>().velocity.y);
        turretLineDraw = GetComponent<TrajectoryPredict>();

        //float a = Vector3.Angle(transform.up, transform.parent.GetComponent<Rigidbody2D>().velocity) / 180;
        //int lineRes = Mathf.CeilToInt(Mathf.Lerp(lineResMax, 1, a));

        int lineRes = 10;

        turretLineDraw.SimulatePath(barrelPos, fireVelocity, lineDistanceMax, lineRes);
    }
	
	public void ProjectileFire () {
        fireSound = GetComponent<AudioSource>();
        if(fireSound) fireSound.Play();
        if(GetComponent<Animator>()) GetComponent<Animator>().SetTrigger("Fire");
        barrelPos = transform.position + (transform.up * barrelLength);
        fireVelocity = (fireSpeed * transform.up) + new Vector3(transform.parent.GetComponent<Ship>().velocity.x, transform.parent.GetComponent<Ship>().velocity.y);
        ProjectileTrajectory();
        projectile = Resources.Load("Missile") as GameObject;
        projectile = Instantiate(projectile, barrelPos, Quaternion.LookRotation(transform.up, Vector3.up)) as GameObject;
        projectile.layer = gameObject.layer;
        projectile.GetComponent<Projectile>().velocity = fireVelocity;

        StartCoroutine(Camera.main.GetComponent<CameraEffects>().Shake(2f, 0.1f));
    }
}
