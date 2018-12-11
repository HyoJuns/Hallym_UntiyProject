using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Spawn : MonoBehaviour {

	public GameObject obj;		// 내 자신
	public float delay = 10.0f;
	public Cs_EnemyManager manager;
	public Enemy e;

	private void Update() {
		delay -= Time.deltaTime;

		if(delay <= 0.4f)
		{
			// 리스폰
			manager.SpawnMonster(obj,e);
			delay = 10.0f;
		}
	}


}
