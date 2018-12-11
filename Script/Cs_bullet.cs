using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cs_bullet : MonoBehaviour {
	Cs_Enemyinfomation obj;
	Player.Cs_PlayerStatus staus;

	

	void OnTriggerEnter(Collider other) {
		try{
		if(other.gameObject.tag == "enemy")
		{
			obj = other.GetComponent<Cs_Enemyinfomation>();
			if (obj != null){
				obj.Damage(5);
				Debug.Log("Damage !!");
			}
		}
		}catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
}
