using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class HJS_GameConnect : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject PlayerPrefab;

    // 플레이어가 들어왔을 때
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, HJS_PlayerPosition.Instance.playerPos, Quaternion.identity);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        Vector3 pos = HJS_PlayerPosition.Instance.playerPos;

        Debug.Log($"플레이어 퇴장 : {pos}");

        // TODO: 플레이어 퇴장 시 위치 저장
     //   // 현재 로그인 한 유저를 가져온다.
     //   FirebaseUser user = HJS_FirebaseManager.Auth.CurrentUser;
     //   // 없으면 중단
     //   if (user == null) return;
     //
     //   // 있으면 고유 번호를 가져온다.
     //   string uid = user.UserId;
     //
     //   // 고유번호의 해당하는 플레이어 위치 테이블의 위치를 설정하고.
     //   DatabaseReference userDataRef = HJS_FirebaseManager.Database.RootReference.Child("UserData").Child(uid);
     //
     //   // 설정한 테이블을 가져온다
     //   userDataRef.GetValueAsync()
     //       .ContinueWithOnMainThread(task =>
     //       {
     //           // 오류가 발생하면 예외처리를 진행하고
     //           if (task.IsCanceled || task.IsFaulted) { Debug.LogError($"오류 발생! 사유 : {task.Exception}"); return; }
     //
     //           // 테이블의 데이터를 스냅샷으로 가져온다
     //           DataSnapshot snapshot = task.Result;
     //
     //           Dictionary<string, object> dic = new Dictionary<string, object>();
     //           dic["/spawnPos/x"] = pos.x;
     //           dic["/spawnPos/y"] = pos.y;
     //           dic["/spawnPos/z"] = pos.z;
     //
     //           userDataRef.UpdateChildrenAsync(dic);
     //       });
    }
}
