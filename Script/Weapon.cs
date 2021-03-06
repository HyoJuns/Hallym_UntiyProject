﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponList
{
	// 무기 설정
	
	public class Weapon : MonoBehaviour {
		[Header("무기 이름")]
		[Tooltip("무기의 이름을 입력하세요.(string)")]
		public string w_name;							// 무기 이름

		[Header("공격 속도")]
		[Tooltip("무기의 연사속도를 입력하세요.(float) \n(0 - 기본모드, 1 - 레이저모드)")]
		public float[] w_fireRate = new float[2];		// 무기 공격속도 

		[Header("사정 거리")]
		[Tooltip("무기의 사정거리를 입력하세요.(int) \n(0 - 기본모드, 1 - 레이저모드)")]
		public int[] w_range = new int[2];				// 무기 사정거리

		[Header("장전 속도")]
		[Tooltip("무기의 장전속도를 입력하세요.(int) \n(0 - 기본모드, 1 - 레이저모드)")]
		public int[] w_reloadtime = new int[2];			// 무기 공격속도

		public bool is_change = false;					// False - 노멀모드, True - 채광모드

		/* ======================================================================================= */
		#region NormalMode
		[Header("공격력")]
		[Tooltip("무기 공격력(int)")]
		public int damage;						// 총의 데미지.

		[Header("재장전 갯수")]
		[Tooltip("총알 재장전시 나오는 갯수(int)")]
		public int reloadBulletCount;			// 총알의 재장전 하면 나오는 갯수 

		[Header("남은 탄환")]
		[Tooltip("현재 남은 탄환(int)")]
		public int currentBulletCount;			// 현재 탄알집의 남아있는 총알의 갯수 

		[Header("최대 탄환")]
		[Tooltip("최대 소유 가능한 탄환(int)")]
		public int maxBulletCount;				// 최대 소유 가능한 총알 갯수

		[Header("소유중인 탄환")]
		[Tooltip("총알의 갯수(int)")]
		public int carryBulletCount;			// 현재 소유하고 있는 총알의 갯수 (주머니속)

		#endregion NormalMode		// 기본모드
		/* ======================================================================================= */
		#region LaserMode
		[Header("채광데미지")]
		[Tooltip("특정자원 캐는데 데미지")]
		public int minedamage;						// 채광 데미지

		[Header("최대 에너지양")]
		[Tooltip("레이저 채광의 최대 에너지양")]
		public int max_mine_energy;					// Max Energy

		[Header("현재 에너지양")]
		[Tooltip("레이저 에너지 양")]
		public int curr_mine_energy;				// Current Energy


		#endregion LaserMode
		/* ======================================================================================= */
		[Header("기타 시스템")]
		[Tooltip("파티클시스템, 레이저빔")]
		public ParticleSystem muzzleFlash;		// 총구 섬광
		public ParticleSystem laserFlash;		// 레이저 플래쉬
		public GameObject LaserWeapon;			// 레이저 무기
		
		public Animator anim;					// 애니메이터


		// 총알 추가
		public void Addbullet(int _count)
		{
			int retrunbullet = currentBulletCount + _count;		// 남은 탄환
			// 먼저 총알 횟수가 장전 범위내 총알보다 많을 경우
			if (retrunbullet >= reloadBulletCount )
			{
				retrunbullet -= reloadBulletCount;
				currentBulletCount = reloadBulletCount;			// 탄약 충전 완료
				if ( carryBulletCount + retrunbullet >= maxBulletCount){	// 현재 가지고 있는 총알 수가 최대 가질 수 있는 총알 수 보다 많을 때
					carryBulletCount = maxBulletCount;
				}else{
					carryBulletCount += retrunbullet;
				}
			}else{
				currentBulletCount += retrunbullet;			// 탄환 충전
			}

			Debug.Log("총알 추가완료 " +carryBulletCount );
		}

		// 레이저 추가
		public void AddLaser(int _count)
		{
			max_mine_energy += _count;
			Debug.Log("레이저 추가완료 " + max_mine_energy);
		}
	}	// class
}	// namespace

