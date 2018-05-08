using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour
{
    public Camera mainC;
    public GameObject prevUI;
    public string firstSelectedName;
    // public GameObject loadingImage;
    // public

    public void LoadScene(int level)
    {
     //   loadingImage.SetActive(true);
        SceneManager.LoadScene(level);
        Time.timeScale = 1;

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResumeGame()
    {
        mainC = Camera.main;
        mainC.GetComponent<AudioReverbFilter>().enabled = false;
        mainC.GetComponent<AudioLowPassFilter>().enabled = false;
        Time.timeScale = 1;
        // this.enabled = false;
        gameObject.transform.parent.gameObject.SetActive(false);
      // GetComponent<>
    }

    public void LoadPrefab(Object pre)
    {
        
        Instantiate(pre, Vector3.zero, Quaternion.Euler(0, 0, 0));
        prevUI.SetActive(false);
        GameObject.Find(firstSelectedName).GetComponent<Button>().Select();
    }
}