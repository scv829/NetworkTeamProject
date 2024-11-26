using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_BumperUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;


    public void ResultGame()
    {
        _text.text = "Game Over!";
    }


}
