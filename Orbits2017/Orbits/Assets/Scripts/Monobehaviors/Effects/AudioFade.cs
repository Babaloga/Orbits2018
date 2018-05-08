using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class AudioFade : MonoBehaviour {

	public float timer = 15f;
	public AudioSource source;
	public float fullVolume = 1;

	void Awake(){
		source = GetComponent<AudioSource> ();
		//print (source);
	}
	void Update()
	{
			if (timer > 0) {
				source.volume -= 0.1f;
				timer -= 1f;
				//print (timer + " " + source.volume);
			}
			if (source.volume == 0) {
				//print ("finished");
				source.Stop ();
				source.volume = fullVolume;
				Destroy (this);
			}
	}
}