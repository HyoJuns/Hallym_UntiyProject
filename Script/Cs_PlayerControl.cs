using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player{
	// 플레이어 컨트롤 관련 스크립트
	public class Cs_PlayerControl : MonoBehaviour {
	//################################기본 변수######################################
		// 플레이어 관련 속도
		private float walkSpeed = 5f;		// 걷기 속도
		private float runSpeed = 10f;		// 달리기 속도
		private float applySpeed;			// 최종적으로 나올 속도
		
		private float jumpForce = 7f;		// 점프력
		private float crouchSpeed = 2f;	// 앉을 때 나오는 속도

		// 플레이어 상태 변수
		private bool isRun = false;			// 나는 달리고 있는가?
		private bool isGround = true;		// 점프를 할 수 있는 건가?
		private bool isWalk = false;

		// 움직임 체크 변수
		private Vector3 lastPos; 			// 전 프레임 플레이어 위치 표시

		// 앉을 경우 얼마나 앉을지 결정하는 변수
		private float crouchPosY = 0f;				// 앉을 높이 저장
		private float originPosY;					// 원래 있었던 높이 저장
		private float applyCrouchPosY;				// 최종적으로 앉을 높이 값

		// 카메라 민감도 설정
		public float lookSensitivity = 2f;			// 카메라의 민감도 설정 (Menu 설정 가능하게)

		// 카메라 회전 한계 설정
		private float cameraRotationLimit = 45f;	// 카메라 회전 제한선
		private float currentCameraRotationX = 0;	// 위 함수 45이면, 45도 위까지 볼 수 있다.

		// 컴퍼넌트 설정
		private Rigidbody myRigid;					// 플레이어 물리상태
		private BoxCollider box;	// 플레이어 콜라이더
		private Cs_PlayerStatus st;

		// 에니메이션 설정
		public Animator CrosBarHud;				// 중앙 크로스바 애니메이션
		public Animator anim;					// 플레이어 애니메이션
		// 크로스바 설정
		private float Accuracy;						// 크로스바인한 명중률
		
		float dealy = 1.0f;							// 달릴때 sp 다는 속도

		// 사운드 파일
		private string sound_walk = "se_walk";		// 걷는 사운드
		private string sound_run = "se_run";		// 달리는 사운드
		private string sound_jump = "se_jump";		// 점프 사운드
		
		float dealy_sound = 2.0f;					// 딜레이 사운드
		// SerializeField는 private 상태에도 Inspector 에 나올 수 있도록 해준다.
		[SerializeField] private Camera mainCam;	// 나의 시점

	//#############################################################################
		
		// 초기화
		void Start () {
			box = GetComponent<BoxCollider>(); 					// 플레이어 콜라이더 값 대입
			myRigid = GetComponent<Rigidbody>();				// 플레이어 물리적 값 대입
			st = GetComponent<Cs_PlayerStatus>();				// 플레이어 스텟관련 함수 대입


			applySpeed = walkSpeed;								// 걷는 속도 변경
			originPosY = mainCam.transform.localPosition.y;		// 플레이어 카메라의 로컬 y 값	

			applyCrouchPosY = originPosY;						// 시작 하기 전에 앉지는 않으니 기본값 설정
			
			// 민감도
			try{
				lookSensitivity += Cs_Difficulty.instance.Ls;	
			} catch(Exception e)
			{
				lookSensitivity = lookSensitivity;
			}
			Debug.Log("민감도 : " + lookSensitivity);

			Cs_SoundManager.instance.StopAllSE();
			Cs_SoundManager.instance.StopBGM();
			
		}
		
		// 프레임마다 실행
		void Update () {
			// 함수 진행\
			try{
				v_IsGround();	// 땅에 착지 했는지 함수
				v_TryJump();	// 점프에 대한 조건 함수
				v_TryRun();		// 달리기 조건 함수
				p_Move();		// 플레이어 이동하기
				p_MoveCheck();	// 플레이어 이동 체크
				p_CameraRotation(); 	// 플레이어 카메라 위 아래 회전
				p_CharacterRotation(); 	// 플레이어 좌우 회전
				dealy_sound -= Time.deltaTime;

				if(dealy_sound < 0.2f) dealy_sound = 2.0f;		// 사운드 딜레이
			}catch(Exception e)
			{
				Debug.Log("ERROR" + e.ToString());
			}
		}

		private void p_MoveCheck()
		{
			if(!isRun){
				if(Vector3.Distance(lastPos, transform.position) >= 0.01f)
					isWalk = true;
				else
					isWalk = false;

				v_WalkAnimation(isWalk);
				v_Walkplayer(isWalk);
				lastPos = transform.position;	// 현재 위치 비교

			}
		
		}
		// 달리기 조건
		private void v_TryRun()
		{
			// 쉬프트 키를 누르는 상태일 경우
			if(Input.GetKey(KeyCode.LeftShift) && st.current_sp >= 1)
				p_Running();		// 달리기 실행 함수
			else if(Input.GetKey(KeyCode.LeftShift) && st.current_sp < 1)
				p_RunningCancel();

				
			// 쉬프트 키를 뗄 경우
			if(Input.GetKeyUp(KeyCode.LeftShift))
				p_RunningCancel();	// 달리기 실행 취소 함수
		}
		

		// 점프를 했는가?
		private void v_TryJump()
		{
			// Space Key를 눌렀으며 땅에 착지상태인가를 확인한 후
			if(Input.GetKeyDown(KeyCode.Space) && isGround)
			{
				// 점프 함수를 실행한다.
				p_JUMP();	// 점프 실행 함수
			}
		}
		// 땅에 착지했는지에 대한 함수
		private void v_IsGround()
		{
			/*
			* 점프를 할 경우 내려올 때 땅에 있는지 확인한다.
			* 공중점프를 방지하기 위해 사용
			* RayCast는 어느 방향에서 어느 방향으로 거리만큼 레이저를 쏴는 함수를 말하며
			* 이곳에서는 밑에 레이저를 쏴서, 땅에 닿을 경우 점프를 다시 할 수 있도록 해주는 함수. 
			*/
			isGround = Physics.Raycast(transform.position, Vector3.down	// 이 객체의 지점에서 아래방향으로 레이저를 발사
			,box.bounds.extents.y + 0.2f);	// 콜라이더 영역의 2분의 1정도 y 길이만큼 레이저가 나간다.

			// 선을 이용하여 착지 표시
			if(isGround)
				Debug.DrawRay(transform.position,Vector3.down,Color.red,box.bounds.extents.y + 0.2f);
			else
				Debug.DrawRay(transform.position,Vector3.down,Color.green,box.bounds.extents.y + 0.2f);
			//Debug.Log("착지여부: " + isGround);
			//Debug.Log("Box coll : " + box.bounds.extents.y);
			// 0.2은 혹시모를 오차가 있으니 넉넉하게 적용하여 오차를 줄이기 위한 값이다.
		}

		public void v_WalkAnimation(bool _n)
		{
			CrosBarHud.SetBool("isWalk",_n);
		}
		public void v_RunAnimation(bool _n)
		{
			CrosBarHud.SetBool("isRun",_n);
		}
		
		public void v_Walkplayer(bool _n)
		{
			anim.SetBool("walk",_n);
		}
		public void v_RunPlayer(bool _n)
		{
			anim.SetBool("run",_n);
		}
		public void v_JumpPlayer()
		{
			anim.SetTrigger("t_jump");
		}
		public void v_DeadPlayer()
		{
			anim.SetTrigger("t_dead");
		}
		



	//################################p_XXXXX######################################

		private void p_JUMP()
		{
			// 움직이는 위치를 순식간에 위로 힘을 준다.
			myRigid.velocity = transform.up * jumpForce;
			v_JumpPlayer();
			Cs_SoundManager.instance.PlaySE(sound_jump);
		}
		// 달리기 취소 함수
		private void p_RunningCancel()
		{
			isRun = false;				// 조건 : 달리기 취소
			st.Used_sp = false;
			applySpeed = walkSpeed;		// 걷는 속도로 변경

			v_RunAnimation(isRun);
			v_RunPlayer(isRun);
			
		}

		// 달리기 실행 함수
		private void p_Running()
		{
			
			dealy -= Time.deltaTime;			

			if (dealy <= 0.2f){
				st.current_sp--;
				dealy = 1.0f;
				st.Used_sp = true;
			}
			isRun = true;				// 조건 : 달리기
			v_RunAnimation(isRun);
			v_RunPlayer(isRun);
			applySpeed = runSpeed;		// 달릴 속도로 변경
			
		}
		
		

		// 플레이어 이동
		private void p_Move()
		{
			// 키보드 방향키 or a,d를 누르면 1과 -1,0로 리턴해서 넣어준다.
			float _moveDirX = Input.GetAxisRaw("Horizontal");	// 좌우
			float _moveDirZ = Input.GetAxisRaw("Vertical");		// 상하
			// X 는 좌우, Z는 앞뒤

			Vector3 _moveHorizontal = transform.right * _moveDirX;
			// transform은 기본 컴포넌트의 회전,위치속성등 값과 right 값을 곱한다.
			// (1,0,0) * 1 or -1 or 0
			Vector3 _moveVertical = transform.forward * _moveDirZ;
			// (0,0,1) * 1 or -1 or 0

			Vector3 _Velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
			// normalized 는 (0.5,0,0.5) 로 되게 해준다. 
			// (1,0,0)(0,0,1) = (1,0,1) = 2
			// (1,0,1) = (0.5,0,0.5) = 1 즉 유니티에서 계산하기 편하게 해준다.

			myRigid.MovePosition(transform.position + _Velocity * Time.smoothDeltaTime);
			// MovePosition 이동위치로 이동한다. 순간이동하니 Time.deltaTime으로 끊어준다.	Time.deltaTime으로 하니까 프레임 다운 현상일어나서 smoothDeltaTime으로 변경
			
		}

		// 카메라 위, 아래 회전
		private void p_CameraRotation()
		{
			
			// Mouse는 2차원이므로 X와 Y만 있다.
			float _xRotation = Input.GetAxisRaw("Mouse Y");
			float _cameraRotationX = _xRotation * lookSensitivity; // 민감도 설정
			// 여기서 +는 흔히 FPS에 있는 마우스 Y 반전과 관련이 있다.
			currentCameraRotationX -= _cameraRotationX;	// +=  반전, -= 반전해제
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit,cameraRotationLimit);
			// 위 코드는 가둘수 있는 범위이다. 
			// x값이 -45 ~ 45도 사이에 가둔다.

			mainCam.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f,0f); // 마우스 위아래만 움직일 때 사용
			// localEulerAngles X, Y, Z 회전 기능
		}

		private void p_CharacterRotation()
		{
			
			float _yRotation = Input.GetAxisRaw("Mouse X");	// 좌우
			// 왜 Y좌표로 했냐면 유니티 가서 오브젝트 회전을 할 때 Y축을 회전화면 좌우로 이동하기 때문
			// 민감도 설정하여 회전 속도 지정
			Vector3 _charcterRotationY = new Vector3(0f,_yRotation,0f) * lookSensitivity;
			
			// Euler 쿼터니엄 값으로 변환 시켜준다. 위에 변수를
			myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_charcterRotationY));
			//Debug.Log("A "+myRigid.rotation);
			//Debug.Log("B " +myRigid.rotation.eulerAngles);
		}


	}	// class
}	// namespace