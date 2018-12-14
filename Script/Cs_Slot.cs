using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cs_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler ,IPointerClickHandler, IBeginDragHandler,IDragHandler, IEndDragHandler, IDropHandler {

	public Item item;		// 획득한 아이템
	public int itemCount;	// 획득한 아이템 갯수
	public Image itemImage;	// 아이템 이미지
	
	private Cs_ItmeEffectDB thedb;			// 아이템 정보 DB

	// 컴포넌트
	[SerializeField] private Text text_count;
	[SerializeField] private GameObject go_countimage;


	private void Start() {
		thedb = FindObjectOfType<Cs_ItmeEffectDB>();	// 이 스크립트 들어있는 존재 찾기	
	}

	// 이미지 투명도 조절
	private void SetColor(float _alpha)			// 0 ~ 1 사이 값 투명도 조절(1 불투명도)
	{
		Color color = itemImage.color;			// 아이템 이미지를 투명도를 조절시킨다.
		color.a = _alpha;						// 입력된 알파값으로 투명도 조절
		itemImage.color = color;				// 알파값 이미지 컬러에 조절
	}

	// 아이템을 획득
	public void AddItem(Item _item, int _count = 1)
	{
		// 아이템창에 적용
		item = _item;						
		itemCount = _count;
		itemImage.sprite = item.itemImg;	

		if(item.itemtype != Item.ItemType.Equipment)
		{
			// 장비가 아닌 경우
			go_countimage.SetActive(true);
			text_count.text = itemCount.ToString();
		}
		else{
			text_count.text = "0";
			go_countimage.SetActive(false);
		}
		SetColor(1);	// 투명도 설정
	}

	// 아이템 갯수 조정.
	public void SetSlotCount(int _count)
	{
		itemCount += _count;
		text_count.text = itemCount.ToString();

		if(itemCount <= 0)	ClearSlot();

	}

	// 슬롯 초기화
	private void ClearSlot()
	{
		item = null;
		itemCount = 0;
		itemImage.sprite = null;
		SetColor(0);	// 투명

		text_count.text = "0";
		go_countimage.SetActive(false);
	}

	// 아이템을 클릭할 경우 (우클릭)
	public void OnPointerClick(PointerEventData eventdata)
	{
		if(eventdata.button == PointerEventData.InputButton.Right)
		{
			if(item != null)
			{
				if(item.itemtype == Item.ItemType.Used){
					if(item.name.Equals( "ItemBox_02"))		// Water
						Cs_SoundManager.instance.PlaySE("se_water");
					else
					Cs_SoundManager.instance.PlaySE("se_Itemeat");

					Debug.Log(item.name);
					thedb.UseItme(item);
					SetSlotCount (-1);
					thedb.HideToolTip();
				}else{
					Cs_SoundManager.instance.PlaySE("se_select");
					SetSlotCount (-1);
					Player.Cs_PlayerStatus.Point_Score += item.score;
					thedb.HideToolTip();
				}
			}
			
		}
	}

	

	// 아이템 창 칸에 드래그를 한경우
	public void OnBeginDrag(PointerEventData eventData)
	{
		if(item != null)
		{	// 아이템이 비어있지 않은 경우
			Cs_DragSlot.instance.dragSlot = this;
			Cs_DragSlot.instance.DragSetImage(itemImage);
			
			Cs_DragSlot.instance.transform.position = eventData.position;
		}
	}

	// 드래그 중
	public void OnDrag(PointerEventData eventData)
	{
		if(item != null)
		{
			Cs_DragSlot.instance.transform.position = eventData.position;
		}
	}

	// 드래그 끝
	public void OnEndDrag(PointerEventData eventData)
	{
		Cs_DragSlot.instance.SetColor(0);
		Cs_DragSlot.instance.dragSlot =null;
	}

	// 
	public void OnDrop(PointerEventData eventData)
	{
		if(Cs_DragSlot.instance.dragSlot != null)
		{
			ChangeSlot();
		}
	}


	private void ChangeSlot()
	{
		Item _tempItem = item;
		int _tempitemcount = itemCount;

		AddItem(Cs_DragSlot.instance.dragSlot.item,Cs_DragSlot.instance.dragSlot.itemCount);	// 아이템을 추가한다.
	
		if(_tempItem != null)
			Cs_DragSlot.instance.dragSlot.AddItem(_tempItem, _tempitemcount);
		else
			Cs_DragSlot.instance.dragSlot.ClearSlot();
	}


	// 마우스가 슬롯에 들어갈 때 발동
    public void OnPointerEnter(PointerEventData eventData)
    {
		if (item != null)
    		thedb.ShowToolTip(item);	
    }

	// 슬롯에 빠져나갈 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        thedb.HideToolTip();
    }
}
