using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ljh_UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text choiceText;

    

    public void ShowUiIdle()
    {
        choiceText.text = ljh_GameManager.instance.curState.ToString();
    }

    public void ShowUiEnterMove()
    {
        choiceText.text = ljh_GameManager.instance.curState.ToString();
    }

    public void ShowUiChoice()
    {
        choiceText.text = ljh_GameManager.instance.curState.ToString();
    }

    public void ShowUiExitMove()
    {
        choiceText.text = ljh_GameManager.instance.curState.ToString();
    }
}
