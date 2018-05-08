using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenTime : MonoBehaviour {
    float t;
    float startTime = 5;
    float endTime = 1;
	// Use this for initialization
	void Start () {
        t = 0;
        Time.timeScale = 1f;
        StartCoroutine("SlowDown");
	}
	
	// Update is called once per frame
	IEnumerator SlowDown () {
        while (Time.timeScale < startTime)
        {
            Time.timeScale = Mathf.Lerp(1f, startTime, t / 10);
            t++;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        t = 0;
        while (Time.timeScale > endTime)
        {
            Time.timeScale = Mathf.Lerp(startTime, endTime, t / 200);
            t++;
            yield return new WaitForSecondsRealtime(0.01f);
        }
	}
}
