using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;		// TextMashPro

public class Cs_tutorial : MonoBehaviour {

	[SerializeField]
	float delay = 100.0f;
	
	[Header("3D TextMashPro Object")]
	[Tooltip("TextMash 이용한 텍스트오브젝트")]
	public TextMeshPro txt;

	// Update is called once per frame
	void Update () {
		delay -= Time.deltaTime;

		if(delay <= 75.0f)
			change();
		else if(delay > 75.0f && delay <=45.0f)
			change2();
		else if(delay > 45.0f && delay <= 20.0f)
			change3();

		if(delay <= 1.0f)		// 딜레이 지나면 자동으로 삭제
			Destroy(txt);
	}

	void change()
	{
		txt.text = "Day 30 -> Winner\n =Status= \n Lv (Level), Hp(health), Sp(Energy), Atk(Damage), Arm(armor) ";
	}

	void change2(){
		txt.text ="Level Up = Status Point 3+ \n Score = Build ";
	}

	void change3()
	{
		txt.text = "Laser - Mineing mode, Bullet - Normal Mode";
	}
	
}
