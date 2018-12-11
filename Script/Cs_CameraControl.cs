using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_CameraControl : MonoBehaviour {
	
	[Header("목표물")]
	public Transform target;		// 카메라가 따라올 객체
	public float smoothing = 5f;	// 카메라 속도

	Vector3 offset;					// 타겟 초기 offset

	void Start () {
		offset = transform.position - target.position;	// 카메라 위치와 타겟 위치의 간격
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// 카메라의 위치를 새로 지정
		Vector3 targetCamPos = target.position + offset;

		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.smoothDeltaTime);
	}
}
