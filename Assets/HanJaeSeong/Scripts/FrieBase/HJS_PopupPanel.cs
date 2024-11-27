using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HJS_PopupPanel : MonoBehaviour
{
    [SerializeField] Image popupBackground;
    [SerializeField] TMP_Text popupText;

    public void ShowPopup(string text)
    {
        popupText.text = text;

        popupText.gameObject.SetActive(true);
        popupBackground.gameObject.SetActive(true);
    }

}
