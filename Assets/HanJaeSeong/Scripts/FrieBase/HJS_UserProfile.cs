using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HJS_UserProfile : MonoBehaviour
{
    [SerializeField, Tooltip("유저의 닉네임")] TMP_Text nicknameText;
    [SerializeField, Tooltip("유저의 소개")] TMP_Text descriptionText;
    [SerializeField, Tooltip("유저의 승률")] TMP_Text rateText;
    [SerializeField, Tooltip("유저의 전적")] TMP_Text recordText;

    DatabaseReference userDateRef;

    private StringBuilder sb = new StringBuilder();

    private void OnEnable()
    { 
        FirebaseUser my = HJS_FirebaseManager.Auth.CurrentUser;
        if (my == null)
            return;

        string uid = my.UserId;
        userDateRef = HJS_FirebaseManager.Database.RootReference.Child("UserData").Child(uid);

        // DB에서 정보를 불러와서 할당하는 내용
        userDateRef.GetValueAsync()
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

                Debug.Log("유저 프로필 가져오기 성공!");
                DataSnapshot snapshot = task.Result;

                if (snapshot.Value is not null)
                {
                    double rate = 0;

                    sb.Clear();sb.Append(snapshot.Child("name").Value);
                    nicknameText.SetText(sb);
                    sb.Clear(); sb.Append(snapshot.Child("description").Value);
                    descriptionText.SetText(sb);

                    sb.Clear(); 
                    
                    sb.Append($"Total: {snapshot.Child("record").Child("total").Value} ");
                    sb.Append($"Win: {snapshot.Child("record").Child("win").Value} ");
                    sb.Append($"Lose: {snapshot.Child("record").Child("lose").Value} ");
                    recordText.SetText(sb);

                    sb.Clear();

                    double winCount = (long)snapshot.Child("record").Child("win").Value;
                    double totalCount = (long)snapshot.Child("record").Child("total").Value;

                    rate = Math.Round(winCount * 100 / totalCount, 1);
                    sb.Append($"PlayerRate\n{rate}%");
                    rateText.SetText(sb);
                }

            });
    
    }



}
