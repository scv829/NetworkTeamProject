using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_BumperUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; // 텍스트 변수
    [SerializeField] private WaitForSeconds _delay = new WaitForSeconds(1f);    // 코루틴 딜레이를 위한변수

    public void CountDownGameStart()    // 카운트 다운을 시작하는 함수
    {
        StartCoroutine(CountDownCoroutine());
    }

    public void ResultGame(string winner)   // 우승자를 출력하는 함수
    {
        _text.text = $"winner Is {winner} Player!";
    }

    private IEnumerator CountDownCoroutine()    // 카운트 다운 코루틴 함수
    {
        _text.text = "3";
        yield return _delay;

        _text.text = "2";
        yield return _delay;

        _text.text = "1";
        yield return _delay;

        _text.text = "Game Start!!!";
        yield return _delay;

        _text.text = "";
    }
}
