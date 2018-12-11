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
		private bool is_walk = false;					// 다른 일 하고 있나?

		// 프로퍼티
		public bool Is_walk{
			set {is_walk = value;}
			get{return is_walk;}
		}

		[SerializeField] private Vector3 originPos;	 	// 기존 위치값
		
		// 사운드
		private string _weaponmode = "se_weapon";		// 일반 모드로 싸울시 효과음
		private string _lasermode = "se_laser";			// 레이저 모드로 싸울시 효과음
		private string _select = "se_select";			// 선택 효과음 (여기선 무기 체인지 효과음)
		private string _reload = "se_reload";			// 장전
		// 레이저 충돌 정보
		private RaycastHit hitInfo;

		// 필요 컴포넌트
		[Header("MainCamera")]
		public Camera theCam;							// 채광모드시.. Raycasthit 이용하여 레이저 충돌정보 받아옴
		[Header("Enemy Information")]
		public Cs_EnemyHud huds;						// 몹 HUD
		[Header("Player Status")]
		public Player.Cs_PlayerStatus stat;				// 스테이터스

		// 피격 이펙트
		[SerializeField] private GameObject hit_effect_prefab;	

		private void Start() {
			originPos = Vector3.zero;				// 0,0,0 으로 초기화	
		}

		void Update() {
			if(!is_walk){		// 일을 안할 때 
				Cursor.lockState = CursorLockMode.Locked;
				FireRateCalc();
				TryFire();
				TryReload();	
				Change_Mode();
			}else									// 무기 변경
			Cursor.lockState = CursorLockMode.None;
		}

		
		/// <summary>
		/// 채광모드 -> 일반모드로 무기모드 변경
		/// </summary>
		private void Change_NormalMode(){
			// 채광모드 -> 일반모드 변경
			Debug.Log("일반모드 변경");
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
			is_minning = true;								// 채광모드 On
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
			if(Input.GetKeyDown(KeyCode.R) && !is_reload && currentweapon.currentBulletCount < currentweapon.reloadBulletCount){
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

		}

		// 명중
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
					huds.Player_Attack(stat.maxdmg);	// 공격
				}
				Debug.Log(hitInfo.transform.name);
				
			}
		}
		
		// 재장전
		IEnumerator ReloadCoroutine()
		{
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
		}
/*


		private void WeaponFireRateCalc()
		{
			if (currrate > 0)
				currrate -= Time.smoothDeltaTime; // 발사 장전시간
		}
		/// <summary>
		/// 무기가 뭔지 체크해주는 함수
		/// </summary>
		private void check_list()
		{
			// 무기 체크 
			if(play_weapon.w_list == Weapon.list_weapon.lasers)
			{
				is_laser = true;
			}else
			{
				is_laser = false;
			}
		}

		/// <summary>
		/// 무기를 발사하는 함수
		/// </summary>
		private void TryFire()
		{
			check_list();		// 채크
			// 발사 함수
			if(Input.GetMouseButton(0) && currrate <= 0 && !is_reload)
			{
				if(is_laser)
				{
					// 레이저총 발사시
					Laser_Fire();
				}else{
					// 기본 총 발사시
					Weapon_Fire();
				}
			}
			if(Input.GetMouseButtonUp(0) && is_laser)
			{
				play_weapon.LaserWeapon.SetActive(false);		// 레이저 숨김
			}
		}

		/// <summary>
		/// 장전 여부 함수
		/// </summary>
		private void TryReload()
		{
			if(Input.GetKeyDown(KeyCode.R) && !is_reload && play_weapon.currentBulletCount <= play_weapon.reloadBulletCount)
			{
				Debug.Log("장전완료");
				StartCoroutine(ReloadCoroutine());
			}
		
		}
		


		/// <summary>
		/// 총알 발사
		/// </summary>
		private void Weapon_Fire()
		{
			// 총알 발사시
			if(!is_reload)
			{
				if(play_weapon.currentBulletCount > 0)
				{
					weapon_shoot();
				}else
					StartCoroutine(ReloadCoroutine());
			}
		}
		/// <summary>
		/// 레이저 발사
		/// </summary>
		private void Laser_Fire()
		{
			// 레이저 발사시
			if(!is_reload)
			{
				if(play_weapon.currentBulletCount > 0)
				{
					laser_shoot();
				}else
					StartCoroutine(ReloadCoroutine());
			}
		}


		/// <summary>
		/// 재장전
		/// </summary>
		/// <returns></returns>
		IEnumerator ReloadCoroutine()
		{
			if(play_weapon.carryBulletCount <= 0)
			{
				is_reload = true;
				play_weapon.carryBulletCount += play_weapon.currentBulletCount;	// 총알 증가
				play_weapon.currentBulletCount = 0;

				yield return new WaitForSeconds(play_weapon.w_reloadtime);		// 장전시간 증가
			
				if(play_weapon.carryBulletCount >= play_weapon.reloadBulletCount)
				{
					play_weapon.currentBulletCount = play_weapon.reloadBulletCount;
					play_weapon.carryBulletCount = play_weapon.reloadBulletCount;
				}else{
					play_weapon.currentBulletCount = play_weapon.carryBulletCount;
					play_weapon.carryBulletCount =0;
				}

				is_reload = false;
			}else{
				Debug.Log("부족");
			}
		}



		/// <summary>
		/// 발사
		/// </summary>
		private void weapon_shoot()
		{
			play_weapon.carryBulletCount--;		// 현재있는 총알 발사
			play_weapon.muzzleFlash.Play();			// 파티클 실행
			Debug.Log("총알 발사");
		}
		private void laser_shoot()
		{
			play_weapon.carryBulletCount--;		// 레이저 총알 발사
			play_weapon.LaserWeapon.SetActive(true);
			Debug.Log("레이저 발사");
		}

*/
	}	// class
}	// namespace

