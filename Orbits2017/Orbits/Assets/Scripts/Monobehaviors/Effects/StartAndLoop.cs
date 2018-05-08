using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndLoop : MonoBehaviour {
    public AudioClip start;
    public AudioClip loop;

    public AudioSource aud;
    public double bufferTime = 0.0;
    double startTick;
    double sampleTime;
    double startTrackTime;

	// Use this for initialization
	void Start () {
        aud = transform.Find("Audio Source").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (GetComponent<AudioSource>().isPlaying)
        {
            startTick = AudioSettings.dspTime;
            startTrackTime = start.length;
            aud.PlayScheduled(startTick + (startTrackTime / GetComponent<AudioSource>().pitch) + bufferTime);
            enabled = false;
        }
    }
}
