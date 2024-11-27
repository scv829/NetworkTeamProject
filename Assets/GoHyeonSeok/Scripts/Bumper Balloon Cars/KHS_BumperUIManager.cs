using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_BumperUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private WaitForSeconds _delay = new WaitForSeconds(1f);

    public void CountDownGameStart()
    {
        StartCoroutine(CountDownCoroutine());
    }

    public void ResultGame()
    {
        _text.text = "Game Over!";
    }

    private IEnumerator CountDownCoroutine()
    {
        _text.text = "3";
        yield return _delay;

        _text.text = "2";
        yield return _delay;

        _text.text = "1";
        yield return _delay;

        _text.text = "Game Start!!!";
    }
}
