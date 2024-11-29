using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private WaitForSeconds _delay;

    private void Start()
    {
        _delay = new WaitForSeconds(1);
    }

    public void CountDownGame()
    {
        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine()
    {
        _text.text = "3";
        yield return _delay;

        _text.text = "2";
        yield return _delay;

        _text.text = "1";
        yield return _delay;

        _text.text = "Game Start!";
        yield return _delay;
       
        _text.text = "";
    }

    public void OnWinner(string winner)
    {
        _text.text = $"Winner is {winner} Player!";
    }
}
