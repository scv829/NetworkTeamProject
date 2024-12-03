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
        StartGameArgs args = new StartGameArgs();
        args.GameMode = GameMode.Shared;
        args.SessionName = "kr";
        args.CustomLobbyName = "Lobbys";

    //   Debug.Log("scene init");
    //   var scene = SceneRef.FromIndex(9);
    //
    //   Debug.Log($"scene after {scene}");
    //
    //   Debug.Log("sceneinfo init");
    //   var sceneInfo = new NetworkSceneInfo();
    //
    //   if (scene.IsValid)
    //   {
    //   Debug.Log("sceneinfo true");
    //       sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
    //   }
    //   Debug.Log("sceneinfo after");
    //
    //   //args.Scene = scene;

        Debug.Log("hi");


        NetworkRunner runner = FindObjectOfType<NetworkRunner>();
        runner.StartGame(args);
    }

}
