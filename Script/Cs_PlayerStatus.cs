using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;		// TextMashPro 불러오기
namespace Player{
	// 포인트 올릴 열거형
	public enum pointset{
			point_HP = 1, point_SP, point_ATK, point_ARM 
	}

	public class Cs_PlayerStatus : MonoBehaviour {

		#region 스탯
		[Header("채력")]
		[SerializeField]
		private int max_hp;					// 최대 채력
		private int current_hp;				// 현재 체력
		[Header("스테미나")]
		[SerializeField]
		public int max_sp;					// 최대 스테미나
		public int current_sp;				// 현재 스테미나
		[Header("스테미나 증가량")]
		[SerializeField]
		private int speed_sp;				// 스테미나 증가량
		[Header("스테미나 회복 딜레이")]
		[SerializeField]
		private float RegenTime_sp;			// 스테미나 재회복 딜레이
		private float currentRegenTime_sp;	// 현재 딜레이 확인

		public bool Used_sp;				// 스테미나 감소 여부확인
		[Header("방어력")]
		[SerializeField]
		private int armor;					// 방어력
		private int current_armor;			
		[Header("음식")]
		[SerializeField]
		private int hungry;					// 배고픔
		private int current_hungry;			// 현재 배고픔 상태
		[Header("배고파지는 속도")]
		[SerializeField]
		private int DecreaseTime_hungry;	// 배고픔이 줄어드는 속도
		private int current_DecreaseTime_hungry;
		[Header("물 줄어드는 속도")]
		[SerializeField]
		private int DecreaseTime_thirsty;	// 목마름이 줄어드는 속도
		private int current_DecreaseTime_thirsty;
		[Header("물")]
		[SerializeField]
		private int thirsty;				// 목마름
		private int current_thirsty;		// 현재 목마름
		#endregion 스탯
		
		[Header("UI Image")]
		[Tooltip("[0] HP, [1] Armor, [2] SP, [3] 배고픔, [4] 목마름")]
		[SerializeField] private Image[] image_Gauge;	// 플레이어 상태 프로그래스바 형태로 나타내는 이미지
		[Tooltip("[0] HP, [1] Armor, [2] SP, [3] 배고픔, [4] 목마름")]
		[SerializeField] private Text[] Text_Gauge;		// 플레이어 상태를 몇퍼센트 정도 인지 확인해주는 텍스트
		[SerializeField] private Image dmg_img;			// 데미지 입을 때 나타나는 색상

		[Header("조건")]
		
		[Tooltip("Dead = True")][SerializeField] private bool isDead = false;	// 나는 죽었습니까?
		[Tooltip("맞으면 = true")][SerializeField] private bool isDmg	= false;	// 나는 데미지를 받았습니까?

		[Header("색상")]
		public Color flashColour = new Color(1f,0f,0f,0.5f);	// 섬광 색상
		public float flashSpeed = 8f;							// 섬광 속도

		[Header("player Animation")]
		public Animator ans;
		
		// 스크립트
		Player.Cs_PlayerControl controls;

		/*
			Status 창 - O Key
		 */
		[Header("화면창")][Tooltip("화면을 뛰우는 오브젝트")]
		public GameObject player_status;	// 플레이어 스테이스터스 창
		public GameObject player_msg;		// 플레이어 메세지 창

		[Header("스테이터스")]
		[Tooltip("스테이터스 관련 함수")]
		public string status_ID;			// 스테이터스 닉네임
		public int status_Level;			// 스테이터스 레벨
		public static int status_Day;		// 하루
		public int status_Time;				// 시간 (분)
		private int status_point;			// 스텟포인트

		public static int exp;					// 경험치
		public static int Point_Score =0;           // 포인트

		// 프로퍼티
		public int Status_point{
			set{status_point += value;}	
			get{return status_point;}		
		}

