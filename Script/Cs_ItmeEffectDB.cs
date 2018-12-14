using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
	[Header("아이템 정보")]
	[Tooltip("키값")] public string itemName;		// 아이템 이름 (키값)
	[Tooltip("어느 부위")] public string[] part;			// 부위
	[Tooltip("수치값")] public int[] num;				// 수치

}

public class Cs_ItmeEffectDB : MonoBehaviour {

	[SerializeField]
	[Tooltip("HP 회복, SP 기력, ATK 공격력 \n THIRSTU 목마름 HUNGRY 배고픔 ARM 방어력 \n CORE 에너지무기, BULLET 탄환무기")]
	private ItemEffect[] itemEffect;

	// 상수 선언
	private const string HP = "HP", ATK ="ATK", THIRSTY="THIRSTU", BULLET = "BULLET";
	private const string SP = "SP", HUNGRY = "HUNGRY", ARM = "ARM", CORE = "CORE";

	// 컴퍼넌트
	[SerializeField]
	private Player.Cs_PlayerStatus status;
	[SerializeField]
	private WeaponList.Weapon weapon;
	[SerializeField]
	private Cs_SlotTooltip theslottooltip;

	public void ShowToolTip(Item _item)
	{
		theslottooltip.ShowTooltip(_item);
	}
	public void HideToolTip()
	{
		theslottooltip.HideTooltip();
	}

	public void UseItme(Item _item)
	{
		if(_item.itemtype == Item.ItemType.Used)
		{
			// 소모품일경우
			for(int x =0; x < itemEffect.Length; x++)
			{
				if(itemEffect[x].itemName == _item.itemName)	// 키 값이 일치하냐?
				{
					for(int y = 0; y <itemEffect[x].part.Length; y++)
					{
						// 부의 마다 다르니 배열로 회복
						switch(itemEffect[x].part[y])
						{
							case HP:		// 회복
								status.Heal_HP(itemEffect[x].num[y]);
							break;
							case SP:		// 기력
								status.Heal_SP(itemEffect[x].num[y]);
							break;
							case ATK:		// 공격력
								status.Up_attack(itemEffect[x].num[y]);
							break;
							case HUNGRY:	// 배고픔
								status.Up_Hungry(itemEffect[x].num[y]);
							break;
							case THIRSTY:	// 목마름
								status.Up_Thirsty(itemEffect[x].num[y]);
							break;
							case ARM:		// 방어력
								status.Up_armor(itemEffect[x].num[y]);
							break;
							case CORE:		// 레이저빔
								weapon.AddLaser( itemEffect[x].num[y]);
								PlayerPrefs.SetInt("maxlaser",itemEffect[x].num[y]);		// HUD
							break;
							case BULLET:		// 총알
								weapon.Addbullet( itemEffect[x].num[y]);
								
							break;

							default:
								Debug.Log("잘못된 Status 부위.");
								break;
						}
						
					}
					
					return;
				}
			}
			Debug.Log("ItemEffectDB에 일치한 itemName이 없습니다.");
		}
	}
}
