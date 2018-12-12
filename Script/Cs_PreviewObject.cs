using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_PreviewObject : MonoBehaviour {

	// 충돌한 오브젝트의 콜라이더를 저장하는 리스트 변수
	private List<Collider> colllist = new List<Collider>();

	[SerializeField]
	private int layerGround;					// 지상 레이어
	private const int IGNORE_RAYCAST_LAYER = 2;	// 2번 레이어 번호 (충돌해도 colllist에 추가 안함)
	
	[SerializeField]
	private Material Green;	// 건설가능 색상
	[SerializeField]
	private Material Red;	// 건설불가 색상

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ChangeColor();
	}
	void ChangeColor()
	{
		if(colllist.Count > 0)
		{
			// 레드
			SetColor(Red);	// 건설불가지역]
			
		}else{
			// 초록
			SetColor(Green); // 건설가능지역
		}
	}

	private void SetColor (Material mat)
	{
		// 자기자신 스크립트가 붙어있는 트랜스폼 안에 있는 다른 객체의 트랜스폼을 가져와 반복문 돌린다.
		// 즉 자식들의 객체를 가져와서 트랜스폼 값을 가져온다.
		foreach(Transform tf_Child in this.transform)
		{
			var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

			// 자식객체를 전부 색상 변경
			for(int i = 0 ; i < newMaterials.Length; i++)
			{
				newMaterials[i] = mat;
			}
			
			tf_Child.GetComponent<Renderer>().materials = newMaterials;
		}
	}

	// 들어가면 추가
	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
		colllist.Add(other);	// 콜라이더를 추가시킨다.
	}
	// 들어가지 않으면 제거 
	private void OnTriggerEXit(Collider other) {
		if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
		colllist.Remove(other);
	}


	public bool isBuildable()
	{
		return colllist.Count == 0;
	}
}
