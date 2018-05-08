using UnityEngine;
using System.Collections;

public class PlanetBehavior : MonoBehaviour {
    private Rigidbody2D hotBod;
    private Vector2 position;
    public GravityHandler gravity;
    public Vector2 velocity = Vector2.zero;
	public string planetType;
	// Use this for initialization
	void Start () {
        hotBod = GetComponent<Rigidbody2D>();
        hotBod.velocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
        
        position = this.transform.position;
        hotBod.AddForce(GravityHandler.SumFieldStrength(position) * hotBod.mass);
		
		if (planetType.ToLower() == "earth"){
			
			//Earth stuff
		}else if(planetType.ToLower() == "mars"){
			
			
			//Mars stuff
			
		}else if(planetType.ToLower() == "jupiter"){
			//jupiter stuff
			
			
			
		}else{
			
			//generic planet stuff
			
		}	
			
		
			
		
			
		
	}
}
