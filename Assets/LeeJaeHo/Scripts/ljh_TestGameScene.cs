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
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class ljh_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "Testroom";

    [SerializeField] GameObject playerPrefab;
    [SerializeField] ljh_InputManager inputManager;

    [SerializeField] GameObject playerSpawner1;
    [SerializeField] GameObject playerSpawner2;
    [SerializeField] GameObject playerSpawner3;
    [SerializeField] GameObject playerSpawner4;

    Vector3 playerPos1;
    Vector3 playerPos2;
    Vector3 playerPos3;
    Vector3 playerPos4;

    Color playerColor1;
    Color playerColor2;
    Color playerColor3;
    Color playerColor4;

    [SerializeField] GameObject[] cartArray;
    [SerializeField] GameObject cart1;
    [SerializeField] GameObject cart2;
    [SerializeField] GameObject cart3;
    [SerializeField] GameObject cart4;

    public int curUserNum;
    public GameObject player;

    public GameObject[] playerArray;
    public int playerCount;


    [SerializeField] public Vector3 playerPos;
    Color playerColor;

    public int index;

    public Vector3[] vectorPlayerSpawn;


    private void Start()
    {
        

        playerArray = new GameObject[4];
        vectorPlayerSpawn = new Vector3[4];

        playerPos1 = playerSpawner1.transform.position;
        playerPos2 = playerSpawner2.transform.position;
        playerPos3 = playerSpawner3.transform.position;
        playerPos4 = playerSpawner4.transform.position;

        playerColor1 = new (1, 0, 0);
        playerColor2 = new (0, 0, 1);
        playerColor3 = new (0, 1, 0);
        playerColor4 = new (0, 0, 0);

        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(0000,9999)}"; // 이거 지우고
        //PhotonNetwork.ConnectUsingSettings(); // 이거 지우고

        PhotonNetwork.LocalPlayer.SetLoad(true);
    }


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
        //StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
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

    private void PlayerSpawn()
    {
        index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        vectorPlayerSpawn = new Vector3[] { playerPos1, playerPos2, playerPos3, playerPos4};

        playerPos = new Vector3(vectorPlayerSpawn[index].x, 0, vectorPlayerSpawn[index].z);
        player = PhotonNetwork.Instantiate("ljh_Player", playerPos, Quaternion.identity);
        player.GetComponent<ljh_Player>().playerNumber = (PlayerNumber)index;
        


        RideCart(player);

        Color[] vectorColor = { playerColor1, playerColor2, playerColor3, playerColor4 };
        playerColor = new Color(vectorColor[index].r, vectorColor[index].g, vectorColor[index].b, 1);

        player.GetComponentInChildren<Renderer>().material.color = playerColor;


        

    }

    private void RideCart(GameObject player)
    {
        player.transform.parent = cartArray[index].transform;
    }

    










}