		// TextMashPro
		[Header("TextMashPro 오브젝트")]
		[Tooltip("텍스트Mash를 이용한 오브젝트")]
		public TextMeshProUGUI txtmash_ID;		// 아이디
		public TextMeshProUGUI txtmash_lv;		// 레벨
		public TextMeshProUGUI txtmash_hp;		// 채력
		public TextMeshProUGUI txtmash_sp;		// 스테미나
		public TextMeshProUGUI txtmash_atk;		// 공격력
		public TextMeshProUGUI txtmash_arm;		// 방어력
		public TextMeshProUGUI txtmash_time;	// 시간 (분)
		public TextMeshProUGUI txtmash_day;		// 일
		public TextMeshProUGUI txtmash_point;	// 포인트
		public TextMeshProUGUI txtmash_score;	// 스코어

		// Msg TextMAshPro
		[Header("Msg창 TextMashPro")]
		public TextMeshProUGUI txtmash_msgpoint;	// 메세지 포인트

		private int number;						// UP 선택버튼 (1 hp,2 sp,3 atk,4 arm)

		private bool is_status = false;			// 스테이터스 창 열려잇는가?
		private float timer = 0.0f;
		private float timer_day = 0.0f;
		// 프로퍼티
		public int Number{
			get{return number;} set{number = value;}
		}
		public bool Is_status{
			get{return is_status;} set{is_status = value;}
		}
		
		

		[Header("공격력")]
		public int maxdmg;		// 총 공격력
		public int currdmg;		// 현재 공격력

		private const int HP =0, ARMOR =1, SP =2, HUNGRY =3, THIRSTY =4, SATISFY =5;

		

		void Awake()
		{
			
			player_status = GameObject.Find("Player_StatWindow") as GameObject;	// 플레이어 스텟창 찾기
		}




		// Use this for initialization
		void Start () {
			current_hp = max_hp;
			current_armor = armor;
			current_sp = max_sp;
			current_hungry = hungry;
			current_thirsty = thirsty;
			
			//Status_point = 3;
			player_status.SetActive(false);
			
			// Load File
			status_ID = PlayerPrefs.GetString("ID","Null");
			exp = PlayerPrefs.GetInt("ID_" + status_ID + "_exp",0);					// 경험치 불러오기
			status_Level = PlayerPrefs.GetInt("ID_" + status_ID +"_lv",0);			// 레벨 불러오기
			status_point = PlayerPrefs.GetInt("ID_" + status_ID + "_point",0);		// 포인트 불러오기
			max_hp = PlayerPrefs.GetInt("ID_" + status_ID + "_hp",100);				// 채력
			max_sp = PlayerPrefs.GetInt("ID_" + status_ID + "_msp",10);				// sp
			armor = PlayerPrefs.GetInt("ID_" + status_ID + "_arm",10);				// 방어력
			current_hungry = PlayerPrefs.GetInt("ID_" + status_ID + "_hungry",2000); 	// 배고픔
			current_thirsty = PlayerPrefs.GetInt("ID_" + status_ID + "_thirsty",2000); 	// 목마름
			maxdmg = PlayerPrefs.GetInt("ID_" + status_ID + "_dmg",5);
			currdmg = PlayerPrefs.GetInt("ID_" + status_ID + "_currdmg",0);				

			Cs_SoundManager.instance.PlayBGM("bgm_gameday");

			current_hp = max_hp;
		}
		
		// Update is called once per frame
		void Update () {
			timer += Time.smoothDeltaTime;
			timer_day += Time.smoothDeltaTime;
			//Debug.Log("t" + timer + " ; " + timer_day);
			p_damage();
			p_Hungry();
			p_Thirsty();
			p_GaugeUpdate();
			p_SpHealth();
			Status_Update();
			Dead_animation();
			winner();
			StartCoroutine(Save());		// 저장
			StartCoroutine(LevelUp());  // 레벨업
			

			
			if(timer >= 60)
			{
				status_Time++;
				timer = 0;
			}

			if(timer_day >= 120)
			{
				status_Day++;
				timer_day = 0;
			}
			
		}

