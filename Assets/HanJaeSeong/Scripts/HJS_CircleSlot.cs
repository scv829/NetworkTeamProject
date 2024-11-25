using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HJS_CircleSlot : MonoBehaviourPun
{
    private enum Slot { Heart, Clover, Spade, Diamond, Empty, Size }

    [Header("Heart")]
    [SerializeField] Sprite[] heartSprites;  // 슬롯들의 스프라이트 배열
    [Header("Clover")]
    [SerializeField] Sprite[] cloverSprites;  // 슬롯들의 스프라이트 배열
    [Header("Spade")]
    [SerializeField] Sprite[] spadeSprites;  // 슬롯들의 스프라이트 배열
    [Header("Diamond")]
    [SerializeField] Sprite[] diamondSprites;  // 슬롯들의 스프라이트 배열
    [Space]
    [SerializeField] Image slotImage;       // 스프라이트를 표시할 이미지
    [SerializeField] Slot curSlot;          // 현재 슬롯의 상태

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    /// <summary>
    /// 슬롯 설정 함수
    /// </summary>
    /// <param name="index">슬롯의 번호</param>
    public void SetSlot(int index)
    {
        photonView.RPC("SlotSettingRPC", RpcTarget.All, index, Random.Range(0, 4));
    }

    [PunRPC]
    private void SlotSettingRPC(int index, int color)
    {
        curSlot = (Slot)index;
        switch (index)
        {
            case (int)Slot.Heart:
                slotImage.sprite = heartSprites[color];
                break;
            case (int)Slot.Clover:
                slotImage.sprite = cloverSprites[color];
                break;
            case (int)Slot.Spade: 
                slotImage.sprite = spadeSprites[color];
                break;
            case (int)Slot.Diamond: 
                slotImage.sprite = diamondSprites[color];
                break;
            default:
                slotImage.sprite = null;
                break;
        }
    }
}
