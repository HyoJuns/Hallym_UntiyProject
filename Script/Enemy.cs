using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="New", menuName="New/enemy")]
// Create 창에 New 목록을 생성하고 그안에 enemy이라는 칸을 생성한다.
public class Enemy : ScriptableObject {
	/*
		ScriptableObject란? 독자적인 Asset 작성하기 위한 구조이다.
		유니티의 Serialize 구조를 사용하는 형식이다.
		ScriptableObject는 유니티 에디터 요소이며, 다양한 곳에서 사용되고 있다.
		씬뷰, 게임 뷰 등의 에디터 윈도우는 ScriptableObject의 자식 클래스에서 생성되었으며,
		Inspecter 에 표시하는 Editor 오브젝트도 ScriptableObject 자식 클래스에서 생성되었다.
	 */
	
	[Header("몬스터 이름")]
	public string enemyname;		// 몬스터 이름
	[Header("몬스터 타입")]
	[Tooltip("animal 동물\n monster 몬스터\n Boss 보스")]
	public enemyType enemytype;		// 몬스터 타입
	[Header("몬스터 프리팹")]
	public GameObject enemyprefab;	// 몬스터 프리팹

	/* 몬스터 기본정보 */
	[Header("몬스터 정보")]
	[Tooltip("적 채력")]
	public int hp;						// 적 몬스터
	public int maxhp;					// 적 최대 채력

	[Tooltip("적 데미지")]
	public int damge;					// 적 데미지
	[Tooltip("적 방어력")]
	public int armor;					// 방어력
	[Tooltip("이동속도")]
	public int speed;					// 이동속도
	[Tooltip("적 공격 딜레이 (float)")]
	public float delay;					// 공격속도
	[Tooltip("경험치")]
	public int exp;						// 경험치

	[Tooltip("드랍아이템")]
	public Cs_ItemPickup prefab;			// 드랍 아이템
	public Item item;						// 아이템
	
	[Tooltip("확률 (0.1 ~ 1.0)")]
	[Range(0.0f,1.0f)]
	public float random;				// 확률 
	

	public enum enemyType{
		// 몬스터 타입
		animal = 0,					// 동물 (중립)
		monster,					// 몬스터 (호전적)
		boss						// 보스
	}
	
}