		IEnumerator Save()
		{
			PlayerPrefs.SetInt("ID_" + status_ID +"_exp",exp);
			PlayerPrefs.SetInt("ID_" + status_ID +"_lv",status_Level);
			PlayerPrefs.SetInt("ID_" + status_ID + "_point",status_point);
			PlayerPrefs.SetInt("ID_" + status_ID + "_hp",max_hp);	
			PlayerPrefs.SetInt("ID_" + status_ID + "_msp",max_sp);
			PlayerPrefs.SetInt("ID_" + status_ID + "_arm",armor);
			PlayerPrefs.SetInt("ID_" + status_ID + "_hungry",current_hungry);
			PlayerPrefs.SetInt("ID_" + status_ID + "_thirsty",current_thirsty);
			PlayerPrefs.SetInt("ID_" + status_ID + "_dmg",maxdmg);
			PlayerPrefs.SetInt("ID_" + status_ID + "_currdmg",currdmg);


			yield return new WaitForSeconds(10.0f);
		}
		IEnumerator LevelUp()
		{
			if((status_Level * 300 + 100 ) <= exp)
			{	
				Cs_SoundManager.instance.PlaySE("se_level");
				exp -= status_Level * 300 + 100;
				status_point += 3;		// 3포인트 증가
				PlayerPrefs.SetInt("ID_" + status_ID +"_lv",++status_Level);
				PlayerPrefs.SetInt("ID_" + status_ID +"_exp",exp);
				
			}
			yield return new WaitForSeconds(10.0f);
		}

		void winner()
		{
			if(status_Day == 30)
				SceneManager.LoadScene("Winner", LoadSceneMode.Single);
		}

		void Dead_animation()
		{
			if(!isDead && (current_hungry <= 0.1f || current_hp <= 0 || current_thirsty <= 0.1f))
			{
				isDead = true;
				Cursor.lockState = CursorLockMode.None;
				ans.SetTrigger("t_dead");
				
			}
		}

	
		// 데미지를 받을 경우 화면에 빨간색으로 맞은 효과를 연출한다.
		void p_damage()
		{
			if(isDmg) {dmg_img.color = flashColour;}
			else{
				dmg_img.color = Color.Lerp(dmg_img.color, Color.clear, flashSpeed * Time.deltaTime);
			}

			isDmg = false;
		}

		/// <summary>
		/// 나에게 데미지가 amount 만큼 들어왔다.
		/// </summary>
		/// <param name="amount"></param>
		public void take_damage(int amount)
		{
			int t = 0;
			isDmg = true;			// 맞았기 때문에 p_damage() 실행
			if (armor <= amount){
				t = amount - armor;
				 armor = armor- (armor / 10);	// 내구도 감소
			}
			else{
				t = 1;
				armor = armor - 1;
			}
			if(armor <= 0) {armor = 0;}
			if(current_hp > t)
				current_hp -= t;	// 채력 감소
			else
			{
				current_hp = 0;
			}
		}

		/// <summary>
		/// HP 회복 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Heal_HP(int _count)
		{
			if(current_hp + _count < max_hp) current_hp += _count;
			else current_hp = max_hp;
		}

		/// <summary>
		/// 기력 회복 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Heal_SP(int _count)
		{
			if(current_sp + _count < max_sp) current_sp += _count;
			else current_sp = max_sp;
		}

		/// <summary>
		/// 방어력 증가 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Up_armor(int _count)
		{
			armor += _count;
		}

		/// <summary>
		/// 공격력 증가 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Up_attack(int _count)
		{
			currdmg += _count;
		}

		/// <summary>
		/// 방어력 감소 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Down_armor(int _count)
		{
			if (armor - _count > 0)
				armor -= _count;
			else
				armor =0;
		}

		/// <summary>
		/// 배고픔 증가 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Up_Hungry(int _count)
		{
			if(current_hungry + _count < hungry)
				current_hungry += _count;
			else
				current_hungry = hungry;
		}

		public void ExpUp(int _count)
		{
			exp += _count;
		}

		/// <summary>
		/// 배고픔 감소 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Down_Hungry(int _count)
		{
			if(current_hungry - _count > 0)
				current_hungry -= _count;
			else
				current_hungry = 0;
		}

