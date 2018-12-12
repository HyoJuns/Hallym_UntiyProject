using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Inventory : MonoBehaviour {
	public static bool inventoryActivated = false;			// 인벤토리 활성화

	// 필요 컴포넌트
	[SerializeField] private GameObject go_inventorybase;	// UI로 만든 인벤토리 창
	[SerializeField] private GameObject go_slotparent;		// 슬롯 부모 

	// 슬롯들
	private Cs_Slot[] slots;

	// Use this for initialization
	void Start () {
			slots = go_slotparent.GetComponentsInChildren<Cs_Slot>();
			// Cs_slot 있는 아들객체들을 배열형태로 컴퍼넌트값을 넣는다.
			
	}
	
	void Update () {
		TryOpenInventory();				// 인벤토리 창 열기
	}

	/// <summary>
	/// 인벤토리 창을 여는 함수
	/// </summary>
	private void TryOpenInventory()
	{
		// I 키 누를 경우
		if(Input.GetKeyDown(KeyCode.I)){
			inventoryActivated = !inventoryActivated;		// 활성화 and 비활성화

			if(inventoryActivated){
				OpenInventory();
				WeaponList.Cs_WeaponControl.waitfire = true;
			}
			else{
				WeaponList.Cs_WeaponControl.waitfire = false;
				CloseInventory();
			}
		}
	}

	/// <summary>
	/// 인벤토리 오픈
	/// </summary>
	private void OpenInventory()
	{
		go_inventorybase.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}

	/// <summary>
	/// 인벤토리 닫기
	/// </summary>
	private void CloseInventory()
	{
		
		go_inventorybase.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary>
	/// 인벤토리 창에 아이템을 추가시킨다.
	/// 장비템인 경우 (장비는 1개씩, 나머지 기타템은 숫자 증가)
	/// </summary>
	/// <param name="_item"></param>
	/// <param name="_count"></param>
	public void AcquireItem(Item _item, int _count = 1)
	{
		if(Item.ItemType.Equipment != _item.itemtype)	
		{	// 장비템이 아닌 경우  (장비템인 경우 빈 자리에 넣는다.)
			for(int i = 0 ; i < slots.Length; i++)
			{
				if(slots[i].item != null)
				{
					// 만약 슬롯이 비어있지 않은 경우
					if(slots[i].item.itemName == _item.itemName)
					{
						slots[i].SetSlotCount(_count);
						return;
					} 

				}
			}
		}	// end if

		// 아이템이 없으면 빈자리에 놓는다.
		for (int j = 0; j < slots.Length; j++)
		{
			if(slots[j].item == null)
			{
				slots[j].AddItem(_item,_count);
				return;
			}
		}
	}

}
