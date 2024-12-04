using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HJS_GameConnect : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] HJS_FadeController fadeController;

    [SerializeField] NetworkSceneInfo info;

    // 플레이어가 들어왔을 때
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            //Runner.Spawn(PlayerPrefab, spawnPoint.position, Quaternion.identity);
            Runner.Spawn(PlayerPrefab);
            Runner.StartCoroutine(FadeRoutine());
        }
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(3f);

        fadeController.FadeIn();
    }

    private void Start()
    {
        Debug.Log("login init");
        Init();
    }

    private void Init()
    {
        StartGameArgs args = new StartGameArgs();
        args.GameMode = GameMode.Shared;
        args.SessionName = "KGA-c8045daa-c92e-4fba-8299-527d770cf5ea";
        args.CustomLobbyName = "LobbyRoom";

        transform.GetComponent<NetworkRunner>().StartGame(args);
    }

}
