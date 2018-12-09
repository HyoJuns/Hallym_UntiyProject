using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Cs_EnemyHud : MonoBehaviour {

		[Header("HUD")]
		[SerializeField] private Image image_Gauge;			// 몬스터 HP 나타내는 이미지
		[SerializeField] private Text enemy_text;			// 몬스터 이름
		public GameObject obj;								// 자신
		public Enemy e;										// 몬스터 정보
		



		private void Update() {
			if(e == null)		// 몬스터 정보가 안들어온경우
			{
				HUD(false);			
			}else{
				GaugeUpdate();
			}
		}

		/// <summary>
		/// HUD Update
		/// </summary>
		private void GaugeUpdate()
		{
			if (image_Gauge != null)
				if (e.hp <= 0)
					image_Gauge.fillAmount = 0;
				else
					image_Gauge.fillAmount = (e.hp / e.maxhp * 100);

			enemy_text.text = e.name;
		}

		/// <summary>
		/// Enemy HUD를 보여주거나 숨긴다.
		/// </summary>
		public void HUD(bool on)
		{
			obj.SetActive(on);
		}


}
