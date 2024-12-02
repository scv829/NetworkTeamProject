using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 종료에 관련된 기능을 담당해주는 스크립트
/// </summary>
public class HJS_GameSaveManager : MonoBehaviourPun
{
    public static HJS_GameSaveManager Instance;         // 싱글톤
    
    [Header("Fade")]
    [SerializeField] HJS_FadeController fadeController;
    [Header("SceneName")]
    [SerializeField] string sceneName;  // 넘어갈 씬 이름

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 마스터 클라이언트에서 실행하므로 따로 예외처리는 없다
    public void GameOver(Player[] winners)
    {
        // 그래도 혹시 모르니 마스터 클라이언트 빼고는 결과를 처리하지 않게 예외처리
        if (PhotonNetwork.IsMasterClient == false) return;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            photonView.RPC("SaveResultRPC", player, winners.Contains(player));
        }

        StartCoroutine(ChangeSceneRoutine());
    }

    private IEnumerator ChangeSceneRoutine()
    {
        // 페이드 역할이 모두 끝났을 때 
        Debug.Log("페이드 역할 끝남");
        yield return new WaitUntil(() => {
            return fadeController.isFadeOver.Equals(true);
        });
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        yield return new WaitForSeconds(0.5f);  // 다른 컴퓨터의 성능에 따라 페이드가 진행되는 도중에 이동할 수 있어서 약간의 차이만큼 더 기다리기

        Debug.Log(sceneName);

        if (HJS_GameMap.instance is null || HJS_GameMap.instance.SceneEmpty())
        {
            PhotonNetwork.LoadLevel(sceneName);    // 초기 설정한 씬으로 이동
        }
        else
        { 
            PhotonNetwork.LoadLevel(HJS_GameMap.instance.NextMap());    // 씬으로 이동
        }

    }

    [PunRPC]
    private void SaveResultRPC(bool isWinner)
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} is start");
        FirebaseUser user = HJS_FirebaseManager.Auth.CurrentUser;

        if (user == null) { Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} is null"); return; }

        string uid = user.UserId;
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} get user!");

        DatabaseReference userRecordRef = HJS_FirebaseManager.Database.RootReference.Child("UserData").Child(uid).Child("record");

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        // 데이터를 불러오기
        userRecordRef.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogWarning($"에러 발생! {task.Exception}");
                    return;
                }

                DataSnapshot snapshot = task.Result;

                long total = (long)snapshot.Child("total").Value;
                long increase = (isWinner) ? (long)snapshot.Child("win").Value : (long)snapshot.Child("lose").Value;

                total++; increase++;

                dictionary["/total"] = total;
                dictionary[(isWinner ? "/win" : "/lose")] = increase;

                Debug.Log($"{user.Email} total: {total} increase: {increase}");
            })
            .ContinueWithOnMainThread(task =>
            {
                // 불러온 데이터를 저장하기
                userRecordRef.UpdateChildrenAsync(dictionary)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        Debug.LogWarning($"에러 발생! {task.Exception}");
                        return;
                    }

                    PhotonNetwork.LocalPlayer.SetLoad(false);
                    fadeController.FadeOut();
                });
            });
    }

}
