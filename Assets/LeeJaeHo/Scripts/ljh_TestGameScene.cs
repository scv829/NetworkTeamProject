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

public class ljh_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "Testroom";

    [SerializeField] GameObject playerPrefab;

    Coroutine spawnRoutine;

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


    [SerializeField] Vector3 playerPos;
    Color playerColor;

    PhotonView pv;
    int index;

    private void Start()
    {
        playerPos1 = playerSpawner1.transform.position;
        playerPos2 = playerSpawner2.transform.position;
        playerPos3 = playerSpawner3.transform.position;
        playerPos4 = playerSpawner4.transform.position;

        playerColor1 = new (1, 0, 0);
        playerColor2 = new (0, 0, 1);
        playerColor3 = new (0, 1, 0);
        playerColor4 = new (0, 0, 0);

        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(0000,9999)}";
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
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }
    public void TestGameStart()
    { 
        PlayerSpawn();

        if (!PhotonNetwork.IsMasterClient)
            return;

      //  spawnRoutine = StartCoroutine(MonsterSpawnRoutine());
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

        Vector3[] vectorPlayerSpawn = { playerPos1, playerPos2, playerPos3, playerPos4};

        playerPos = new Vector3(vectorPlayerSpawn[index].x, 0, vectorPlayerSpawn[index].z);

        GameObject player = PhotonNetwork.Instantiate("GameObject/ljh_Player", playerPos, Quaternion.identity);

        Color[] vectorColor = { playerColor1, playerColor2, playerColor3, playerColor4 };
        Debug.Log(playerColor1.r);
        Debug.Log(playerColor1.g);
        Debug.Log(playerColor1.b);
        playerColor = new Color(vectorColor[index].r, vectorColor[index].g, vectorColor[index].b, 1);

        player.GetComponentInChildren<Renderer>().material.color = playerColor;

        // Todo : 다른 플레이어의 색깔 지정해야함

    }









}