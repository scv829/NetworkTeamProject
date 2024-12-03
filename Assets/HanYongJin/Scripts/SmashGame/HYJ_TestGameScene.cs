using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class HYJ_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] HYJ_PlayerSpawn playerSpawnPoint;
    [SerializeField] HYJ_MonsterSpawn monsterSpawnPoint;
    [SerializeField] GameObject timer;
    [SerializeField] TMP_Text gameStartCountText;

    [SerializeField] Color[] playerColors;


    [SerializeField] private Player[] _curPhotonPlayer;
    public Player[] CurPhotonPlayer { get { return _curPhotonPlayer; } set { _curPhotonPlayer = value; } }
    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        //PhotonNetwork.ConnectUsingSettings(); //게임 씬 테스트용

        Vector3 vectorColor = new Vector3(playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].r, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].g, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].b);
        PhotonNetwork.LocalPlayer.SetPlayerColor(vectorColor);
        PhotonNetwork.LocalPlayer.SetLoad(true);

        CurPhotonPlayer = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(HJS_CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.NickName} 이 로딩이 완료되었습니다. ");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어 로딩 완료 여부 : {allLoaded} ");
            if (allLoaded)
            {
                StartCoroutine(StartDelayRoutine());
            }
        }
    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
                return false;
        }
        return true;
    }
    /*
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName,options,TypedLobby.Default);
    }
    */
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(2f); // 로딩기다리기
        
        gameStartCountText.text = "3";
        yield return new WaitForSeconds(1f);
        gameStartCountText.text = "2";
        yield return new WaitForSeconds(1f);
        gameStartCountText.text = "1";
        yield return new WaitForSeconds(1f);
        gameStartCountText.fontSize = 85;
        gameStartCountText.text = "Start!";
        yield return new WaitForSeconds(0.3f);
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("게임 시작");
        gameStartCountText.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);
        timer.gameObject.GetComponent<HYJ_GameTimer>().TimerStart();
        playerSpawnPoint.GetComponent<HYJ_PlayerSpawn>().PlayerSpawn();
        monsterSpawnPoint.GetComponent <HYJ_MonsterSpawn>().MonsterSpawn();
    }

    public void GameEnd(int _winnerNumber)
    {
        if(_winnerNumber != 0)
        {
            GameObject.FindWithTag("Player").SetActive(false);
            photonView.RPC("HYJ_GameEnd", RpcTarget.All, _winnerNumber);
        }
    }

    [PunRPC]
    public void HYJ_GameEnd(int winnerNumber)
    {
        gameStartCountText.gameObject.SetActive(true);
        gameStartCountText.text = $"{winnerNumber}P is Winner!!!";
        Player winner = CurPhotonPlayer[winnerNumber-1];
        HJS_GameSaveManager.Instance.GameOver(new Player[] {winner});
    }
}
