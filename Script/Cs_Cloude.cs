using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Cloude : MonoBehaviour {

	public float turn = 360.0f;		// 구름 움직이기 위한 변수
	GameObject cam;					// 카메라 (SkyBox)
	public Material day;			// 아침
	public Material afternoon;		// 점심
	public Material night;			// 저녁 (밤)

	void Awake()
	{
		cam = GameObject.Find("Main Camera");
		Debug.Log("cam : " + cam.ToString());
	}

	void Start()
	{
		cam.GetComponent<Skybox>().material = day;
	}

	void Update () {
		turn -= Time.deltaTime;		// Rotation
		GameObject obj = GameObject.FindGameObjectWithTag("sun");	// 태양
		
		if( turn <= 1.0f)
		{
			turn = 360.0f;
			transform.rotation = Quaternion.Euler(0f,360f,0f);
			obj.transform.rotation = Quaternion.Euler(360f,0f,0f);
		}

		transform.localEulerAngles = new Vector3(0f,turn,0f);	// 구름 회전
		obj.transform.localEulerAngles = new Vector3(turn,0f,0f);	// 태양 회전

		ChangeSkybox(turn);

	}

	// SKyBox 변경
	void ChangeSkybox(float n)
	{
		if (n <= 360.0f && n > 270.0f)
			cam.GetComponent<Skybox>().material = day;
		else if(n <= 270.0f && n > 120.0f)
			cam.GetComponent<Skybox>().material = afternoon;
		else if(n <= 120.0f && n > 0.1f)
			cam.GetComponent<Skybox>().material = night;
	}
}
