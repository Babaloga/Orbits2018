using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour {
    public int team;
    public int player;
    public InputSetter inSet;

    Color brightRed;
    Color brightYel;
    Color brightBlu;
    Color brightGre;
    Color darkRed;
    Color darkYel;
    Color darkBlu;
    Color darkGre;

    Color[] brightColors;
    Color[] darkColors;

	// Use this for initialization
	void Start () {
        
        brightColors = new Color[4];
        darkColors = new Color[4];

        brightRed = Color.red;
        brightBlu = Color.blue;
        brightYel = new Color(1, 1, 0);
        brightGre = Color.green;
        brightColors[0] = brightRed;
        brightColors[1] = brightBlu;
        brightColors[2] = brightYel;
        brightColors[3] = brightGre;

        darkRed = new Color32(130, 0, 0, 255);
        darkYel = new Color32(125, 105, 0, 255);
        darkBlu = new Color32(0, 0, 130, 255);
        darkGre = new Color32(0, 130, 0, 255);

        darkColors[0] = darkRed;
        darkColors[1] = darkBlu;
        darkColors[2] = darkYel;
        darkColors[3] = darkGre;

        if (SceneManager.GetActiveScene().name != "Menu2")
        {
            if (GameObject.Find("InputChoice"))
            {
                team = GameObject.Find("InputChoice").GetComponent<InputSetter>().teamDict[player];
            }
            else team = player;
        }
        else
        {
            inSet = GameObject.Find("InputChoice").GetComponent<InputSetter>();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu2")
        {
            //print("Setting " + player + " to team " + team);
            inSet.teamDict[player] = team;
        }
        else
        {
            if (GetComponent<TrajectoryPredict>())
            {
                GetComponent<TrajectoryPredict>().lineColor = SetColor(true);
            }
        }
    }

    public Color SetColor(bool bright)
    {
        if (bright)
        {
            return brightColors[team - 1];
        }
        else
        {
            return darkColors[team - 1];
        }
    }

    public void CycleTeam(bool up)
    {
        if (up)
        {
            if (team == 4)
            {
                team = 1;
            }
            else team++;
        }
        else
        {
            if (team == 1) team = 4;
            else team--;
        }


    }
}
