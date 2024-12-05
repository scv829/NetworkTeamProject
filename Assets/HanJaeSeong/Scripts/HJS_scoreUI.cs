using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HJS_scoreUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TMP_Text scoreText;    // 점수 텍스트
    [SerializeField] Image profile;         // 프로필 이미지
    [SerializeField] Image background;      // 점수 배경 이미지


    private StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder();
    }

    public void SetProfile(Vector3 vectorColor)
    {
        Color color = new Color();
        color.r = vectorColor.x; color.g = vectorColor.y; color.b = vectorColor.z; color.a = 1;
        profile.color = color;
        color.a = 0.5f;
        background.color = color;
    }

    public void SetScore(int score)
    {
        sb.Clear();
        sb.Append(score);
        scoreText.SetText(sb);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 방장이 데이터를 전송
            stream.SendNext(int.Parse(scoreText.text));
        }
        else if (stream.IsReading)
        {
            // 클라이언트가 데이터를 수신
            int receivedScore = (int)stream.ReceiveNext();
            if (receivedScore.Equals(0)) return;

            sb.Clear();
            sb.Append(receivedScore);
            scoreText.SetText(sb);
        }
    }
}
