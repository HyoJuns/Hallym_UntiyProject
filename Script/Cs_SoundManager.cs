using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 사운드 클래스
[System.Serializable]	// 데이터 직렬화 
public class Sound{
	public string name;		// 곡의 이름
	public AudioClip clip;	// 곡
}

public class Cs_SoundManager : MonoBehaviour {

	// SoundManager는 씬 옮겨도 파괴되지 않도록 설정
	// 싱글턴화 시킨다.
	static public Cs_SoundManager instance;		// 공유자원으로 접근하는 사운드 메니저
	
	// 부분 가려주기 
	#region singleton
		void Awake()	// 최초 1번만 호출
		{
			if(instance == null){
				instance = this;
				DontDestroyOnLoad(gameObject);	// 씬이 로드 될때 자동으로 파괴되지 않는 오브젝트
			}
				
			else	// 사운드 매니저가 2개 실행됬을 때 하나 제거한다.
				Destroy(this.gameObject);
		}
	#endregion singleton
	
	[Header("효과음")]
	public AudioSource[] audioSourceEffects;	// 효과음 Mp3 Player
	[Header("배경음악")]
	public AudioSource audioSourceBGM;		// 브금 Mp3 Player

	private string[] playSoundName;				// 음악 이름
	private string playBGMName;				// 브금 이름
	[Header("사운드 파일정보")]
	public Sound[] effectSounds;				// 효과음 사운드
	public Sound[] bgmSounds;					// 브금 사운드
	
	/*
		사용방법 다른 C# 스크립트 가서 private string OOOOO_Sound; 한다음 
		Cs_SoundManager.instance.PlaySE(파일이름); 하면 사운드 재생된다.
	 */



	// Sound안에 곡이름을 입력하면 이 사운드 실행
	public void PlaySE(string _name)
	{
		for(int i = 0 ; i < effectSounds.Length; i++)
		{
			if(_name == effectSounds[i].name)
			{
				// 재생중이지 않은 오디오 소스 찾기 위해
				for(int j = 0 ; j < audioSourceEffects.Length; j++)
				{
					if(!audioSourceEffects[j].isPlaying)
					{
						playSoundName[j] = effectSounds[i].name;			// 재생중인 이름 등록 
						audioSourceEffects[j].clip = effectSounds[i].clip;
						audioSourceEffects[j].Play();
						Debug.Log(playSoundName + " 효과음.");
						return;		// 함수 자체 종료
					}
				}
				Debug.Log("모든 가용 AudioSource가 사용중입니다.");
				return;;
			}
		}
		Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
	}

	// Sound안에 브금 입력하면 실행
	public void PlayBGM(string _name)
	{
		for (int i = 0; i < bgmSounds.Length; i++)
		{
			if(_name == bgmSounds[i].name)
			{
				// 이름이 맞을 경우
				if(!audioSourceBGM.isPlaying)
				{
					playBGMName = bgmSounds[i].name;			// 재생중인 브금을 저장
					audioSourceBGM.clip = bgmSounds[i].clip;	// 클립 교체
					audioSourceBGM.Play();						// 브금 재생
					Debug.Log(playBGMName + " 사운드를 틀었습니다.");
					return;
				}
			}
		}
		Debug.Log(_name + " 브금이 SoundManager에 등록되지 않았습니다.");
	}
	// 모든 효과음 정지 
	public void StopAllSE()
	{
		for (int i = 0; i < audioSourceEffects.Length; i++)
		{
			audioSourceEffects[i].Stop();
		}
		Debug.Log("모든 효과음 정지");
	}

	// BGM 정지
	public void StopBGM()
	{
		audioSourceBGM.Stop();
		Debug.Log("BGM 정지");
	}

	public void StopSE(string _name)
	{
		for (int i = 0; i < audioSourceEffects.Length; i++)
		{
			if(playSoundName[i] == _name)
			{
				audioSourceEffects[i].Stop();
				return;
			}
		}
		Debug.Log("재생 중인 " + _name + " 사운드가 없습니다.");
	}

}
