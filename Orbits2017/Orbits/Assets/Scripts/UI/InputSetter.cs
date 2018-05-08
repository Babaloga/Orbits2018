using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using XboxCtrlrInput;

public class InputSetter : MonoBehaviour {

    int controller;
    bool full;
    bool right;
    int avaliblePads;
    GameObject p1Group;
    GameObject p2Group;
    GameObject p3Group;
    GameObject p4Group;

    public GameObject beginButtons;

    GameObject[] keyToggles;
    GameObject[] conToggles;

    List<int> keyboardPlayers;
    List<int> controllerPlayers;

    public Dictionary<int, int> controllerDict;
    public Dictionary<int, int> teamDict;
    public Dictionary<int, bool> fullDict;
    public Dictionary<int, bool> rightDict;
    public Dictionary<int, bool> activeDict;
    public int numberOfPlayers;
    bool used = false;

    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(transform.gameObject);
        
        keyToggles = GameObject.FindGameObjectsWithTag("keyboardToggle");
        conToggles = GameObject.FindGameObjectsWithTag("controllerToggle");
        p1Group = GameObject.Find("P1 Group");
        p2Group = GameObject.Find("P2 Group");
        p3Group = GameObject.Find("P3 Group");
        p4Group = GameObject.Find("P4 Group");

        controllerDict = new Dictionary<int, int>();
        teamDict = new Dictionary<int, int>();
        fullDict = new Dictionary<int, bool>();
        rightDict = new Dictionary<int, bool>();
        activeDict = new Dictionary<int, bool>();

        keyboardPlayers = new List<int>();
        keyboardPlayers.Add(1);
        keyboardPlayers.Add(2);
        controllerPlayers = new List<int>();
        controllerPlayers.Add(3);
        controllerPlayers.Add(4);

        //for (int i = 1; i < 5; i++)
        //{
        //    teamDict[i] = i;
        //}
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Menu2")
        {
            if (used == true)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu2")
        {
            //avaliblePads = XCI.GetNumPluggedCtrlrs();
            avaliblePads = Input.GetJoystickNames().Length;
            //print(Input.GetJoystickNames()[0].ToString());
            if (avaliblePads < 1)
            {
                //if(p3Group) p3Group.SetActive(false);
                //if (p4Group) p4Group.SetActive(false);
                //activeDict[3] = false;
                //activeDict[4] = false;

                //PlayerPrefs.SetInt("Total Players", 2);

                //if (p3Group)
                //{
                //    foreach (Toggle t in p3Group.GetComponentsInChildren<Toggle>())
                //    {
                //        t.isOn = false;
                //    }
                //}
                //if (p4Group)
                //{
                //    foreach (Toggle t in p4Group.GetComponentsInChildren<Toggle>())
                //    {
                //        t.isOn = false;
                //    }
                //}
                foreach (GameObject c in conToggles)
                {
                    c.GetComponent<Toggle>().isOn = false;
                    c.GetComponent<Toggle>().interactable = false;
                }
            }
            else
            {
                if(p3Group) p3Group.SetActive(true);
                if (p4Group) p4Group.SetActive(true);
                //PlayerPrefs.SetInt("Total Players", 4);
                foreach (GameObject g in conToggles)
                {
                    g.GetComponent<Toggle>().interactable = true;
                }
            }

            if (controllerPlayers.Count >= avaliblePads * 2)
            {
                foreach (GameObject g in conToggles)
                {
                    if (!g.GetComponent<Toggle>().isOn) g.GetComponent<Toggle>().interactable = false;
                }
            }

            numberOfPlayers = 0;

            keyboardPlayers.Clear();
            foreach (GameObject k in keyToggles)
            {
                if (k.GetComponent<Toggle>().isOn)
                {
                    keyboardPlayers.Add(k.GetComponent<InputPasser>().player);
                    numberOfPlayers++;
                }
            }
            controllerPlayers.Clear();
            foreach (GameObject c in conToggles)
            {
                if (c.GetComponent<Toggle>().isOn)
                {
                    controllerPlayers.Add(c.GetComponent<InputPasser>().player);
                    numberOfPlayers++;
                }
            }

            //print(keyboardPlayers.Count + " " + controllerPlayers.Count);
            //print(PlayerPrefs.GetInt("Total Players"));
        }
        else
        {
            used = true;
        }
    }

    public void SetPreference(int player, bool useController)
    {
        GameObject group = GameObject.Find("P" + player + " Group");
        bool active = false;
        foreach(Toggle t in group.GetComponentsInChildren<Toggle>())
        {
            if (t.isOn)
            {
                active = true;
            }
        }

        if (!active)
        {
            activeDict[player] = false;
            GameObject.Find("P" + player + " Window").GetComponent<ColorReciever>().bright = false;
        }
        else
        {
            activeDict[player] = true;
            GameObject.Find("P" + player + " Window").GetComponent<ColorReciever>().bright = true;
        }

        controllerPlayers.Sort();
        keyboardPlayers.Sort();

        //print(controllerPlayers.Count + keyboardPlayers.Count);
        foreach (Button b in beginButtons.GetComponentsInChildren<Button>()) {
            if (controllerPlayers.Count + keyboardPlayers.Count <= 0) b.interactable = false;
            else b.interactable = true;
        }

        if (useController)
        {
            if(controllerPlayers.Count > avaliblePads)
            {
                if (controllerPlayers.Count == (2 * avaliblePads))
                {
                    fullDict[player] = false;
                    if(controllerPlayers.IndexOf(player) == 0 || controllerPlayers.IndexOf(player) == 2)
                    {
                        rightDict[player] = false;
                    }
                    else
                    {
                        rightDict[player] = true;
                    }

                    if(controllerPlayers.IndexOf(player) < 2)
                    {
                        controllerDict[player] = 1;
                    }
                    else
                    {
                        controllerDict[player] = 2;
                    }
                }
                else if (avaliblePads == controllerPlayers.Count - 1)
                {
                    if(controllerPlayers.IndexOf(player) == controllerPlayers.Count - 1)
                    {
                        fullDict[player] = false;
                        rightDict[player] = true;
                        controllerDict[player] = controllerPlayers.Count - 1;
                    }
                    else if (controllerPlayers.IndexOf(player) == controllerPlayers.Count - 2)
                    {
                        fullDict[player] = false;
                        rightDict[player] = false;
                        controllerDict[player] = controllerPlayers.Count - 1;
                    }
                    else
                    {
                        controllerDict[player] = controllerPlayers.IndexOf(player) + 1;
                        fullDict[player] = true;
                    }
                }
            }
            else //avalible players <= avalible pads
            {
                controllerDict[player] = controllerPlayers.IndexOf(player) + 1;
                fullDict[player] = true;
            }
        }
        else
        {
            controllerDict[player] = 0;
            if (keyboardPlayers.Count > 1)
            {

                fullDict[player] = false;

                if (keyboardPlayers.IndexOf(player) == 0 || keyboardPlayers.IndexOf(player) == 2)
                {
                    rightDict[player] = false;
                }
                else
                {
                    rightDict[player] = true;
                }
                foreach (GameObject g in keyToggles)
                {
                    if (!g.GetComponent<Toggle>().isOn) g.GetComponent<Toggle>().interactable = false;
                }
            }
            else
            {
                fullDict[player] = true;
                foreach (GameObject g in keyToggles)
                {
                    g.GetComponent<Toggle>().interactable = true;
                }
            }
        }
        
    }
}

