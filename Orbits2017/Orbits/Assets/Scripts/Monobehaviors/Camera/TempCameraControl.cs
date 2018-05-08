using UnityEngine;
using System.Collections;

public class TempCameraControl : MonoBehaviour {

    public Vector3 position;
    public GameObject ship1;
    public GameObject ship2;
    public GameObject ship3;
    public GameObject ship4;
    public GameObject planet;
    private Vector3 velocity = Vector3.zero;
    private float scaleVelocity = 0;
    public float smoothTime = 0.2f;
    public float minCamSize = 600;
    private float camSize;
    private Vector3 shipDist;
    public Camera cam;
    public float scaleTime = 1;

    public bool singlePlayer;

	public float velConstant;
    public Transform spaceBackground;
    public float yDominance = 1.5f;
    public AudioFade fade;
    private bool matchEnd;
    private bool alreadyTriggered;
    public AudioLowPassFilter lowPass;
    public AudioReverbFilter reverb;
    public float buffer = 50;
    GameObject[] ships;

    bool oneTeamLeft;

    void Awake () {
        ship1 = GameObject.Find("ship1");
        ship2 = GameObject.Find("ship2");
        ship3 = GameObject.Find("ship3");
        ship4 = GameObject.Find("ship4");
        planet = GameObject.Find("planet");
        //cam = GetComponent<Camera>();
        fade = GetComponent<AudioFade>();
        matchEnd = false;
        alreadyTriggered = false;
        lowPass = GetComponent<AudioLowPassFilter>();
        reverb = GetComponent<AudioReverbFilter>();
        ships = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        ships = GameObject.FindGameObjectsWithTag("Player");
        camSize = minCamSize;
        if (ships.Length == 0)
        {
            position = new Vector3(0, 300, -10);
            matchEnd = true;
        }

        else if (ships.Length == 1)
        {
            if (singlePlayer)
            {
                position = (ship1.transform.position + planet.transform.position) / 2;
                shipDist = ship1.transform.position - planet.transform.position;
                camSize = (shipDist.magnitude * 0.66f) + buffer;
            }
            else
            {
                position = new Vector3(0, 300, -10);
                smoothTime = 3f;
                matchEnd = true;
            }
        }
        else
        {
            oneTeamLeft = true;   
            position = Vector2.zero;
            //float i = 0;
            foreach(GameObject ship in ships)
            {
                //Debug.DrawLine(position, position + (ship.transform.position / ships.Length), new Color(1 - (i / 100), 1 - (i / 100), i / 100));
                position += ship.transform.position / (ships.Length + 1);
                
                //i += 25;
            }
            position += planet.transform.position / (ships.Length + 1);
            //smoothTime = 1f;
            shipDist = Vector2.zero;
            GameObject g = ships[0];
            foreach(GameObject s in ships)
            {
                foreach(GameObject h in ships)
                {
                    Vector2 d = h.transform.position - s.transform.position;
                    //if(d.x > shipDist.x)
                    //{
                    //    shipDist = new Vector2(d.x, shipDist.y);
                    //}
                    //if (d.y > shipDist.y)
                    //{
                    //    shipDist = new Vector2(shipDist.x, d.y);
                    //}
                    if (d.magnitude > shipDist.magnitude) shipDist = d;

                    if (s.GetComponent<ColorManager>().team != h.GetComponent<ColorManager>().team) oneTeamLeft = false;
                }
                Vector2 e = planet.transform.position - s.transform.position;
                if (e.magnitude > shipDist.magnitude) shipDist = e;
                //if (e.x > shipDist.x)
                //{
                //    shipDist = new Vector2(e.x, shipDist.y);
                //}
                //if (e.y > shipDist.y)
                //{
                //    shipDist = new Vector2(shipDist.x, e.y);
                //}

                g = s;
            }

            //if (Mathf.Abs(shipDist.x) > Mathf.Abs(shipDist.y) * yDominance * cam.aspect)
            //    camSize = ((1.2f) * Mathf.Abs(shipDist.x) + buffer) / (2 * cam.aspect);
            //else
            //    camSize = (0.7f) * Mathf.Abs(shipDist.y) + buffer;
            camSize = (shipDist.magnitude * 1f) + buffer;
            Debug.DrawLine(Vector3.zero, Vector3.up * camSize, Color.red);

            if (oneTeamLeft)
            {
                GameObject.Find("Canvas").GetComponent<ExtranousMenu>().EndMenu(g.GetComponent<ColorManager>().team, g.GetComponent<InputManager>().player);
            }

        }

		transform.position = Vector3.SmoothDamp(transform.position, position + new Vector3 (0,0,-100), ref velocity, smoothTime);

        if (camSize <= minCamSize)
            camSize = minCamSize;

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSize, ref scaleVelocity, scaleTime);
        spaceBackground.localScale = new Vector3(cam.orthographicSize/10, cam.orthographicSize/10, 0);

        spaceBackground.position = transform.position + new Vector3 (0,0,124);

        if (matchEnd == true && alreadyTriggered == false)
        {
            GameObject.Find("Audio Source").AddComponent<AudioFade>();
            alreadyTriggered = true;
        }
    }
}
