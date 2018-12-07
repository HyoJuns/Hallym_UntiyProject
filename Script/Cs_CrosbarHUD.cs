using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_CrosbarHUD : MonoBehaviour {


	[SerializeField]
	private Animator animator;

	private float gunAccuracy;	// 크로스헤어 상태에 따른 정확도

	[SerializeField] private GameObject go_CrossBarHUD;	// 크로스 헤어 제어


	public void WalkAnimation(bool _n)
	{
		animator.SetBool("isWalk",_n);
	}
	public void RunAnimation(bool _n)
	{
		animator.SetBool("isRun",_n);
	}
	public void CrouchAnimation(bool _n)
	{
		animator.SetBool("isCrouch",_n);
	}

	public void FireAnimation()
	{
		if(animator.GetBool("isWalk"))
			animator.SetTrigger("t_walkfire");
		else if (animator.GetBool("isCrouch"))
			animator.SetTrigger("t_crouchfire");
		else
			animator.SetTrigger("t_idlefire");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
