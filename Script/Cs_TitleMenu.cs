﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cs_TitleMenu : MonoBehaviour {
	// 이 스크립트는 타이틀 화면에 대한 제어 장치 스크립트다.
	// 해당 오브젝트를 숨기고 활성화 시키는 역활을 한다.
	// CameraObject.SetBool ("option_clk", false); 이것은 에니메이터 실행 함수

	// 변수 선언.
	public GameObject Background_Title;					// Title_BackGround 오브젝트.
	public GameObject Background_Title2;				// Title_BackGround_option 오브젝트
	public GameObject Background_Title3;				// Title_BackGround_option_List 오브젝트

	public GameObject Title_menu1;						// Start Menu 오브젝트
	public GameObject Title_menu2;						// Option Menu 오브젝트

	public GameObject option_menu1;						// Option_Game
	public GameObject option_menu2;						// Option_Control
	public GameObject option_menu3;						// Option_Video

	public GameObject newgame_menu;						// 새로운 게임 오브젝트

	public GameObject nova;								// 비행기

	public GameObject Loding_menu;						// 로딩메뉴		
	public GameObject Loding_Manager;					// 로딩메니저
	private Cs_Loding cs_loding;						// CS_Loding
	// 위에 관련된 서브 변수
	public Text difficulty_selet;					// 난이도 선택시 변화되는 텍스트
	public Text difficulty_newgame_txt;				// NewGame 버튼 누르면 나타나는 텍스트
	public static int difficulty_number = 0;		// 난이도 선택시 오르는 버튼 (0-기본,1-이지,2-노멀,3-하드).
	private string difficulty_name = null;			// 난이도 이름 표시

	// 사운드
	private string bgm = "bgm_title";				// 타이틀 곡
	private string se = "se_select";					// 메뉴 선택 브금

	// 슬라이더
	public Slider bgm_volume;			// 배경 볼륨
	public Slider se_volume;			// 사운드 볼륨
	public Slider ls;					// 마우스 민감도 슬라이더

	// 함수 선언.
	void Awake()										// Start 보다 먼저 시작.
	{
		// UI 오브젝트를 찾아서 각 변수에 대입
		Background_Title = GameObject.Find("Title_BackGround") as GameObject;
		Background_Title2 = GameObject.Find("Title_BackGround_option") as GameObject;
		Background_Title3 = GameObject.Find("Title_BackGround_Option_List") as GameObject;
		Debug.Log("BackGround Loading Complete");

		Title_menu1 = GameObject.Find("Title_menu1(Startmenu)") as GameObject;
		Title_menu2 = GameObject.Find("Title_menu2(Optionmenu)") as GameObject;
		Debug.Log("Title menu Loading Complete");

		option_menu1 = GameObject.Find("Title_menu3(Game)") as GameObject;
		option_menu2 = GameObject.Find("Title_menu3(Control)") as GameObject;
		option_menu3 = GameObject.Find("Title_menu3(Video)") as GameObject;
		newgame_menu = GameObject.Find("Title_menu4(NewGame)") as GameObject;
		Debug.Log("Option Loading Complete");

		cs_loding = Loding_Manager.GetComponent<Cs_Loding>();
		
	}
	
	void Start()										// Awake 완료 후 시작.
	{
		// 초기화
		Title_menu_screen(true);
		Option_menu_screen(false);
		Option_menu_list_Game(false);
		Option_menu_list_Control(false);
		Option_menu_list_Video(false);
		New_Game_screen(false);
		Option_Game_Difficulty(0);						// 기본 난이도 설정
		nova.SetActive(true);
		HideLoding();
		// 불러오기
		bgm_volume.value = PlayerPrefs.GetFloat("BGMV",1.0f);
		se_volume.value = PlayerPrefs.GetFloat("SEV",1.0f);
		ls.value = PlayerPrefs.GetFloat("LSV",0.0f);

		// 초기화
		SE_V();
		BGM_V();
		LS_SAVE();
		Cs_SoundManager.instance.PlayBGM(bgm);			// 사운드 재생
		Cs_Difficulty.instance.Debug_DIFF = 1;
	}


	// 로딩 화면 표시 & 숨기기
	public void ShowLoding(){
		Loding_menu.SetActive(true);
	}
	public void HideLoding(){
		Loding_menu.SetActive(false);
	}


	
	public void Title_menu_screen(bool n)				// 타이틀 시작화면을 보여주거나 비활성화 해주는 함수
	{
		Background_Title.gameObject.SetActive(n);
		Title_menu1.gameObject.SetActive(n);
		Debug.Log("Menu Screen 작동");
		
	}

	public void Option_menu_screen(bool n)				// 옵션 화면 보여주거나 비활성화 하는 함수
	{
		
		Background_Title2.gameObject.SetActive(n);
		Title_menu2.gameObject.SetActive(n);
		Debug.Log("Option Screen 작동");
		
	}

	public void New_Game_screen(bool n)					// 새로운 게임 시작 화면을 활성/비활성 하는 함수
	{
		Background_Title3.gameObject.SetActive(n);
		if (n == true) {difficulty_newgame_txt.text = "난이도 : " + difficulty_name;}
		newgame_menu.gameObject.SetActive(n);
		

	}
	// 옵션 내용 화면
	public void Option_menu_list_Game(bool n)			// Game 옵션
	{
		Background_Title3.gameObject.SetActive(n);
		option_menu1.gameObject.SetActive(n);
		Debug.Log("Game Option 작동");
		
	}
	public void Option_menu_list_Control(bool n)		// Control 옵션
	{
		Background_Title3.gameObject.SetActive(n);
		option_menu2.gameObject.SetActive(n);
		Debug.Log("Control Option 작동");
		
	}
	public void Option_menu_list_Video(bool n)			// Video 옵션
	{
		Background_Title3.gameObject.SetActive(n);
		option_menu3.gameObject.SetActive(n);
		Debug.Log("Video Option 작동");
		
	}

	// 옵션 리스트 관련 함수
	public void Option_Game_Difficulty(int n)			// 난이도 버튼을 누를 때 발동
	{
		switch(n)
		{
			case 1: 			// Easy
				difficulty_selet.text = "Default : <color=green>Easy</color>";
				difficulty_number = 1;			
				difficulty_name = "쉬움";
				Cs_Difficulty.instance.Debug_DIFF = 1;
				
				Debug.Log("Default : Easy");
			break;
			case 2: 			// Normal
				difficulty_selet.text = "Default : <color=orange>Normal</color>";
				difficulty_number = 2;
				difficulty_name ="보통";
				Cs_Difficulty.instance.Debug_DIFF = 2;
				Debug.Log("Default : Normal");
			break;
			case 3: 			// Hard
				difficulty_selet.text = "Default : <color=red>Hard</color>";
				difficulty_number = 3;
				difficulty_name = "어려움";
				Cs_Difficulty.instance.Debug_DIFF = 3;
				Debug.Log("Default : Hard");
			break;
			default:			// 기본값
				difficulty_selet.text = "Default : <color=orange>Normal</color>";
				difficulty_name ="보통";
				difficulty_number = 1;
				Cs_Difficulty.instance.Debug_DIFF = 1;
			break;
		}
	}
	// 사운드 크기
	public void SE_V(){
		Cs_SoundManager.instance.VolumeSE(se_volume.value);
		Cs_SoundManager.instance.VolumeBGM(bgm_volume.value);
		PlayerPrefs.SetFloat("SEV",se_volume.value);
	}
	public void BGM_V(){
		Cs_SoundManager.instance.VolumeBGM(bgm_volume.value);
		PlayerPrefs.SetFloat("BGMV",bgm_volume.value);
	}

	public void LS_SAVE(){
		PlayerPrefs.SetFloat("LSV",ls.value);
	}

	public void Quits()
	{
		// 종료
		Application.Quit();
	}

}
