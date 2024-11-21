using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class HJS_scoreUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TMP_Text scoreText;    // 점수 텍스트
    [SerializeField] Image profile;         // 프로필 이미지
    
    private StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder();
    }

    public void SetProfile(Color color)
    {
        profile.color = color;
    }

    public void SetScore(int score)
    {
        sb.Clear();
        sb.Append(score);
        scoreText.SetText(sb);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 변수 데이터를 보내는 경우
        if (stream.IsWriting)
        {
            stream.SendNext(int.Parse(scoreText.text));       
        }
        // 변수 데이터를 받는 경우
        else if (stream.IsReading)
        {
            sb.Clear();
            sb.Append((int)stream.ReceiveNext());
            scoreText.SetText(sb);
        }
    }
}
