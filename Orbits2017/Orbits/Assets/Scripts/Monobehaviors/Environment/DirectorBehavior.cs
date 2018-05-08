using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorBehavior : MonoBehaviour {
	public float AsteroidSpawnRadiusMax = 5.0f;
	public float AsteroidSpawnRadiusMin = 5.0f;
	public float AsteroidSpawnRate = 1.0f;
	private bool AsteroidSpawnCooldown = false;
	public float AsteroidSpeedInit = 20;
	public GameObject planet;
	public GameObject ship;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("SpawnAsteroid");
	}
	
	
	int RandomSignedIntegerBool(){
		
		if(Random.value > 0.5){
			
			return 1;
		}else{
			return -1;
		}
		
		
		
	}
	
	
	IEnumerator SpawnAsteroid(){
		if(AsteroidSpawnCooldown == true){
			
			yield break;
			}
		GameObject rock = Resources.Load("Asteroid_simple") as GameObject;
		rock =Instantiate(rock) as GameObject;
		//rock.transform.position = Random.insideUnitCircle;
		rock.transform.position = ship.transform.position;
		rock.transform.position += new Vector3(RandomSignedIntegerBool()*Random.Range(AsteroidSpawnRadiusMin,AsteroidSpawnRadiusMax),0,0);
		rock.transform.position += new Vector3(0,RandomSignedIntegerBool()*Random.Range(AsteroidSpawnRadiusMin,AsteroidSpawnRadiusMax),0);
		rock.GetComponent<Rigidbody2D>().mass = .2f; 
		rock.GetComponent<AsteroidBehavior>().velocity =((planet.transform.position-rock.transform.position).normalized *AsteroidSpeedInit);
		
		//rock.transform.position *= RandomSignedIntegerBool();
		//rock.transform.position.Normalize();
		//rock.transform.position *= AsteroidSpawnRadius;
		
		
		AsteroidSpawnCooldown = true;
		yield return new WaitForSeconds(AsteroidSpawnRate);
		AsteroidSpawnCooldown = false;
	}
}
