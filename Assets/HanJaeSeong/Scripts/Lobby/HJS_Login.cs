using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HJS_Login : MonoBehaviour
{
    [SerializeField] HJS_GameConnect connect;

    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] HJS_PopupPanel popupPanel;

    [SerializeField] string userId = "tester";

    [Header("Test")]
    [SerializeField, Tooltip("테스트 모드 여부")] bool isTest;   // 테스트 모드일 때는 인증 상관없이 로그인 가능

    public void Login()
    {
        string email = emailField.text;
        string password = passwordField.text;

        HJS_FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("로그인 취소");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.Log($"로그인 에러, 사유 : {task.Exception}");

                    // 아이디가 없을 때
                    if (task.Exception.Message.Contains("An email address must be provided"))
                    {
                        popupPanel.ShowPopup("모든 내용을 입력해주세요.");
                    }
                    // 비밀번호가 입력
                    else if(task.Exception.Message.Contains("A password must be provided"))
                    {
                        popupPanel.ShowPopup("모든 내용을 입력해주세요.");
                    }
                    else if(task.Exception.Message.Contains("The email address is badly formatted"))
                    {
                        popupPanel.ShowPopup("이메일 주소 형식으로 입력해주세요.");
                    }
                    else if(task.Exception.Message.Contains("An internal error has occurred"))
                    {
                        popupPanel.ShowPopup("아이디 혹은 비밀번호가 맞지 않습니다.");
                    }

                    return;
                }

                AuthResult result = task.Result;
                Debug.Log($"유저 로그인 성공! {result.User.DisplayName} ({result.User.UserId})");

                // 이메일 인증 여부 
                if (!isTest && result.User.IsEmailVerified.Equals(false))
                {
                    popupPanel.ShowPopup("Please verify your email!");
                    return;
                }

                // 인증이 됐을 때 유저 정보 확인
                CheckUserInfo();
            });

    }

    private void CheckUserInfo()
    {
        string uid = HJS_FirebaseManager.Auth.CurrentUser.UserId;
        DatabaseReference userDataRef = HJS_FirebaseManager.Database.RootReference.Child("UserData").Child(uid);

        userDataRef.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("값 가져오기 취소됨");
                    return;
                }
                else if (task.IsFaulted)
                {
                    Debug.LogWarning($"값 가져오기 실패 : {task.Exception.Message}");
                    return;
                }

                Debug.Log("값 가져오기 성공!");
                DataSnapshot snapshot = task.Result;

                if (snapshot.Value is null)
                {
                    HJS_UserData userData = new HJS_UserData();
                    userData.name = HJS_FirebaseManager.Auth.CurrentUser.DisplayName;
                    userData.email = HJS_FirebaseManager.Auth.CurrentUser.Email;
                    userData.spawnPos = new Vector3(0, 0, 0);

                    userData.record.Reset();

                    PhotonNetwork.LocalPlayer.NickName = HJS_FirebaseManager.Auth.CurrentUser.DisplayName;

                    string json = JsonUtility.ToJson(userData);
                    userDataRef.SetRawJsonValueAsync(json);
                }
                else
                {
                    PhotonNetwork.LocalPlayer.NickName = snapshot.Child("name").Value.ToString();

                    Debug.Log(snapshot.Child("name").Value);
                    Debug.Log(snapshot.Child("email").Value);

                    foreach (DataSnapshot data in snapshot.Child("record").Children)
                    {
                        Debug.Log($"Record's {data} : {data.Value}");
                    }

                    List<string> list = new List<string>();

                    foreach (DataSnapshot data in snapshot.Child("spawnPos").Children)
                    {
                        list.Add(data.Value.ToString());
                        Debug.Log($"spawnPos's {data} : {data.Value}");
                    }

                    HJS_PlayerPosition.Instance.PlayerPos = new Vector3(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2]));
                }
            })
             .ContinueWithOnMainThread(task =>
             {
                 Debug.Log("get");
                 PhotonNetwork.LocalPlayer.SetPlayerUID(uid);
                 PhotonNetwork.ConnectUsingSettings();

                 // 씬 넘어가기
                 SceneManager.LoadScene("HJS_MainScene");
                 PhotonNetwork.AutomaticallySyncScene = true;
             });
    }


}
