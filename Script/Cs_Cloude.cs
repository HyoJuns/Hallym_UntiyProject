using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Cloude : MonoBehaviour {

	public float turn = 0.0f;		// 구름 움직이기 위한 변수
	GameObject cam;					// 카메라 (SkyBox)
	public Material day;			// 아침
	public Material afternoon;		// 점심
	public Material night;			// 저녁 (밤)

	[SerializeField] private float fogdup;				// 포그 증가량
	[SerializeField] private float fogcurr;				// 현재 포그량
	[SerializeField] private float nightFogDensity;		// 밤 상태 포그 밀도
	[SerializeField] private float dayFogDensity;		// 낯 상태 포그

	void Awake()
	{
		cam = GameObject.Find("Main Camera");
		Debug.Log("cam : " + cam.ToString());
	}

	void Start()
	{
		cam.GetComponent<Skybox>().material = day;
		dayFogDensity = RenderSettings.fogDensity;
		fogcurr = RenderSettings.fogDensity;
	}

	void Update () {
		turn += Time.deltaTime;		// Rotation
		GameObject obj = GameObject.FindGameObjectWithTag("sun");	// 태양
		
		if( turn >= 359.0f)
		{
			turn = 0.0f;
			transform.rotation = Quaternion.Euler(0f,0f,0f);
			obj.transform.rotation = Quaternion.Euler(0f,0f,0f);
			Player.Cs_PlayerStatus.status_Day ++;
		}

		transform.localEulerAngles = new Vector3(0f,turn,0f);	// 구름 회전
		obj.transform.localEulerAngles = new Vector3(turn,0f,0f);	// 태양 회전

		ChangeSkybox(turn);

	}

	// SKyBox 변경
	void ChangeSkybox(float n)
	{
		// 밤 낯 일때 밝기 조절
		if (n <= 30.0f && n > -0.5f){
			if(fogcurr >= dayFogDensity)
			{
				fogcurr -= 0.2f * fogdup * Time.deltaTime;
				RenderSettings.fogDensity = fogcurr;
			}
			cam.GetComponent<Skybox>().material = day;
		}
		else if(n <= 140.0f && n > 30.0f){
			if(fogcurr >= dayFogDensity)
			{
				fogcurr -= 0.2f * fogdup * Time.deltaTime;
				RenderSettings.fogDensity = fogcurr;
			}
			cam.GetComponent<Skybox>().material = afternoon;
		}
		else if(n <= 360.0f && n > 140.1f){
			if(fogcurr <= nightFogDensity)
			{
				fogcurr += 0.1f * fogdup * Time.deltaTime;
				RenderSettings.fogDensity = fogcurr;
			}
			cam.GetComponent<Skybox>().material = night;
		}
	}
}
