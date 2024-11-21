using Photon.Pun;
using UnityEngine;

public class KHS_HeyHoController : MonoBehaviourPun
{
    [SerializeField] private KHS_MechaMarathonGameManager _mechaMarathonGameManager;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isMoved;
    [SerializeField] private bool _isTargeted;
    [SerializeField] private Vector3 targetPos;


    private void Awake()
    {
        _mechaMarathonGameManager = FindAnyObjectByType<KHS_MechaMarathonGameManager>();
    }

    private void Start()
    {
        ReadyHeyHo();

    }

    private void Update()
    {
        if (_mechaMarathonGameManager.IsFinished == true && _isMoved == false && photonView.IsMine)
        {
            MoveHeyHo(_mechaMarathonGameManager._totalCount[photonView.Owner.ActorNumber]);
            Debug.Log("헤이호 움직이는중");
        }
        else if (_isMoved == true)
        {
            return;
        }
    }

    public void MoveHeyHo(int count)
    {
        if (_isTargeted == false)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + count);
            Debug.Log($"현재 타겟 위치 {targetPos}");
            _isTargeted = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        if (transform.position.z >= targetPos.z && _isMoved == false)
        {
            _isMoved = true; // 목표 위치에 도달했음을 표시
        }
    }

    private void ReadyHeyHo()
    {
        photonView.RPC("ReadyHeyHoRPC", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ReadyHeyHoRPC()
    {
        _mechaMarathonGameManager.HeyHoController[photonView.Owner.ActorNumber] = this;
        Debug.Log($"{PhotonNetwork.LocalPlayer.ActorNumber}번째 헤이호");
    }
}
