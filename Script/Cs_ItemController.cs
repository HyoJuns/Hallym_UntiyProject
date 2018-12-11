using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cs_ItemController : MonoBehaviour {

	[SerializeField] private float range;				// 습득 가능 최대거리
	[SerializeField] private GameObject go_slotparent;	// 슬롯 부모 
	private Cs_Slot[] slots;							// 슬롯들
	private bool pickupActive = false;					// 습득 가능시 True
	private RaycastHit hitinfo;							// 충돌체 정보 저장

	public Transform prefab_open;						// 상자 여는 파티클

	// Item Layer 반응하기 위해 레이어 마스크 설정
	/*
		레이어 마스크는 특정한 레이어로 지정된 오브젝트만 카메라에 노출되도록 포함시키거나, 반대로 노출에서 제외되도록
		컬링 마스크를 설정할 때 주로 사용한다.
		이것은 게임 오브젝트의 골라서 랜더링하는 것이 가능하다는 의미이다.

	 */
	[SerializeField] private LayerMask layermask;		// Item 레이어마스크 
	
	// 필요 컴포넌트
	[SerializeField] private Text actionText;			// 행동 취할 때 나오는 텍스트
	[SerializeField] private Cs_Inventory myinventory;	// 내 인벤토리창
	
	Animator anim;								// 선물상자 애니메이션

	// 사운드
	private string se_drop = "se_drop";

	void Start() {
		slots = go_slotparent.GetComponentsInChildren<Cs_Slot>();
		
	}

	void Update () {
		CheckItem();	// 아이템 체크
		TryAction();	// 줍기

	
		
	}

	// 줍기 버튼 클릭시 
	private void TryAction()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("E");
			// 줍기 버튼 클릭시
			CheckItem();
			CanPickUp();
		}
	}

	// 아이템 먹기
	private void CanPickUp()
	{
		if(pickupActive)
		{
			if(hitinfo.transform != null)
			{
				Cs_SoundManager.instance.PlaySE(se_drop);		// 재생
				anim = hitinfo.transform.GetComponent<Animator>();
				Debug.Log(hitinfo.transform.GetComponent<Cs_ItemPickup>().item.itemName);		// 아이템 습득 로그 표시
				myinventory.AcquireItem(hitinfo.transform.GetComponent<Cs_ItemPickup>().item);
				Instantiate(prefab_open,hitinfo.transform.position, hitinfo.transform.rotation);		// 프리팹 생성
				if(anim != null)
					anim.SetTrigger("is_open");
				Destroy(hitinfo.transform.gameObject, 0.5f);		// 먹었으니 땅에 떨어져 있는건 제거
				
				

				ItemInfoDis();
			}
		}
	}

	// 아이템 체크 
	private void CheckItem()
	{
		bool is_full = true;			// 아이템창 칸이 가득 찰 경우
		// 해당 위치에서 (월드좌표)로 앞쪽으로 범위 만큼 광선을 뿜어준다.
		//Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward),Color.blue,range);
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),out hitinfo, range,layermask))
		{
			// 광선으로 쏜 물체의 태그가 Item 일 경우
			if(hitinfo.transform.tag == "Item")
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if(slots[i].item == null){ is_full = false; break;}
				}

				if(!is_full)
					ItemInfoApp();			// 먹기 정보 활성화
				else
					ItemInfoFull();			// 가득찼다고 한다.
			}
		}else
			ItemInfoDis();				// 먹기 정보 숨기기
	}

	// 아이템 정보 획득 라벨 출력
	private void ItemInfoApp()
	{
		pickupActive = true;						// 먹기 활성화
		actionText.gameObject.SetActive (true);		// 아이템을 먹은 걸 텍스트로 표시
		actionText.text = hitinfo.transform.GetComponent<Cs_ItemPickup>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
	}

	private void ItemInfoDis()
	{
		pickupActive = false;						// 먹기 비활성화
		actionText.gameObject.SetActive(false); 	// 숨기기
	}

	private void ItemInfoFull()
	{
		pickupActive = false;
		actionText.gameObject.SetActive (true);
		actionText.text = " 아이템창이 " + "<color=red>" + "FULL" + "</color>" + "입니다.";
	}

}
