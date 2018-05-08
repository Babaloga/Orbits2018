using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    Button sourceButton;
    Toggle sourceToggle;
    Text targetText;

    bool highlight;

	void Start () {
        targetText = GetComponentInChildren<Text>();
        if (GetComponent<Button>())
        {
            sourceButton = GetComponent<Button>();
            targetText.color = sourceButton.colors.normalColor;
        }

        if (GetComponent<Toggle>())
        {
            sourceToggle = GetComponent<Toggle>();
        }

        
    }
	
    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        highlight = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        highlight = false;
    }

    private void Update()
    {
        if (sourceButton)
        {
            if(!sourceButton.interactable) targetText.color = sourceButton.colors.disabledColor;
            else
            {
                if(highlight) targetText.color = sourceButton.colors.highlightedColor;
                else targetText.color = sourceButton.colors.normalColor;
            }
        }
        if (sourceToggle)
        {
            if (!sourceToggle.interactable) targetText.color = sourceToggle.colors.disabledColor;
            else
            {
                if(sourceToggle.isOn) targetText.color = Color.white;
                else targetText.color = new Color(0.7f, 0.7f, 0.7f, 1);
            }
        }
    }

}
