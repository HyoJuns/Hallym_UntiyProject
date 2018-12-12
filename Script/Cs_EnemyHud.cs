using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Cs_EnemyHud : MonoBehaviour {

		[Header("HUD")]
		[SerializeField] private Image image_Gauge;			// 몬스터 HP 나타내는 이미지
		[SerializeField] private Text enemy_text;			// 몬스터 이름
		public GameObject obj;								// 자신
		public Cs_Enemyinfomation e;										// 몬스터 정보
		
		private float delay = 15.0f;


		private void Update() {
			if(e == null)		// 몬스터 정보가 안들어온경우
			{
				HUD(false);			
			}else{
				GaugeUpdate();
				delay -= Time.deltaTime;
			}

			if(delay <= 0.2f){
				e = null;
				delay = 15.0f;
			}
				
		}

		/// <summary>
		/// HUD Update
		/// </summary>
		private void GaugeUpdate()
		{
			if (image_Gauge != null)
				if (e.Hp <= 0)
					image_Gauge.fillAmount = 0;
				else
					image_Gauge.fillAmount = (float)e.Hp / e.Maxhp;

			enemy_text.text = e.name + "(<color=yellow>" + e.Hp + "</color>) \n";
		}

		/// <summary>
		/// Enemy HUD를 보여주거나 숨긴다.
		/// </summary>
		public void HUD(bool on)
		{
			obj.SetActive(on);
		}

		public void Player_Attack(int damge)
		{
			// 플레이어가 몬스터를 향해 공격
			e.Damage(damge);
		}
}
