using Photon.Chat.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ljh_TestGameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "Testroom";

    Coroutine spawnRoutine;
    [SerializeField] GameObject playerSpawner1;
    [SerializeField] GameObject playerSpawner2;
    [SerializeField] GameObject playerSpawner3;
    [SerializeField] GameObject playerSpawner4;

    [SerializeField] List<GameObject> playerSpawnerList;
    Vector3 playerPos;
    Queue<GameObject> playerposQ;
    Color playerColor;
    Queue<Color> playerColorQ;

    private void Start()
    {
        playerposQ = new Queue<GameObject>();

        playerposQ.Enqueue(playerSpawner1);
        playerposQ.Enqueue(playerSpawner2);
        playerposQ.Enqueue(playerSpawner3);
        playerposQ.Enqueue(playerSpawner4);

        playerColorQ = new Queue<Color>();

        playerColorQ.Enqueue(Color.red);
        playerColorQ.Enqueue(Color.blue);
        playerColorQ.Enqueue(Color.green);
        playerColorQ.Enqueue(Color.yellow);

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
        int playerNum = 1;

        Vector3 playerPos1 = playerSpawner1.transform.position;
        Vector3 playerPos2 = playerSpawner2.transform.position;
        Vector3 playerPos3 = playerSpawner3.transform.position;
        Vector3 playerPos4 = playerSpawner4.transform.position;

        Vector3 playerPos;
        Color playerColor;

        playerSpawnerList.Add(playerSpawner1);

       // playerPos; //= SpawnerPick(playerposQ);
       // playerColor = colorPick();
       //
       // GameObject player1 = PhotonNetwork.Instantiate("GameObject/ljh_Player", playerPos, Quaternion.identity);
       // player1.GetComponentInChildren<Renderer>().material.color = Color.red;
       //
       // GameObject player2 = PhotonNetwork.Instantiate("GameObject/ljh_Player", playerPos, Quaternion.identity);
       // player2.GetComponentInChildren<Renderer>().material.color = Color.blue;
       //
       // GameObject player3 = PhotonNetwork.Instantiate("GameObject/ljh_Player", playerPos, Quaternion.identity);
       // player3.GetComponentInChildren<Renderer>().material.color = Color.green;
       // 
       // GameObject player4 = PhotonNetwork.Instantiate("GameObject/ljh_Player", playerPos, Quaternion.identity);
       // player4.GetComponentInChildren<Renderer>().material.color = Color.yellow;

        // Todo : 본인꺼 생성하게 해야함
    }

    public Vector3 SpawnerPick(Queue playerQue)
    {
        playerPos = playerposQ.Peek().transform.position;
        playerposQ.Dequeue();

        return playerPos;
    }

    public Color colorPick()
    {
        playerColor = playerColorQ.Peek();
        playerColorQ.Dequeue();
        return playerColor;
    }

   
    
}
