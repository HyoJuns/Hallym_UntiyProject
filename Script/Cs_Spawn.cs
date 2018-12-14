using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Spawn : MonoBehaviour {


	public static int num;
	public GameObject obj;		// 내 자신
	public float delay;
	private float maxdelay;
	public Cs_EnemyManager manager;
	public Enemy e;
	private void Start() {
		
		maxdelay = delay;
	}
	private void Update() {
		delay -= Time.deltaTime;
		
		GameObject[] n = GameObject.FindGameObjectsWithTag("enemy");

		num = n.Length;
		if(delay <= 0.4f && num <= 100)
		{
			// 리스폰
			manager.SpawnMonster(obj,e);
			delay = maxdelay;
			//num ++;
			//Debug.Log(num);
		}
		Debug.Log(num);
	}


}
