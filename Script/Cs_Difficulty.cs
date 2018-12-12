using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]	// 데이터 직렬화 
public class Cs_Difficulty : MonoBehaviour {
	static public Cs_Difficulty instance;
	private string p_name = null;
	public 
	#region singleton
	void Awake() {
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);	// 씬이 로드 될때 자동으로 파괴되지 않는 오브젝트
		}	
		else	// Diffculty 2개 이상일 경우
			Destroy(this.gameObject);	
	}


	#endregion singleton

	// 변수모음
	#region variable
		private int diff;				// 난이도
		public int Diff{				// 프로퍼티
			set{diff = value; GetStrong();}
			get{return diff;}
		}

		private float ls;				// 마우스 민감도
		public float Ls{
			set{ls = value; GetLs();}
			get{return ls;}
		}
		public static int strong;				// 난이도에 따른 강력함
	#endregion variable

	[Header("디버그용")]
	[Tooltip("난이도")]
	public int Debug_DIFF;
	[Tooltip("민감도")]
	public float Debug_LS;


	private void Update() {
		Debug_DIFF = instance.Diff;
		Debug_LS = instance.Ls;	
		PlayerPrefs.SetInt("diff", Diff); // 임시저장변수
	}

	public static int GetStrong()
	{
		Debug.Log("Strong Get: " + instance.Diff);
		return instance.Diff;
	}
	public static float GetLs(){
		Debug.Log("Ls Get: " + instance.Ls);
		return instance.Ls;
	}
	// 닉네임
	public string NickName(string name){
		return name;
	}

	public void NickNameSet(TMP_InputField n)		//TEXTInput Mesh 값 저장
	{
		p_name = NickName(n.text);	// 닉네임 저장
	}
	
	public void SaveName()							// 저장
	{
		PlayerPrefs.SetString("ID" , p_name);
	}
}
