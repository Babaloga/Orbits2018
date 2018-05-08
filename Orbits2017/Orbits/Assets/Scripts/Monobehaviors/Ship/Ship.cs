using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(InputManager), typeof(TrajectoryPredict))]
public class Ship : Projectile {

    public int health = 3;

    InputManager manager;
    TrajectoryPredict trajectory;

    public float inputAxis;
    public float accelRate = 10f;

    float accelMagnitude;

    public bool enginesDisabled;

    Vector2 direction;
    Vector2 accelVector;

    public GameObject thrusterBase;
    public GameObject body;
    public AudioSource thrusterBaseAudio;

    public GameObject Left;
    public GameObject Right;
    public GameObject FrontLeft;
    public GameObject FrontRight;

    private Animator[] thrustersFrontLeft;
    private Animator[] thrustersBackLeft;
    private Animator[] thrustersFrontRight;
    private Animator[] thrustersBackRight;

    private bool justAccelerated;

    protected override void Start () {
        base.Start();

        manager = GetComponent<InputManager>();
        trajectory = GetComponent<TrajectoryPredict>();

        //Animation Setup
        thrustersBackRight = Right.GetComponentsInChildren<Animator>();
        thrustersBackLeft = Left.GetComponentsInChildren<Animator>();
        thrustersFrontLeft = FrontLeft.GetComponentsInChildren<Animator>();
        thrustersFrontRight = FrontRight.GetComponentsInChildren<Animator>();
    }
	
	protected override void FixedUpdate () {

        base.FixedUpdate();

        //Player Acceleration Input
        inputAxis = AccelAxis();
        direction = rb.velocity.normalized;
        accelMagnitude = accelRate * inputAxis;
        accelVector = accelMagnitude * direction;
        rb.AddForce(accelVector * rb.mass);

        trajectory.SimulatePath(transform.position, rb.velocity, 200, 2);

        //Animation (consider separating out into another class)
        if (inputAxis > 0)
        {
            if (!thrusterBaseAudio.isPlaying) thrusterBaseAudio.Play();
            justAccelerated = true;

            for (int i = 0; i < thrustersBackLeft.Length; i++)
            {
                thrustersBackLeft[i].SetBool("accelerating", true);
                thrustersBackRight[i].SetBool("accelerating", true);
            }
            for (int i = 0; i < thrustersFrontLeft.Length; i++)
            {
                thrustersFrontLeft[i].SetBool("accelerating", false);
                thrustersFrontRight[i].SetBool("accelerating", false);
            }
        }
        else if (inputAxis < 0)
        {
            if (!thrusterBaseAudio.isPlaying) thrusterBaseAudio.Play();
            justAccelerated = true;

            for (int i = 0; i < thrustersBackLeft.Length; i++)
            {
                thrustersBackLeft[i].SetBool("accelerating", false);
                thrustersBackRight[i].SetBool("accelerating", false);
            }
            for (int i = 0; i < thrustersFrontLeft.Length; i++)
            {
                thrustersFrontLeft[i].SetBool("accelerating", true);
                thrustersFrontRight[i].SetBool("accelerating", true);
            }
        }
        else
        {
            if (justAccelerated == true) thrusterBase.AddComponent<AudioFade>().fullVolume = 0.5f;
            justAccelerated = false;

            for (int i = 0; i < thrustersBackLeft.Length; i++)
            {
                thrustersBackLeft[i].SetBool("accelerating", false);
                thrustersBackRight[i].SetBool("accelerating", false);
            }
            for (int i = 0; i < thrustersFrontLeft.Length; i++)
            {
                thrustersFrontLeft[i].SetBool("accelerating", false);
                thrustersFrontRight[i].SetBool("accelerating", false);
            }
        }
    }

    protected override void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(deathSequence());
        }

        if (colliding)
        {
            health -= 1;
            StartCoroutine(hitSequence());
            if (health == 1)
            {
                body.GetComponent<AudioSource>().clip = Resources.Load("Sounds/ShipHit2Alt") as AudioClip;
            }
            if (health == 0) body.GetComponent<AudioSource>().clip = Resources.Load("Sounds/ShipHitFinal") as AudioClip;
            body.GetComponent<AudioSource>().Play();
            ChromaticAberrationModel.Settings c = new ChromaticAberrationModel.Settings();
            c.intensity = 2;
            Camera.main.GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings = c;
        }
    }

    IEnumerator deathSequence()
    {
        Time.timeScale = Mathf.Epsilon;
        yield return new WaitForSecondsRealtime(0.3f);
        Instantiate(death, transform.position, new Quaternion(0, 0, 0, 0));


        //if (GameObject.FindGameObjectsWithTag("Player").Length == 2)
        //{
        //    foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        //    {
        //        if (g != gameObject)
        //        {
        //            MenuScreen.GetComponent<ExtranousMenu>().EndMenu(g.GetComponent<ColorManager>().team, g.GetComponent<InputManager>().player);
        //        }
        //    }

        //    mainC.gameObject.AddComponent<AudioFade>();
        //}
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    IEnumerator hitSequence()
    {
        Time.timeScale = Mathf.Epsilon;
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1;
    }

    public float AccelAxis()
    {
        if (!enginesDisabled)
            return manager.AccelAxis();
        else
            return 0;
    }
}
