using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class HJS_UserProfile : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] HJS_MatchView matchView;

    [Header("Spec")]
    [SerializeField, Tooltip("유저의 닉네임")] TMP_Text nicknameText;
    [SerializeField, Tooltip("유저의 소개")] TMP_Text descriptionText;
    [SerializeField, Tooltip("유저의 승률")] TMP_Text rateText;
    [SerializeField, Tooltip("유저의 전적")] TMP_Text recordText;

    private DatabaseReference userDateRef;

    private StringBuilder sb = new StringBuilder();

    /// <summary>
    /// 유저의 프로필을 불러오는 함수
    /// </summary>
    /// <param name="key">유저 식별자</param>
    public void GetUserProfile(string key = "my")
    {
        // 일단 자신의 유저를 가져고
        FirebaseUser my = HJS_FirebaseManager.Auth.CurrentUser;

        // 만약 로그인이 안됐을 때 호출은 안되게 설정
        if (my == null)
            return;
        
        // 유저 식별자를 담을 변수
        string uid;

        // 만약 유저 식별자가 my(없다)일 경우 자신의 UID 가져오기
        if (key.Equals("my")) uid = my.UserId;
        // 있으면 해당 UID를 사용
        else uid = key;

        // 데이터베이스에 요청
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
                    matchView.GetUI<TMP_InputField>("PlayerNicknameInputField").text = sb.ToString();
                    nicknameText.SetText(sb);
                    
                    sb.Clear(); sb.Append(snapshot.Child("description").Value);
                    matchView.GetUI<TMP_InputField>("PlayerDescriptionInputField").text = sb.ToString();
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
                // 모든 정보가 세팅이 되면 보여주기
                gameObject.SetActive(true);
            });
    }

    public void UpdateProfile()
    {
        // 일단 자신의 유저를 가져고
        FirebaseUser my = HJS_FirebaseManager.Auth.CurrentUser;


        // 만약 로그인이 안됐을 때 호출은 안되게 설정
        if (my == null)
            return;

        string uid = my.UserId;

        // 데이터베이스에 요청
        DatabaseReference userRef = HJS_FirebaseManager.Database.RootReference.Child("UserData");
        DatabaseReference userProfileRef = userRef.Child(uid);

        userRef.GetValueAsync()
         .ContinueWithOnMainThread(task =>
         {

             if (task.IsCanceled || task.IsFaulted) Debug.LogError($"error : {task.Exception}");

             DataSnapshot snapShot = task.Result;

             // 설명은 중복이 필요없으니 바로 저장하고
             Dictionary<string, object> dictionary = new Dictionary<string, object>();
             dictionary["/description"] = matchView.GetUI<TMP_InputField>("PlayerDescriptionInputField").text;

             // UserData의 uid를 다 불러와서
             string newNickname = matchView.GetUI<TMP_InputField>("PlayerNicknameInputField").text;

             if(!snapShot.Child(uid).Child("name").Value.Equals(newNickname) && !newNickname.IsNullOrEmpty())
             {
                 foreach (DataSnapshot data in snapShot.Children)
                 {
                     if (data.Child("name").Value.Equals(newNickname))
                     {
                         matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("Already using Nickname!");
                         return;
                     }
                 }
                 dictionary["/name"] = matchView.GetUI<TMP_InputField>("PlayerNicknameInputField").text;
             }

             userProfileRef.UpdateChildrenAsync(dictionary);

             matchView.GetUI<HJS_PopupPanel>("PopupPanel").ShowPopup("Update complete!");
         });


    }
}
