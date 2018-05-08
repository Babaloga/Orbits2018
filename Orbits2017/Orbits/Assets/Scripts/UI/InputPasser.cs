using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPasser : MonoBehaviour {

    public int player;
    public InputSetter inSet;

    private void FixedUpdate()
    {
        inSet = GameObject.Find("InputChoice").GetComponent<InputSetter>();
        inSet.SetPreference(player, GetComponent<Toggle>().isOn);
    }
}
