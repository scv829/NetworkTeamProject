using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 게임의 씬을 관리하는 클래스
/// 역할: 플레이어 배치, 카메라 움직임, 연출 관리
/// </summary>
public class HJS_GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] HJS_GameMaster gameMaster;
    [SerializeField] Transform[] spawnPoint;

    // 플레이어 배치
    public void PlayerSpawn()
    {
        int number = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        PhotonNetwork.Instantiate("HJS/Player", spawnPoint[number].position, Quaternion.identity);
    }

    // 게임 시작 직전 애니메이션 시작
    public void PlayAnimation()
    {
        StartCoroutine(GameStartAnimationRoutine());
    }

    private IEnumerator GameStartAnimationRoutine()
    {

        int len = PhotonNetwork.PlayerList.Length;

        Vector3 arriveCameraPos;

        // 플레이어 전부 보여주기
        for (int i = 1; i < len; i++) 
        {
            arriveCameraPos = new Vector3(spawnPoint[i].position.x , 1.5f, -11f);
            while (true)
            {
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, arriveCameraPos, 5 * Time.deltaTime);
                if (Camera.main.transform.position.x >= arriveCameraPos.x)
                {
                    Camera.main.transform.position = arriveCameraPos;
                    break;
                }
                yield return null;
            }
        }

        // 카메라 전진 출발점으로 이동하기
        arriveCameraPos = new Vector3(0, 4.5f, -11f);
        while (true)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, arriveCameraPos, 3 * Time.deltaTime);
            if (Camera.main.transform.position.Equals(arriveCameraPos))
            {
                break;
            }
            yield return null;
        }

        // 플레이어들 전진
        
        // 카메라 전진
        arriveCameraPos = new Vector3(0, 4.5f, 4.5f);   // 도착위치 : (0, 4.5, 4.5)
        while (true) 
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, arriveCameraPos, 5 * Time.deltaTime);
            if(Camera.main.transform.position.z >= arriveCameraPos.z)
            {
                Camera.main.transform.position = arriveCameraPos;
                break;
            }
            yield return null;
        }

        // 게임 시작 요청
        GameStart();
    }

    // 4. 게임 시작 요청
    private void GameStart() => gameMaster.GameStart();

}
