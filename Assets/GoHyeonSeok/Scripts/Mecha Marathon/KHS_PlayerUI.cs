using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHS_PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public TextMeshProUGUI Text {  get { return _text; } set { _text = value; } }

    [SerializeField] private string _nickName;

    public string NickName { get { return _nickName; } set{ _nickName = value; }}

    private void Start()
    {
        Text.text = _nickName;
    }
}
