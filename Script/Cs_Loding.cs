using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;		// TextMAsh Pro
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cs_Loding : MonoBehaviour {

	public GameObject obj;							// 판정하기 위해 만든 함수

	[Header("TextMashPro")]
	public TextMeshProUGUI txt_loding;				// 텍스트 로딩창

	[Header("삭제할 오브젝트")]
	public GameObject nova;							// 비행전함
	public GameObject[] deletelist;						// 타이틀
	bool IsDone	= false;
	float dealyTime = 0f;		
	/* 
	// 비동기적 코루틴 수행,  yield기능을 사용할 수 있고, (isDone) 또는 (progress)를 사용해서 완료가 되었는지 진행중인지 수동으로 확인할 수 있습니다.
	// allowSceneActivation 장면이 준비된 즉시 장면의 활성화를 허용
	// isDone : 동작 완료되는 되로 나타낼 수 있음
	// priority 동작이 비동기적인 작업요청의 순서를 수정할 수 있도록 함
	// progress 작업의 진행상태를 나타낸다.
	*/
	AsyncOperation async_operation;
	
	public void Loads()
	{
		StartCoroutine(StartLoad("Game_play"));
		Cs_Difficulty.instance.Diff = Cs_TitleMenu.difficulty_number;
		Debug.Log("Loding");
		Destroy(nova);
		for(int i = 0 ; i < deletelist.Length ; i++)
			Destroy(deletelist[i]);
	}	
	
	private void Update() {
		if( obj.active == true){
			dealyTime += Time.deltaTime;
			txt_loding.text = Mathf.Round(dealyTime * 20) + "%";
		}
		if(dealyTime >= 5.0f)
		{
			Debug.Log("Delay Time End");
			async_operation.allowSceneActivation = true;	// 시간에 따라 장면이 즉시 넘어가는 것을 허용한다.
		}
	}
	public IEnumerator StartLoad(string scene_name)
	{
		async_operation = SceneManager.LoadSceneAsync(scene_name);
		async_operation.allowSceneActivation = false;

		if(IsDone == false)
		{
			IsDone = true;
			while(async_operation.progress < 0.9f)
			{
				txt_loding.text =  async_operation.progress + "%";
				Debug.Log(async_operation.progress);
				yield return true;
			}
		}
	}
}
