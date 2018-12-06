using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="NewItem", menuName = "NewItem/item")]
// Create 창에 NewItem 목록을 생성하고 그안에 item이라는 칸을 생성한다.
public class Item : ScriptableObject {
	/*
		ScriptableObject란? 독자적인 Asset 작성하기 위한 구조이다.
		유니티의 Serialize 구조를 사용하는 형식이다.
		ScriptableObject는 유니티 에디터 요소이며, 다양한 곳에서 사용되고 있다.
		씬뷰, 게임 뷰 등의 에디터 윈도우는 ScriptableObject의 자식 클래스에서 생성되었으며,
		Inspecter 에 표시하는 Editor 오브젝트도 ScriptableObject 자식 클래스에서 생성되었다.
	 */
	
	public string itemName;			// 아이템 이름
	public ItemType itemtype;		// 아이템 타입
	public Sprite itemImg;			// 아이템 이미지
	public GameObject itemPrefab;	// 아이템 프리팹

	public string weaponType;		// 무기 유형

	public enum ItemType	// 타입
	{
		Equipment,			// 장비
		Used,				// 소비
		Ingredient,			// 재료
		ETC					// 기타
	}
}