		/// <summary>
		/// 목마름 증가 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Up_Thirsty(int _count)
		{
			if(current_thirsty + _count < thirsty)
				current_thirsty += _count;
			else
				current_thirsty = thirsty;
		}

		/// <summary>
		/// 목마름 감소 (_count)
		/// </summary>
		/// <param name="_count"></param>
		public void Down_Thirsty(int _count)
		{
			if(current_thirsty - _count > 0)
				current_thirsty -= _count;
			else
				current_thirsty = 0;

		}

		/// <summary>
		/// 사망
		/// </summary>
		void Death()
		{
			// 죽음
			Destroy(controls);
			PlayerPrefs.SetInt("ID_" + status_ID + "_hungry",2000);
			PlayerPrefs.SetInt("ID_" + status_ID + "_thirsty",2000);
			SceneManager.LoadScene("Fail",LoadSceneMode.Single);
			
			
		}
		/// <summary>
		/// 배고픔 수치
		/// </summary>
		private void p_Hungry()
		{
			if(isDead) return;
			if(current_hungry > 0)
			{
				if(current_DecreaseTime_hungry <= DecreaseTime_hungry)
					current_DecreaseTime_hungry++;
				else
				{
					current_hungry--;
					current_DecreaseTime_hungry = 0;
				}
			}
		}

		/// <summary>
		/// 목마름 수치
		/// </summary>
		private void p_Thirsty()
		{
			if(isDead) return;
			if(current_thirsty > 0)
			{
				if(current_DecreaseTime_thirsty <= DecreaseTime_thirsty)
					current_DecreaseTime_thirsty++;
				else
				{
					current_thirsty--;
					current_DecreaseTime_thirsty = 0;
				}
			}
		}

		/// <summary>
		/// UI 표시
		/// </summary>
		private void p_GaugeUpdate()
		{
			// 이미지 형태 플레이어 상태 Image 변화
			image_Gauge[HP].fillAmount = (float)current_hp / max_hp;
			image_Gauge[ARMOR].fillAmount = (float)armor / 100;
			image_Gauge[SP].fillAmount = (float)current_sp / max_sp;
			image_Gauge[HUNGRY].fillAmount = (float) current_hungry / hungry;
			image_Gauge[THIRSTY].fillAmount = (float) current_thirsty / thirsty;
			
			// 텍스트 형태 플레이어 상태 UI 변화
			// 전체값의 몇 퍼센트 공식 : 전체값 * 퍼센트 / 100
			// 전체값 일부값의 퍼센트 공식 : 일부값 / 전체값 * 100
			Text_Gauge[HP].text =  current_hp +"";
			Text_Gauge[HUNGRY].text =  ((float)current_hungry / hungry * 100) + "%";
			Text_Gauge[THIRSTY].text =  ((float)current_thirsty / thirsty * 100) + "%";
		}


		/// <summary>
		/// 스테이터스창 업데이트
		/// </summary>
		private void Status_Update()
		{
			
			if(Input.GetKeyDown(KeyCode.O) && !is_status){
				Cursor.lockState = CursorLockMode.None;
				WeaponList.Cs_WeaponControl.waitfire = true;
				player_status.SetActive(true);
				player_msg.SetActive(false);
				Is_status = true;
				Debug.Log(Is_status);
			}else if(Input.GetKeyDown(KeyCode.O) && is_status)
			{
				Cursor.lockState = CursorLockMode.Locked;
				WeaponList.Cs_WeaponControl.waitfire = false;
				player_status.SetActive(false);
				player_msg.SetActive(false);
				Is_status = false;
				Debug.Log(Is_status);
			}
			if(Input.GetKeyDown(KeyCode.Escape) && is_status)
			{
				Cursor.lockState = CursorLockMode.Locked;
				WeaponList.Cs_WeaponControl.waitfire = false;
				player_status.SetActive(false);
				player_msg.SetActive(false);
				Is_status = false;
				Debug.Log(Is_status);
			}

			if(player_status.activeSelf == true){
				txtmash_ID.text = status_ID;
				txtmash_arm.text = "" + armor;
				txtmash_atk.text = "" + maxdmg + "(" +currdmg +")";
				txtmash_hp.text = max_hp + "/" + current_hp;
				txtmash_sp.text = max_sp + "/" + current_sp;
				txtmash_point.text = Status_point + "";
				txtmash_score.text = "Score: "+ Point_Score;
				txtmash_time.text = "Time(M) : " + status_Time;
				txtmash_day.text = "Day : " + status_Day;
				txtmash_lv.text = status_Level +"";
				
			}

		}

