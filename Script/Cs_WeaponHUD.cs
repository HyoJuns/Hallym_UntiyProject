using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeaponList
{
	public class Cs_WeaponHUD : MonoBehaviour {

		[SerializeField]
		private WeaponList.Cs_WeaponControl control;
		
		// 필요하면 HUD 호출, 필요없으면 HUD 비활성화
		[SerializeField]
		[Tooltip("Canvas 화면 중 HUD")]
		private GameObject hud_laserhud;

		// 레이저 에너지 텍스트 반영
		[SerializeField]
		[Header("레이저 에너지량 텍스트")]
		private Text text_laser;		// 탄환수 / 전체탄환

		[Header("총알 횟수 게이지바")]
		public Image bullet_bar;		// 탄환바
		[Header("무기아이콘")]
		public Image weapon_img;		// 무기 이미지

		void Start() {
			control = GameObject.Find("Gun").GetComponent<Cs_WeaponControl>();	
		}

		void Update() {
			Checkbullet();		
		}
		
		private void Checkbullet()
		{
			if(control.play_weapon.carryBulletCount <= 0)
			{
				bullet_bar.fillAmount = 0;
				text_laser.text = 0 + "%";
			}else{
				bullet_bar.fillAmount = ((float)control.play_weapon.carryBulletCount / control.play_weapon.reloadBulletCount );
				text_laser.text =(control.play_weapon.carryBulletCount / control.play_weapon.reloadBulletCount * 100 ) + "%";
			}
		}
	}	
}

