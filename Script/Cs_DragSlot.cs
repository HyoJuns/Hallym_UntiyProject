using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cs_DragSlot : MonoBehaviour {

	static public Cs_DragSlot instance;		// 인스턴스 개체
	public Cs_Slot dragSlot;				// 드래그 할 슬롯

	// 아이템 이미지
	[SerializeField]
	private Image imgitem;					// 드래그할때 마우스 따라가는 이미지

	// Use this for initialization
	void Start () {
		instance = this;					// 자신 컴퍼넌트 대입
	}
	
	// 드래그 할 이미지 선택
	public void DragSetImage(Image _itemimg)
	{
		imgitem.sprite = _itemimg.sprite;
		SetColor(1);
	}

	// 투명도 설정
	public void SetColor(float _alpha)
	{
		Color color = imgitem.color;
		color.a = _alpha;
		imgitem.color = color;
	}
}
