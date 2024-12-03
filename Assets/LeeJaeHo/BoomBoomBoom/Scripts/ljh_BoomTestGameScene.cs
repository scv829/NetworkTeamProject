using Photon.Chat.UtilityScripts;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class ljh_BoomTestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "Testroom";


    [SerializeField] GameObject playerSpawner1;
    [SerializeField] GameObject playerSpawner2;
    [SerializeField] GameObject playerSpawner3;
    [SerializeField] GameObject playerSpawner4;

    Vector3 playerPos1;
    Vector3 playerPos2;
    Vector3 playerPos3;
    Vector3 playerPos4;


    [SerializeField] public GameObject[] cartArray;

    public int curUserNum;
    public GameObject player;

    public GameObject[] playerArray;
    public int playerCount;


    [SerializeField] public Vector3 playerPos;

    public int index;

    public Vector3[] vectorPlayerSpawn;

    [SerializeField]
    public static Color[] playerColors = new Color[]
    {
        Color.red,
        new Color(1f, 0.5f, 0f),
        Color.yellow,
        Color.green
    };


    private void Start()
    {
        

        playerArray = new GameObject[4];

        vectorPlayerSpawn = new Vector3[4];

        //Comment 플레이어 스폰 위치 설정
        playerPos1 = playerSpawner1.transform.position;
        playerPos2 = playerSpawner2.transform.position;
        playerPos3 = playerSpawner3.transform.position;
        playerPos4 = playerSpawner4.transform.position;

        

        //PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(0000,9999)}"; // 이거 지우고
        //PhotonNetwork.ConnectUsingSettings(); // 이거 지우고

        PhotonNetwork.LocalPlayer.SetLoad(true);
    }

    //이놈 주석
   // public override void OnConnectedToMaster()
   // {
   //     RoomOptions options = new RoomOptions();
   //     options.MaxPlayers = 4;
   //     options.IsVisible = false;
   //
   //     PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
   // }

    public override void OnJoinedRoom()
    {
        //이놈 주석
     //   StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
    //이놈 주석 해제
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

    //이놈 주석 해제
    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
                return false;
        }
        return true;
    }
    public void TestGameStart()
    { 
        PlayerSpawn();

        //Comment 플레이어 리스트의 Count 로 현재 인원 파악
        //ljh_GameManager.instance.curUserNum = playerList.Count - 1; 테스트 끝나면 주석 해제

        if (!PhotonNetwork.IsMasterClient)
            return;

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.IsLocal)
        {
            
        }
    }

    //Comment 플레이어 스폰
    private void PlayerSpawn()
    {
        
        index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        vectorPlayerSpawn = new Vector3[] { playerPos1, playerPos2, playerPos3, playerPos4};

        playerPos = new Vector3(vectorPlayerSpawn[index].x, 0, vectorPlayerSpawn[index].z);
        player = PhotonNetwork.Instantiate("ljh_Player", playerPos, Quaternion.identity);
        player.GetComponent<ljh_Player>().playerNumber = (PlayerNumber)index;

        //Commnet : 색상 지정
        Vector3 vectorColor = new Vector3(playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].r, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].g, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].b);
        PhotonNetwork.LocalPlayer.SetPlayerColor(vectorColor);



    }


}
