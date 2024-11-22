using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ljh_UIManager : MonoBehaviour
{
    [SerializeField] GameObject choiceText;

    

    public void ShowUiIdle()
    {
        choiceText.SetActive(false);
    }

    public void ShowUiMove()
    {

    }

    public void ShowUiChoice()
    {
        choiceText.SetActive(true);
    }
}