		// Close 버튼
		public void btn_close()
		{
			Cursor.lockState = CursorLockMode.Locked;
			WeaponList.Cs_WeaponControl.waitfire = false;
			player_status.SetActive(false);
			player_msg.SetActive(false);
			Is_status = false;
			Debug.Log(Is_status);
		}

		/// <summary>
		/// SP 회복여부
		/// </summary>
		public void p_SpHealth()
		{
			if(!Used_sp)		// SP 회복시간
			{
				if (current_sp >= max_sp){current_sp = max_sp;}
				else{
					currentRegenTime_sp += Time.deltaTime;
					if (RegenTime_sp <= currentRegenTime_sp)
					{
						current_sp += speed_sp;
						currentRegenTime_sp = 0.0f;
					}
				}
			}else{
				currentRegenTime_sp = 0.0f;
			}	
		}


		/// <summary>
		/// Up 버튼 누르면 실행되는 함수
		/// 메세지 창을 뛰운 후 Ok버튼 누르면 포인트 증가
		/// </summary>
		/// <param name="i"></param>
		public void viewmsg(int i)
		{
			int needpoint = 1;		// 필요포인트
			number = i;				// 해당 번호
			player_msg.SetActive(true);	// 메세지 화면 보여주기.
			if(i == 4) {needpoint = 2;}	// 방어력은 2포인트 필요
			txtmash_msgpoint.text = needpoint + "";	// 텍스트 보여주기

			
		}

		/// <summary>
		/// Ok - 스텟올려준다.
		/// </summary>
		/// <param name="i"></param>
		public void btn_ok()
		{
			switch(number)
			{
				case 1: btn_up(pointset.point_HP); break;
				case 2: btn_up(pointset.point_SP); break;
				case 3: btn_up(pointset.point_ATK); break;
				case 4: btn_up(pointset.point_ARM); break;
				default :
					break;
			}
		}


		/// <summary>
		/// 버튼을 누를 경우
		/// </summary>
		/// <param name="n"></param>
		public void btn_up(pointset n)
		{	
			 Debug.Log("스텟상승 " + n.ToString() + " point : " + Status_point);
			Cs_SoundManager.instance.PlaySE("se_select");
			if(status_point >= 1)
			{
				// 1포인트 이상인경우
				switch(n)
				{
					case pointset.point_HP:		// 채력인경우
						Status_point = -1 ;	// 스텟감소
						max_hp += 10;						// HP 10 증가
						current_hp += 10;					// 회복 10 증가
					break;
					case pointset.point_SP:		// 스테미너인경우
						Status_point = -1;	// 스텟감소
						max_sp += 1;
						current_sp += 1;
					break;
					case pointset.point_ATK:		// 공격력인경우
						Status_point = -1;	// 스텟감소
						currdmg += 1;
					break;
					case pointset.point_ARM:		// 공격력인경우
						Status_point = -2;	// 2 스텟감소
						armor += 1;					// 방어력 증가
					break;
				}
			}
		} // btn_up


		private void OnTriggerStay(Collider other) {
			if ( other.transform.tag == "_water")		// 정수기 물 마실경우
			{
				Up_Thirsty(25);							// 식수량 증가
				Debug.Log("꿀꺽");
			}
			if (other.transform.tag == "_heal")			// 침대
			{
				Heal_HP(10);							// 회복량 증가
				Debug.Log("회복중");
			}
		}

		

		
	} // class
} // namespace