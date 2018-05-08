using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

public class ShipBehavior : MonoBehaviour
{

    public Rigidbody2D shipBod;
    public Vector2 position;
    public Vector2 velocity;
    public float inputAxis;
    public float accelRate = 10f;
    public GameObject turret;
    public GameObject body;
    public LineRenderer aimLine;
    public BeamBehavior beam;
    public int health = 3;
    public bool stunned;
    public bool beingHit;
    public float stunTime = 5;
    public GameObject hitObject;
    public ShipBehavior shipScript;
    public AudioSource pew;
    public SpriteRenderer lasereffect;
    public GameObject death;
    public GameObject MenuScreen;
    public Camera mainC;
    public float deathField = 800;

    private Vector2 direction;
    private float facing;
    private float accelMagnitude;
    private Vector2 acceleration;
    private float timeSinceHit = 900;
    private RaycastHit2D hit;
	private bool justAccelerated;
    private bool triggerPressed;
    private int timeSinceFiring = 900;
    private bool firing;

    public bool disabled;
    public bool cooling;
    public int coolTime = 30;
	public GameObject thrusterBase;
	public AudioSource thrusterBaseAudio;
    public GameObject Left;
    public GameObject Right;
    public GameObject FrontLeft;
    public GameObject FrontRight;
    public Animator[] thrustersFrontLeft;
    public Animator[] thrustersBackLeft;
    public Animator[] thrustersFrontRight;
    public Animator[] thrustersBackRight;

    public float fireSpeed;
    public float fireMass;
    public float barrelLength;

    public InputManager manager;

    void Start()
    {
        shipBod = GetComponent<Rigidbody2D>();
        shipBod.velocity = velocity;
        direction = velocity.normalized;
        thrustersBackRight = Right.GetComponentsInChildren<Animator>();
        thrustersBackLeft = Left.GetComponentsInChildren<Animator>();
        thrustersFrontLeft = FrontLeft.GetComponentsInChildren<Animator>();
        thrustersFrontRight = FrontRight.GetComponentsInChildren<Animator>();
        beingHit = false;
        disabled = false;
        cooling = false;
        firing = false;
        pew = transform.Find("Body").GetComponent<AudioSource>();
        //lasereffect = transform.Find("laserfireblue").GetComponent<SpriteRenderer>();
		thrusterBase = transform.Find ("Body/ThrustersBase").gameObject;
		thrusterBaseAudio = thrusterBase.GetComponent<AudioSource>();
        death = Resources.Load("Destroyed") as GameObject;
        MenuScreen = GameObject.Find("Canvas");
        mainC = Camera.main;

        manager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        //stun and hit manager
        if (beingHit == true)
        {
            timeSinceHit = 0;
            stunned = true;


        }
        else if (beingHit == false && timeSinceHit < stunTime)
        {
            stunned = true;
            timeSinceHit++;
        }
        else stunned = false;

        //turret cooldown
        if (firing)
        {
            timeSinceFiring = 0;
            cooling = true;
        }
        else if (!firing && timeSinceFiring < coolTime)
        {
            cooling = true;
            timeSinceFiring++;
        }
        else cooling = false;

        //Pause menu (give to game manager)
        if (Input.GetButtonDown("Pause"))
        {
            if (!MenuScreen.activeSelf)
            {
                MenuScreen.SetActive(true);
                MenuScreen.GetComponent<ExtranousMenu>().PauseMenu();
                Time.timeScale = Mathf.Epsilon;
                mainC.GetComponent<AudioReverbFilter>().enabled = true;
                mainC.GetComponent<AudioLowPassFilter>().enabled = true;
            }
            else
            {
                Time.timeScale = 1;
                mainC.GetComponent<AudioReverbFilter>().enabled = false;
                mainC.GetComponent<AudioLowPassFilter>().enabled = false;
                MenuScreen.SetActive(false);
            }
        }

        //Primary fire
        if (stunned == false && cooling == false) disabled = false;
        else disabled = true;

        //hit = beam.BeamFire(PrimaryFire(usingGamepad), !disabled, pew);
        

        //Player Acceleration Input
        inputAxis = AccelAxis();
        position = transform.position;
        shipBod.AddForce(GravityHandler.SumFieldStrength(position) * shipBod.mass);
        direction = velocity.normalized;
        accelMagnitude = accelRate * inputAxis;
        acceleration = accelMagnitude * direction;
        shipBod.AddForce(acceleration * shipBod.mass);

        //rotation
        velocity = shipBod.velocity;
        if (shipBod.velocity.x > 0)
        {
            facing = (Mathf.Atan(shipBod.velocity.y / shipBod.velocity.x)) * Mathf.Rad2Deg - 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else if (shipBod.velocity.x < 0)
        {
            facing = (Mathf.Atan(shipBod.velocity.y / shipBod.velocity.x)) * Mathf.Rad2Deg + 90f;
            transform.eulerAngles = new Vector3(0, 0, facing);
        }
        else
            facing = 0;

        //ship trajectory prediction
        var lineDraw = GetComponent<TrajectoryPredict>();
        lineDraw.SimulatePath(position, velocity, 200, 2);

        //turret rotation and aim
        turret.transform.up = Quaternion.Euler(new Vector3(0, 0, TurretFacing())) * Vector2.up;

        //turret firing
        var turretBehaviorScript = turret.GetComponent<TurretBehavior>();
        if (!disabled)
        {
            if (SecondaryFire())
            {
                turretBehaviorScript.ProjectileTrajectory();
                triggerPressed = true;
            }
            else if (!SecondaryFire() && triggerPressed == true)
            {
                turretBehaviorScript.ProjectileFire();
                triggerPressed = false;
                firing = true;
            }
        }
        else firing = false;

        //Health and damage
        if (health <= 0 || transform.position.magnitude >= deathField) {
            StartCoroutine(deathSequence());
        }

        //Animation
        if (inputAxis > 0)
        {
			if (!thrusterBaseAudio.isPlaying) thrusterBaseAudio.Play ();
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
			if (!thrusterBaseAudio.isPlaying) thrusterBaseAudio.Play ();
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
			if(justAccelerated == true) thrusterBase.AddComponent<AudioFade>().fullVolume = 0.5f;
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
    void OnCollisionEnter2D(Collision2D coll)
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
        mainC.GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings = c;
    }

    IEnumerator deathSequence()
    {
        Time.timeScale = Mathf.Epsilon;
        yield return new WaitForSecondsRealtime(0.3f);
        Instantiate(death, position, new Quaternion(0, 0, 0, 0));

        
        if (GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (g != gameObject)
                {
                    MenuScreen.GetComponent<ExtranousMenu>().EndMenu(g.GetComponent<ColorManager>().team, g.GetComponent<InputManager>().player);
                }
            }

            mainC.gameObject.AddComponent<AudioFade>();
        }
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    IEnumerator hitSequence()
    {
        Time.timeScale = Mathf.Epsilon;
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1;
    }

    IEnumerator CooldownTimer(float time, bool var)
    { // very specific

        var = true;
        print(var);
        yield return new WaitForSeconds(time);
        var = false;
        print(var);
    }

    public float AccelAxis()
    {
        if (!disabled)
            return manager.AccelAxis();
        else
            return 0;
    }
    public float TurretFacing()
    {
        return manager.AimDir();
    }
    public bool SecondaryFire()
    {
        return manager.Fire();
    }
}
