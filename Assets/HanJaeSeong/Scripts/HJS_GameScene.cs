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
    [SerializeField] Color[] playerColors;  // 플레이어의 색상

    [Header("FadeCanvas")]
    [SerializeField] Image fadeImage;
    [SerializeField] bool isFadeOver;       // 페이드 끝났는지 확인

    // 0. 씬 로드 확인
    private void Start()
    {
        Vector3 vectorColor = new Vector3(playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].r, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].g, playerColors[PhotonNetwork.LocalPlayer.GetPlayerNumber()].b);
        PhotonNetwork.LocalPlayer.SetPlayerColor(vectorColor);
        PhotonNetwork.LocalPlayer.SetLoad(true);
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
                // 플레이어 배치
                PlayerSpawn();
                // 카메라 활성화
                FadeIn();
                // 게임 시작 전 애니메이션 시작
                PlayAnimation();
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

    // 1. 플레이어 배치
    private void PlayerSpawn()
    {
        int number = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        GameObject instance = PhotonNetwork.Instantiate("HJS/Player", spawnPoint[number].position, Quaternion.identity);
        instance.GetComponent<HJS_PlayerController>().SetRenderTexture(number);
    }

    // 2. 카메라 활성화 -> 페이드 인
    private void FadeIn()
    {
        // UI가 점점 투명해진다
        StartCoroutine(Fade(1, 0, 1));
    }

    private IEnumerator Fade(float start, float end, float time)
    {
        isFadeOver = false;

        yield return new WaitForSeconds(1f);

        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }

        isFadeOver = true;
    }


    // 3. 게임 시작 직전 애니메이션 시작
    private void PlayAnimation()
    {
        StartCoroutine(GameStartAnimationRoutine());
    }

    private IEnumerator GameStartAnimationRoutine()
    {
        yield return new WaitUntil(() => {  
            return isFadeOver.Equals(true); 
        });

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

    // 5. 게임 종료 시 애니메이션 시작
    public void GameEnd()
    {
        // TODO: 게임 종료시 연출
        StartCoroutine (GameEndAnimationRoutine());
    }

    private IEnumerator GameEndAnimationRoutine()
    {
        // 카메라를 일정 거리 뒤로 이동
        yield break;
    }
}
