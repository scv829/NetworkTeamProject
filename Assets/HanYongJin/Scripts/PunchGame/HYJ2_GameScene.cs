using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public class HYJ2_GameScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    [SerializeField] HYJ2_FieldSpawn HYJ2_FieldSpawn;
    [SerializeField] TextMeshProUGUI _gameUIText;
    [SerializeField] Color[] playerColors;

    [SerializeField] Camera mainCamera;

    //승자 플레이어
    public int WinPlayer;

    [SerializeField] private Player[] _curPhotonPlayer;
    public Player[] CurPhotonPlayer { get { return _curPhotonPlayer; } set { _curPhotonPlayer = value; } }

    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        
        // TODO : 재성님에게 물어봐서 플레이어 컬러 적용법 배우기
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
                Debug.Log(PhotonNetwork.PlayerList.Length);
                // 게임 진행코드
                StartCoroutine(GameStart());
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

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName,options,TypedLobby.Default);
    }

    IEnumerator GameStart()
    {
        // 필드 스폰(필드가 스폰되면 필드에서 플레이어 스폰)
        Debug.Log("필드 스폰");
        HYJ2_FieldSpawn.gameObject.GetComponent<HYJ2_FieldSpawn>().FieldSpawn();
        _gameUIText.text = "Punch! with Spacebar";

        yield return new WaitForSeconds(2.5f);
        _gameUIText.gameObject.SetActive(false);
        // 중앙의 카메라를 필드에서 각 플레이어의 필드로 줌
        CameraMove();
        
        // 게임 시작
        

        HYJ2_FieldSpawn.gameObject.GetComponent<HYJ2_FieldSpawn>().ObjectManagerOn();
    }

    private void CameraMove()
    {
        Debug.Log("카메라 무브");
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1 :
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(-10, 18, 10), 25f);
                break;
            case 2:
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(10, 18, 10), 25f);
                break;
            case 3:
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(-10, 18, -10), 25f);
                break;
            case 4:
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(10, 18, -10), 25f);
                break;
        }
    }

    public void GameEnd(int _playerNumber)
    {
        if(_playerNumber != 0)
        {
            Debug.Log(_playerNumber);
            photonView.RPC("HYJ2_GameEnd", RpcTarget.All, _playerNumber);
        }
    }

    [PunRPC]
    public void HYJ2_GameEnd(int playerNumber)
    {
        _gameUIText.gameObject.SetActive(true);
        _gameUIText.text = $"{playerNumber}P is Winner!";
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(0, 35, 0), 25f);
        Player winner = CurPhotonPlayer[playerNumber - 1];
        HJS_GameSaveManager.Instance.GameOver(new Player[] { winner });
    }
}
