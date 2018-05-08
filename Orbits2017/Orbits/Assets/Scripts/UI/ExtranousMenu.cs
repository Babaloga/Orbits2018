using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtranousMenu : MonoBehaviour {
    private GameObject MenuObject;
    private AudioSource Win;

    public GameObject endMenu;
    public Text text1;
    public Text text2;
    public Text title;

    public GameObject s1Outline;
    public GameObject s2Outline;
    public GameObject s3Outline;
    public GameObject s4Outline;

    public Color t1 = Color.red;
    public Color t2 = Color.blue;
    public Color t3 = Color.yellow;
    public Color t4 = Color.green;

    public float endDelay = 3;

    Color c;
    GameObject s;

    public void EndMenu(int team, int ship)
    {


        if (team == 1) { c = t1;
            title.text = "Red Wins";
        }
        else if (team == 2) { c = t2;
            title.text = "Blue Wins";
        }
        else if (team == 3) { c = t3;
            title.text = "Yellow Wins";
        }
        else { c = t4;
            title.text = "Green Wins";
        }

        endMenu.GetComponent<Image>().color = c;
        text1.color = c;
        text2.color = c;
        title.color = c;

        if (ship == 1) s = s1Outline;
        else if (ship == 2) s = s2Outline;
        else if (ship == 3) s = s3Outline;
        else s = s4Outline;

        s.GetComponent<Image>().color = c;
        StartCoroutine(EndScreenDelay());

        
    }

    IEnumerator EndScreenDelay()
    {
        yield return new WaitForSeconds(1f);
        Win = GameObject.Find("StarsBackground").GetComponent<AudioSource>();
        Win.Play();
        yield return new WaitForSeconds(endDelay);
        endMenu.SetActive(true);
        s.SetActive(true);
        GameObject.Find("Reset Button").GetComponent<Button>().Select();
    }

    public void PauseMenu()
    {
        MenuObject = transform.Find("PauseMenu").gameObject;
        MenuObject.SetActive(true);
        GameObject.Find("PauseButton1").GetComponent<Button>().Select();
    }
}
