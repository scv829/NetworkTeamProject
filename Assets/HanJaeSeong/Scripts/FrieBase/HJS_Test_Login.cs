using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using UnityEngine;

public class HJS_Test_Login : MonoBehaviour
{
    private enum ID { GHS, LJH, HYJ, HJS }

    [Header("UserID")]
    [SerializeField] ID userId;

    private string email;
    private string password = "q1w2e3r4";

    private void Start()
    {
        switch (userId)
        {
            case ID.GHS:
                email = "test@test.com";
                break;
            case ID.LJH:
                email = "test2@test.com";
                break;
            case ID.HYJ:
                email = "test3@test.com";
                break;
            case ID.HJS:
                email = "test4@test.com";
                break;
        }
    }

    public void Login()
    {
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
                    return;
                }

                AuthResult result = task.Result;
                Debug.Log($"유저 로그인 성공! {result.User.DisplayName} ({result.User.UserId})");

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
                    userData.name = userId.ToString();
                    userData.email = HJS_FirebaseManager.Auth.CurrentUser.Email;

                    userData.record.Reset();

                    PhotonNetwork.LocalPlayer.NickName = userId.ToString();

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
                }

                PhotonNetwork.LocalPlayer.SetPlayerUID(uid);
            });
    }

}
