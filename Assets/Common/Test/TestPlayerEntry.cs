using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 테스트용 PlayerEntry
/// </summary>
public class TestPlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;     // 플레이어의 이름

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<TMP_Text>();  // 자동 할당
    }

    public void SetPlayer(Player player)
    {
        if (player.IsMasterClient)
        {
            nameText.text = $"★{player.NickName}";      // 방장인 경우
        }
        else
        {
            nameText.text = player.NickName;            // 아닌 경우
        }
    }

    public void SetEmpty()
    {
        nameText.text = "";     // 플레이어 이름 텍스트 초기화
    }
}
