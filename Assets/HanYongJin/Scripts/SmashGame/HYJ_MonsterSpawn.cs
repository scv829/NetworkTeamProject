using Photon.Pun;
using UnityEngine;

public class HYJ_MonsterSpawn : MonoBehaviourPun
{
    //[SerializeField] GameObject bodyPrefab;
    //[SerializeField] GameObject headPrefab;
    [SerializeField] GameObject monsterPoint1;
    [SerializeField] GameObject monsterPoint2;
    [SerializeField] GameObject monsterPoint3;
    [SerializeField] GameObject monsterPoint4;

    public void MonsterSpawn()
    {
        //몬스터 바디 프리팹 생성, 자식 오브젝트로 이동
        Vector3 monsterSpawnPoint = SetMonsterPoint();
        for (int i = 0; i < 9; i++) // 생성된 바디 프리팹을 하나씩 위로 쌓기
        {
            Vector3 pos = new Vector3(0, 0.5f + (float)i, 0);
            GameObject body = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_MonsterBody", monsterSpawnPoint + pos, Quaternion.identity);
            photonView.RPC("MonsterParentSetRPC", RpcTarget.All, body.GetComponent<PhotonView>().ViewID);
        }

        //몬스터 머리 프리팹 생성, 자식 오브젝트로 이동 및 가장 위에 쌓기
        GameObject head = PhotonNetwork.Instantiate("HYJ_GameObject/HYJ_MonsterHead", monsterSpawnPoint + new Vector3(0, 9.5f, 0), Quaternion.identity);
        photonView.RPC("MonsterParentSetRPC", RpcTarget.All, head.GetComponent<PhotonView>().ViewID);
    }

    private Vector3 SetMonsterPoint() // 몬스터의 생성 지점 설정
    {
        switch (PhotonNetwork.LocalPlayer.ActorNumber) // LocalPlayer의 ActorNumber를 기준으로 몬스터 생성 지점 설정하기
        {
            case 1:
                return monsterPoint1.transform.position;
            case 2:
                return monsterPoint2.transform.position;
            case 3:
                return monsterPoint3.transform.position;
            case 4:
                return monsterPoint4.transform.position;
        }
        return Vector3.zero;
    }

    [PunRPC]
    private void MonsterParentSetRPC(int playerID) // 플레이어 ID에 따라 몬스터의 부모(위치) 설정해주기
    {
        PhotonView monsterView = PhotonView.Find(playerID);
        Debug.Log(monsterView);
        GameObject monsterParent = null;
        switch (monsterView.Owner.ActorNumber)
        {
            case 1:
                monsterParent = monsterPoint1;
                break;
            case 2:
                monsterParent = monsterPoint2;
                break;
            case 3:
                monsterParent = monsterPoint3;
                break;
            case 4:
                monsterParent = monsterPoint4;
                break;
        }
        monsterView.transform.parent = monsterParent.transform; // 몬스터의 부모 설정
    }
}
