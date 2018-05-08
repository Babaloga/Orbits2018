using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorReciever : MonoBehaviour {
    public ColorManager colManage;
    public bool bright;
    Image im;
    SpriteRenderer rend;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu2")
            im = GetComponent<Image>();
        else
        {
            rend = GetComponent<SpriteRenderer>();

        }
    }

    void LateUpdate () {
        if (SceneManager.GetActiveScene().name == "Menu2")
            im.color = colManage.SetColor(bright);
        else
        {
            rend.color = colManage.SetColor(bright);
            enabled = false;
        }
    }
}
