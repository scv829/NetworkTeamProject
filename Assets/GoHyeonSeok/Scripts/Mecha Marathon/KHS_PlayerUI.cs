using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; // 텍스트 변수

    public TextMeshProUGUI Text {  get { return _text; } set { _text = value; } }

    [SerializeField] private string _nickName;  // 받아오는 닉네임 변수

    public string NickName { get { return _nickName; } set{ _nickName = value; }}

    private void Start()
    {
        Text.text = _nickName;  // 해당 스크립트가 시작되면 저장된 닉네임을 UI에 출력
    }
}
