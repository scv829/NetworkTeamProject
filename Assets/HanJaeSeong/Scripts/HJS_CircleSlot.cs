using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HJS_CircleSlot : MonoBehaviour
{
    private enum Slot { Heart, Clover, Spade, Diamond, Empty, Size }

    [SerializeField] Sprite[] slotSprites;  // 슬롯들의 스프라이트 배열
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
        curSlot = (Slot)index;
        Debug.Log($"{gameObject.name} : {curSlot}");
        slotImage.sprite = slotSprites[index];
    }
}
