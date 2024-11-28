using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
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

    [Header("UserProfile")] 
    [SerializeField] HJS_UserProfile profile;

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

    public void ShowProfile(int index)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetPlayerNumber().Equals(index))
            {
                profile.getUserProfile(player.GetPlayerUID());
                break;
            }
        }
    }

    public void CloseProfile()
    {
        profile.gameObject.SetActive(false);
    }
}
