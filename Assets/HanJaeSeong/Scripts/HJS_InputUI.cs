using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HJS_InputUI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TMP_Text directionText;    // 방향 텍스트

    private StringBuilder sb = new StringBuilder();


    public void SetDirection(string answer)
    {
        sb.Clear();
        sb.Append(answer);
        directionText.SetText(sb);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 변수 데이터를 보내는 경우
        if (stream.IsWriting)
        {
            stream.SendNext(directionText.text);
        }
        // 변수 데이터를 받는 경우
        else if (stream.IsReading)
        {
            sb.Clear();
            sb.Append((string)stream.ReceiveNext());
            directionText.SetText(sb);
        }
    }
}
