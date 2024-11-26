using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HJS_LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;

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
                FirebaseUser user = HJS_FirebaseManager.Auth.CurrentUser;
                //Uri uri = user.PhotoUrl;         // 사진 URL 주소
                //Debug.Log(uri.ToString());
                //CheckUserInfo();
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
                    UserData userData = new UserData();
                    userData.name = HJS_FirebaseManager.Auth.CurrentUser.DisplayName;
                    userData.email = HJS_FirebaseManager.Auth.CurrentUser.Email;

                    string json = JsonUtility.ToJson(userData);
                    userDataRef.SetRawJsonValueAsync(json);
                }
                else
                {
                    Debug.Log(snapshot.Child("name").Value);
                    Debug.Log(snapshot.Child("email").Value);
                }

            });

    }

    [Serializable]
    public class UserData
    {
        public string name;
        public string email;
    }
}
