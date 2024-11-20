using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class KHS_TestGameScean : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] private KHS_PlayerController[] _PlayerController;
    [SerializeField] private bool _isStarted;
    [SerializeField] private Player[] _sortedPlayers;


    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
        _isStarted = false;
    }

    // 마스터 서버에 연결
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    // 방에 참가했을때 호출되는 함수
    public override void OnJoinedRoom()
    {
        // 플레이어 리스트를 넣어주고
        _sortedPlayers = PhotonNetwork.PlayerList;
        StartCoroutine(StartDelayRoutine());
    }

    // 네트워크에 진입 후 준비에 필요한 시간 살짝 주기
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);    // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();
    }

    // 테스트 게임씬 시작
    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        _isStarted = true;
        // TODO : 테스트용 게임 시작 부분
        PlayerSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;

        HeyHoSpawn();
    }

    // 플레이어 스폰
    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        Vector3 spawnPosition = Vector3.zero;

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Player", randomPos, Quaternion.identity/*, data : data*/);
    }

    // 헤이호 스폰
    private void HeyHoSpawn()
    {

        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        //Color color = Random.ColorHSV();
        //object[] data = { color.r, color.g, color.b };

        PhotonNetwork.Instantiate("KHS/KHS_Hey-Ho", randomPos, Quaternion.identity/*, data : data*/);

    }
}
