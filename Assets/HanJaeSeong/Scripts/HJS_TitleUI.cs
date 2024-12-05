using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HJS_TitleUI : MonoBehaviourPun, IPunObservable
{

    [SerializeField] TMP_Text countText;

    private StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder();
    }

    public void UpdateUI(int count, int refeatTime)
    {
        sb.Clear();
        sb.Append($"{count} / {refeatTime}");
        countText.SetText(sb);
    }

    public void UpdateUI(string player)
    {
        sb.Clear();
        sb.Append($"{player} Win!!");
        countText.SetText(sb);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 변수 데이터를 보내는 경우
        if (stream.IsWriting)
        {

            stream.SendNext(countText.text);
        }
        // 변수 데이터를 받는 경우
        else if (stream.IsReading)
        {
            string input = (string)stream.ReceiveNext();

            if (input.Contains('0')) return;

            sb.Clear();
            sb.Append(input);
            countText.SetText(sb);
        }
    }

}
