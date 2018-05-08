using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDitto : MonoBehaviour {
    public ColorReciever source;
    ColorReciever thisColor;

	// Use this for initialization
	void Start () {
        thisColor = GetComponent<ColorReciever>();
	}
	
	// Update is called once per frame
	void Update () {
        thisColor.bright = source.bright;
	}
}
