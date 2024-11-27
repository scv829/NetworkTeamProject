using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HJS_LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] HJS_PopupPanel popupPanel;
    public void Login()
    {
        string email = emailField.text;
        string password = passwordField.text;


        HJS_FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task => 
            { 
                if(task.IsCanceled)
                {
                    Debug.Log("로그인 취소");
                    return;
                }
                if(task.IsFaulted)
                {
                    Debug.Log($"로그인 에러, 사유 : {task.Exception}");
                    return;
                }
                
                AuthResult result = task.Result;
                Debug.Log($"유저 로그인 성공! {result.User.DisplayName} ({result.User.UserId})");

                // 이메일 인증 여부 
                if(result.User.IsEmailVerified.Equals(false))
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

                if(snapshot.Value is null)
                {
                    HJS_UserData userData = new HJS_UserData();
                    userData.name = HJS_FirebaseManager.Auth.CurrentUser.DisplayName;
                    userData.email = HJS_FirebaseManager.Auth.CurrentUser.Email;

                    userData.record.Reset();

                    string json = JsonUtility.ToJson(userData);
                    userDataRef.SetRawJsonValueAsync(json);
                }
                else
                {
                    Debug.Log(snapshot.Child("name").Value);
                    Debug.Log(snapshot.Child("email").Value);

                    foreach(DataSnapshot data in snapshot.Child("record").Children)
                    {
                        Debug.Log($"Record's {data} : {data.Value}");
                    }
                }

            });

    }

}
