using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KHS_UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private WaitForSeconds _delay;
    [SerializeField] private GameObject _coolTimeObj;
    [SerializeField] private Image _coolTimeImage;

    private void Start()
    {
        _delay = new WaitForSeconds(1);
        _coolTimeObj.SetActive(false);
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
        _coolTimeObj.SetActive(true);
        StartCoroutine(CoolTimeCoroutine());
        yield return _delay;
       
        _text.text = "";

    }

    private IEnumerator CoolTimeCoroutine()
    {
        _coolTimeImage.fillAmount = 0;
        float duration = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _coolTimeImage.fillAmount = elapsedTime / duration;
            yield return null;
        }

        _coolTimeImage.fillAmount = 1;
        _coolTimeObj.SetActive(false);
    }

    public void OnWinner(string winner)
    {
        _text.text = $"Winner is {winner} Player!";
    }
}
