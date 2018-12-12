using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	// UI
using TMPro;			// 텍스트 mash 임포트

public class Cs_SlotTooltip : MonoBehaviour {

	[SerializeField]
	private GameObject go_Base;		// 툴팁 베이스

	// 아이템 설명
	[SerializeField] private TextMeshProUGUI txt_ItemName;
	[SerializeField] private TextMeshProUGUI txt_ItemInfo;
	[SerializeField] private TextMeshProUGUI txt_ItemHowtoUsed;

	
	public void ShowTooltip(Item _item)
	{
		go_Base.SetActive(true);

		txt_ItemName.text = _item.itemName;
		txt_ItemInfo.text = _item.itmeinfo;
		
		if(_item.itemtype == Item.ItemType.Equipment)
		{
			// 장착
			txt_ItemHowtoUsed.text = "Mouse Right_Click = Equip";
		}else if(_item.itemtype == Item.ItemType.Used)
		{
			// 먹기
			txt_ItemHowtoUsed.text = "Mouse Right_Click = Eat";
		}
		else
			txt_ItemHowtoUsed.text = "";
	}

	// 숨기기
	public void HideTooltip()
	{
		go_Base.SetActive(false);	// 숨기기
	}
}
