using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace WeaponList
{
	public class Cs_WeaponHUD : MonoBehaviour {

		private int curr = 0;
		private int carr = 0;
		private int cannon = 0;
		
		private bool is_change = false;
		
		// 프로퍼티
		public int Curr{get{return curr;} set{curr= value;}}
		public int Carr{get{return carr;}set{carr = value;}}
		public int Cannon {get{return cannon;}set{cannon = value;}}
		
		// 필요하면 HUD 호출, 필요없으면 HUD 비활성화
		[SerializeField]
		[Tooltip("Canvas 화면 중 HUD")]
		private GameObject hud_laserhud;

		// 레이저 에너지 텍스트 반영
		[SerializeField]
		[Header("발사량")]
		private TextMeshProUGUI text_bar;		// 탄환수 / 전체탄환

		[SerializeField]
		[Header("Mode 변경 텍스트")]
		private TextMeshProUGUI text_mode;
		[Header("무기아이콘")]
		public Image weapon_img;		// 무기 이미지

		void Start() {
			
			text_bar.text = "";
			text_mode.text = "";
		}

		void Update() {
			Cannon = PlayerPrefs.GetInt("laser",0);

			Checkbullet();		
		}
		
		// HUD
		private void Checkbullet()
		{
			
			if (is_change) {text_mode.text ="Mineing Mode";
				text_bar.text = Cannon + " / " + PlayerPrefs.GetInt("maxlaser",0);
			}
			else {
				text_mode.text ="Weapon Mode";
				text_bar.text = curr + " / " + carr;
			}
			
		}

        internal void Is_change(bool v)
        {
            is_change = v;
        }
    }	
}

