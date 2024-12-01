using Fusion;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJS_ShowPanel : MonoBehaviour
{
    [SerializeField] HJS_MatchView matchView;
    private void OnTriggerEnter(Collider other)
    {        
        // 플레이어의 충돌만 확인할 건데
        if (other.transform.CompareTag("Player"))
        {
            // 움직인 캐릭터의 소유자가 아닐 경우 보여줄 필요가 없다.
            if (other.transform.GetComponent<NetworkBehaviour>().HasStateAuthority == false) return;

            matchView.GetUI("RandomMatchPanel").SetActive(true);
        }
    }
}
