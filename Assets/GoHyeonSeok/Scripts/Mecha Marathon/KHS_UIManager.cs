using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KHS_UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;     // 텍스트 변수
    [SerializeField] private WaitForSeconds _delay;     // 코루틴 딜레이를 위한 변수
    [SerializeField] private GameObject _coolTimeObj;   // 현재 'J'키를 입력할 수 있다고 표시해주는 쿨타임 오브젝트 변수
    [SerializeField] private Image _coolTimeImage;      // 쿨타임 이미지 변수
    [SerializeField] private GameObject _howToPlay;

    private void Start()
    {
        _delay = new WaitForSeconds(1); // 딜레이를 1초로 설정
        _coolTimeObj.SetActive(false);  // 게임이 시작했을시에는 비활성화 상태로 게임을 실행시켜주기
        _howToPlay.SetActive(false);
    }

    public void CountDownGame() // 카운트 다운 UI를 출력시키는 함수
    {
        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine()    // 카운트 다운에 필요한 로직이 있는 코루틴 함수
    {
        _howToPlay.SetActive(true);
        _text.text = "3";
        yield return _delay;

        _text.text = "2";
        yield return _delay;

        _text.text = "1";
        yield return _delay;

        _text.text = "Game Start!";
        _howToPlay.SetActive(false);
        _coolTimeObj.SetActive(true);           // 게임이 시작되면 쿨타임 오브젝트를 활성화 시켜주기
        StartCoroutine(CoolTimeCoroutine());    // 쿨타임을 출력시키는 코루틴 호출
        yield return _delay;
       
        _text.text = "";

    }

    private IEnumerator CoolTimeCoroutine()     // 쿨타임 이미지를 보여주는 코루틴 함수
    {
        _coolTimeImage.fillAmount = 0;  // 처음 시작했을때는 서서히 차오르는 연출을 위해 fillAmount를 0으로 해주기
        float duration = 5f;    // 5초동안 진행되야하기 때문에 5로 초기화
        float elapsedTime = 0f; // 현재 경과시간 변수 0으로 초기화

        while (elapsedTime < duration)  // 경과시간이 5초보다 작을시,
        {
            elapsedTime += Time.deltaTime;  // 경과시간에 1초씩 더해주기
            _coolTimeImage.fillAmount = elapsedTime / duration; // 5초동안 서서히 fillAmount가 1에 가까워지면서 점점 이미지가 채워지는 연출 진행
            yield return null;
        }

        _coolTimeImage.fillAmount = 1;  // 반복문에서 나오면 1로 정확하게 만들어주기
        _coolTimeObj.SetActive(false);  // 1이 된다면 다시 해당 오브젝트를 비활성화
    }

    public void OnWinner(string winner) // 승자를 출력해주는 함수
    {
        _text.text = $"Winner is {winner} Player!";
    }

    public void NoWinner()  // 승자가 없는 경우에 출력하는 함수
    {
        _text.text = "DRAW..";
    }
}
