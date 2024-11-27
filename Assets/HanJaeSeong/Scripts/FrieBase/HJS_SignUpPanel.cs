using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class HJS_SignUpPanel : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] HJS_PopupPanel popupPanel;

    [Header("Input")]
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField verifyPasswordInputField;
    [SerializeField] TMP_InputField nicknameInputField;

    DatabaseReference userDataRef;

    public void SignUp()
    {
        userDataRef = HJS_FirebaseManager.Database.RootReference.Child("UserData");

        // 빈칸이 있는지 확인
        if (CheckInput().Equals(false))
        {
            return;
        }

        // 중복이 있는지 확인
        CheckDuplicates();
    }

    private bool CheckInput()
    {
        if (emailInputField.text.IsNullOrEmpty())
        {
            popupPanel.ShowPopup("please Enter Email");
            return false;
        }
        else if (passwordInputField.text.IsNullOrEmpty() ||
            verifyPasswordInputField.text.IsNullOrEmpty())
        {
            popupPanel.ShowPopup("please Enter Password");
            return false;
        }
        else if (nicknameInputField.text.IsNullOrEmpty())
        {
            popupPanel.ShowPopup("please Enter nickname");
            return false;
        }
        else if (!passwordInputField.text.Equals(verifyPasswordInputField.text))
        {
            popupPanel.ShowPopup("Password is incorrect!");
            return false;
        }

        return true;
    }

    private void CheckDuplicates()
    {
        userDataRef.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted) Debug.LogError($"error : {task.Exception}");

                DataSnapshot snapShot = task.Result;

                // UserData의 uid를 다 불러와서
                foreach (DataSnapshot data in snapShot.Children)
                {
                    if (data.Child("email").Value.Equals(emailInputField.text)) // 어느 유저의 이메일이 있다  이미 사용죽인 이메일이다
                    {
                        popupPanel.ShowPopup("Already using Email!");
                        return;
                    }
                    else if (data.Child("name").Value.Equals(nicknameInputField.text))
                    {
                        popupPanel.ShowPopup("Already using Nickname!");
                        return;
                    }
                }

                // 회원가입 인증 메일 전송
                CreateUser();
            });
    }

    /// <summary>
    /// 회원가입을 위한 인증 메일을 전송하는 부분
    /// </summary>
    private void CreateUser()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // 회원 생성
        HJS_FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                AuthResult result = task.Result;
                UserProfile profile = new UserProfile();
                profile.DisplayName = nicknameInputField.text;

                // 닉네임 설정
                result.User.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }
                    Debug.Log("setNickName");
                });

                popupPanel.ShowPopup("Send verification your email");
                gameObject.SetActive(false);

                // 인증 메일 전송
                result.User.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SendEmailVerificationAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                        return;
                    }
                    Debug.Log("Send!");
                });

            });
    }
}
