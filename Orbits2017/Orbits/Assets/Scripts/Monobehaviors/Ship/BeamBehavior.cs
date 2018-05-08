using UnityEngine;
using System.Collections;
public class BeamBehavior : MonoBehaviour
{
    public LineRenderer beamLine;
    public int beamDistance;
	public float fadeTime = 1;
    public GameObject enemyShip;
	private bool justFired;
   // public bool IsAbleToFire = true;
  //  public GameObject self;


   
    public RaycastHit2D BeamFire(bool firing, bool IsAbleToFire, AudioSource pew)
    {
		
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        if (firing && IsAbleToFire)
        {
            //ray was defined here ~~
			justFired = true;
			beamLine.positionCount = 2;
            beamLine.SetPosition(0, transform.position + (2 * transform.up));
            if (!pew.isPlaying)
            pew.Play();
            
            if (hit.transform)
            {
                beamLine.SetPosition(1, transform.position + (transform.up * hit.distance) + (2 * transform.up));
                if (hit.transform.gameObject.GetComponent<ShipBehavior>())
                {
                    hit.transform.gameObject.GetComponent<ShipBehavior>().beingHit = true;
                    enemyShip = hit.transform.gameObject;
                }
                //else
                     
      
            }
            else
            {
                beamLine.SetPosition(1, transform.position + (2 * transform.up) + transform.up * beamDistance);
                if (enemyShip)
                {
                    print("kek");
                    enemyShip.GetComponent<ShipBehavior>().beingHit = false;
                }
            }
        }
        else
        {
            beamLine.positionCount = 0;

			if(justFired == true)
			gameObject.AddComponent<AudioFade>();

			justFired = false;
        }
        return hit;
    }
}