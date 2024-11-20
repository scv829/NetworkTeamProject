using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_TestGameScean : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);    // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        // TODO : 테스트용 게임 시작 부분
        PlayerSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;

        HeyHoSpawn();
    }

    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Player", randomPos, Quaternion.identity/*, data : data*/);
    }

    private void HeyHoSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

            //Color color = Random.ColorHSV();
            //object[] data = { color.r, color.g, color.b };

            PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", randomPos, Quaternion.identity/*, data : data*/);
        }
    }
}
