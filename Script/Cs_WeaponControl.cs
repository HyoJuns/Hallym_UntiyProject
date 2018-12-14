using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeaponList
{
	public class Cs_WeaponControl : MonoBehaviour {
		
		// 현재 장착된 무기
		[SerializeField] private Weapon currentweapon;		

		private float currentFireRate;					// 현재 연사속도 계산 변수

		// 상태변수
		private bool is_reload = false;					// 장전중인가?
		private bool is_minning = false;				// 채광모드인가?
		
		public static bool waitfire = false;			// 쏘는거 금지
		
		[SerializeField] private Vector3 originPos;	 	// 기존 위치값
		
		// 사운드
		private string _weaponmode = "se_weapon";		// 일반 모드로 싸울시 효과음
		private string _lasermode = "se_laser";			// 레이저 모드로 싸울시 효과음
		private string _select = "se_select";			// 선택 효과음 (여기선 무기 체인지 효과음)
		private string _reload = "se_reload";			// 장전
		
		// 충돌 정보
		private RaycastHit hitInfo;

		// 필요 컴포넌트
		[Header("MainCamera")]
		public Camera theCam;							// 채광모드시.. Raycasthit 이용하여 레이저 충돌정보 받아옴
		[Header("Enemy Information")]
		public Cs_EnemyHud huds;						// 몹 HUD
		[Header("Player Status")]
		public Player.Cs_PlayerStatus stat;				// 스테이터스
		[Header("HUD")]
		public Cs_WeaponHUD hud;						// 디스플레이
		public Cs_minning mine;
		[Header("피격")]
		// 피격 이펙트
		[SerializeField] private GameObject hit_effect_prefab;
		[SerializeField] private GameObject laserhit_effect_prefab;	

		private void Start() {
			originPos = Vector3.zero;				// 0,0,0 으로 초기화
			Cursor.lockState = CursorLockMode.Locked;	
		}

		void Update() {
			if(!waitfire){		// 일을 안할 때 
				PlayerPrefs.SetInt("laser",currentweapon.curr_mine_energy);
				PlayerPrefs.SetInt("maxlaser", currentweapon.max_mine_energy);
				hud.Curr = currentweapon.currentBulletCount;
				hud.Carr = currentweapon.carryBulletCount;
				FireRateCalc();
				TryFire();
				TryReload();
				TryLaserReload();	
				Change_Mode();
			}// 무기 변경
		}

		
		/// <summary>
		/// 채광모드 -> 일반모드로 무기모드 변경
		/// </summary>
		private void Change_NormalMode(){
			// 채광모드 -> 일반모드 변경
			Debug.Log("일반모드 변경");
			hud.Is_change(false);
			is_minning = false;								// 채광모드 Off
			Cs_SoundManager.instance.PlaySE(_select);		// 효과음 재생
			
		}
		/// <summary>
		/// 일반모드 -> 채광모드로 무기모드 변경
		/// </summary>
		private void Change_LaserMode()
		{
			// 일반모드 -> 채광모드 변경
			Debug.Log("채광모드 변경");
			is_minning = true;
			hud.Is_change(true);							// 채광모드 On
			Cs_SoundManager.instance.PlaySE(_select);		// 효과음 재생
		}

		/// <summary>
		/// 무기를 변경하는 함수 
		/// Q 버튼을 눌러 무기모드를 변경할 수 있게 해준다.
		/// </summary>
		public void Change_Mode(){							
			if(Input.GetKeyDown(KeyCode.Q) && !is_minning)
			{
				// 일반 -> 채광모드
				Change_LaserMode();
			}else if(Input.GetKeyDown(KeyCode.Q) && is_minning)
				Change_NormalMode();	// 채광 -> 일반
			else
			{
				return;
			}
		}

		/// <summary>
		/// 무기 연사속도 재계산
		/// </summary>
		public void FireRateCalc()
		{
			if(currentFireRate > 0)
				currentFireRate -= Time.deltaTime;
		}
		// 발사 시도
		private void TryFire()
		{
			if(Input.GetButton("Fire1") && currentFireRate <= 0 && !is_reload && !is_minning)
				Noraml_Fire();		// 총알 발사
			else if(Input.GetButton("Fire1") && currentFireRate <= 0 && !is_reload && is_minning)
				Laser_Fire();		// 레이저 발사\
		}

		// 재장전 시도
		private void TryReload()
		{
			// R 키를 누르며, 현재 장전중이 아니며, 현재 총알 < 장전후 총알 만큼 적으면 실행
			if(Input.GetKeyDown(KeyCode.R) && !is_reload  &&currentweapon.currentBulletCount < currentweapon.reloadBulletCount){
				StartCoroutine("ReloadCoroutine");		// 재장전 코루틴 실행
			}
		}
		private void TryLaserReload()
		{
			// R 키를 누르며, 현재 장전중이 아니며, 현재 총알 < 장전후 총알 만큼 적으면 실행
			if(Input.GetKeyDown(KeyCode.R) && !is_reload  &&currentweapon.curr_mine_energy < currentweapon.max_mine_energy){
				StartCoroutine("ReloadCoroutine");		// 재장전 코루틴 실행
			}
		}

		// 일반모드 발사준비 완료
		private void Noraml_Fire()
		{
			if(!is_reload)
			{
				if(currentweapon.currentBulletCount > 0)
				{
					Debug.Log("Shoot !!");
					Shoot();
				}else{

				}
			}
		}
		
		// 일반모드 발사
		private void Shoot()
		{
			
			currentweapon.currentBulletCount--;					// 현재 남은 총알 갯수 감소
			currentFireRate = currentweapon.w_fireRate[0];		// 재계산
			Cs_SoundManager.instance.PlaySE(_weaponmode);		// 효과음 재생
			currentweapon.muzzleFlash.Play();					// 섬광
			StopAllCoroutines();								// 실행중인 코루틴 전부 중지
			Hit();
		}

		private void Laser_Fire()
		{
			if(!is_reload)
			{
				if(currentweapon.curr_mine_energy > 0)
				{
					Debug.Log("Laser Shot !!");
					Lasershoot();
				}
			}
		}

		private void Lasershoot()
		{
			currentweapon.curr_mine_energy -= 5;			// 에너지 양 줄어들음.
			currentFireRate = currentweapon.w_fireRate[1]; // 계산
			Cs_SoundManager.instance.PlaySE("se_laser");
			currentweapon.laserFlash.Play();
			StopAllCoroutines();
			Hit2();
		}

		// 총알 명중
		private void Hit()
		{
			if(Physics.Raycast(theCam.transform.position, theCam.transform.forward,out hitInfo, currentweapon.w_range[0]))
			{
				// 타격 이펙트를 hitInfo Point에 생성
				GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				if(hitInfo.transform.tag == "enemy")
				{
					huds.HUD(true);
					huds.e = hitInfo.transform.GetComponent<Cs_Enemyinfomation>();
					huds.Player_Attack(stat.maxdmg + stat.currdmg);	// 공격
				}
				//Debug.Log(hitInfo.transform.name);
				
			}
		}
		// 레이저 명중
		private void Hit2()
		{
			if(Physics.Raycast(theCam.transform.position, theCam.transform.forward,out hitInfo, currentweapon.w_range[1]))
			{
				// 타격 이펙트를 hitInfo Point에 생성
				GameObject clone = Instantiate(laserhit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				if(hitInfo.transform.tag == "enemy")
				{
					// 고쳐야 할 부분
					huds.HUD(true);
					huds.e = hitInfo.transform.GetComponent<Cs_Enemyinfomation>();
					huds.Player_Attack(1);	// 채광용임
				}else if (hitInfo.transform.tag == "mine")
				{
					mine = hitInfo.transform.GetComponent<Cs_minning>();
					mine.Minning(currentweapon.minedamage);
				}
				
				
			}
		}

		// 재장전
		IEnumerator ReloadCoroutine()
		{
			if(!is_minning){
				if(currentweapon.carryBulletCount > 0)
				{
					// 총 탄환 수 중 남아있는 탄환 갯수가 1개 이상일 때
					is_reload = true;		// 나 이제 장전중이야.
					
					// 다시 재장전을 위해 현재 남은 탄환을 주머니에 추가시켜준다.
					currentweapon.carryBulletCount += currentweapon.currentBulletCount;
					currentweapon.currentBulletCount = 0;		// 남은 탄환을 주머니속에 넣어 지금 총안에 장전된 총알은 0발
					Cs_SoundManager.instance.PlaySE(_reload);
					yield return new WaitForSeconds(currentweapon.w_reloadtime[0]); // 장전중

					if(currentweapon.carryBulletCount >= currentweapon.reloadBulletCount)
					{
						// 장전해야할 총알 수 보다 지금 내 주머니 속에 가지고 있는 총알이 더 많을 때
						currentweapon.currentBulletCount = currentweapon.reloadBulletCount;	
						// 주머니에서 총알을 꺼내 장전했으니 그 수만큼 감소
						currentweapon.carryBulletCount -= currentweapon.reloadBulletCount;	
					}else // 장전해야할 총알 수 보다 지금 내 주머니 속에 가지고 있는 총알이 적을 때
					{
						// 필요한 장전수만큼 가지고 있지 않을 때 내 주머니에서 몽땅 장전해 놓았다.
						currentweapon.currentBulletCount = currentweapon.carryBulletCount;
						// 주머니에 있는 총알을 몽땅 다 장전해서 주머니에 가지고 있는 총알은 없으니 0 삽입
						currentweapon.carryBulletCount = 0;
					}

					// 장전타임 끝났음.
					is_reload = false;
				}else{
					Debug.Log("총알 부족");
				}
			}else {
					if(currentweapon.max_mine_energy > 0)
					{
						Debug.Log("레이저 장전중..");
						is_reload = true;
						currentweapon.max_mine_energy += currentweapon.curr_mine_energy;
						currentweapon.curr_mine_energy = 0;
						Cs_SoundManager.instance.PlaySE(_reload);
						yield return new WaitForSeconds(currentweapon.w_reloadtime[1]);	// 대기

						if(currentweapon.max_mine_energy >= 100)
						{
							currentweapon.curr_mine_energy += 100;
							currentweapon.max_mine_energy -= 100;
						}else{
							currentweapon.curr_mine_energy = currentweapon.max_mine_energy;
							currentweapon.max_mine_energy = 0;
						}

						// 장전타임 끝났음.
						is_reload = false;
					}
			}
		}

	}	// class
}	// namespace

