using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Cs_Enemyinfomation : MonoBehaviour {

	public Enemy e;			// 몬스터 정보를 여기로 불러온다.
	public GameObject obj;	// 해당 오브젝트
	
	// 열거형 몬스터 상태
	public enum Current_monster {IDLE, WALK, ATTACK, DEAD};
	public Current_monster currstate = Current_monster.IDLE;	// 기본상태

	RaycastHit hit;
	Animator anim;			// 몹 애니메이션
	int diff;				// 난이도
	//Vector3 fwd = transform.TransformDirection(Vector3.forward);

	bool is_Dead = false;		// 죽었는가?
	bool is_Attack = false;		// 공격했는가?
	
	private int maxhp;			// 몹 최대 채력
	private int hp;				// 몹 채력
	private int atk;			// 몹 공격력
	private int arm;			// 몹 방어력
	private int speed;			// 몹 속도

	float delay;				// 공격딜레이
	
	private int exp;			// 경험치

	private Cs_ItemPickup prefab;	// 드랍 아이템
	private Item item;				// 아이템
	private float random;		// 확률

	// Getter	HUD 보여주기 위해 만듬
	public int Maxhp{get{return maxhp;}}
	public int Hp{get{return hp;}}
	public int Atk{get{return hp;}}
	public int Arm{get{return hp;}}
	public int Exp{get{return hp;}}
	public int Speed{get{return speed;}}
	

	Vector3 pos; 							// 몬스터 현재위치
	Transform _transform;					// 적 transform 창
	Transform playertransform;				// 플레이어 위치

	public float traceDist = 15.0f;			// 추적 사정거리
	public float attackDist = 3.2f;			// 공격 사거리 

	

	// AI
	private NavMeshAgent nav;				// 네비게이션 메쉬
	/*
		이걸 사용하면 Rigid body 를 강제로 잠가버리며,
		Move Position, Rotation 잠겨짐
	 */





	#region hide
	void Start()
	{
		// 기본 셋팅
		obj = transform.gameObject;
		obj.name = e.name;
		
		maxhp = e.maxhp * diff;
		hp = maxhp * diff;
		atk = e.damge * diff;
		arm = e.armor * diff;
		item = e.item;
		delay = e.delay;
		exp = e.exp;
		prefab = e.prefab;
		random = e.random;
		pos = transform.position;
		anim = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();
		prefab.item = item;
		speed = e.speed;
		nav.speed = speed;

		_transform = this.gameObject.GetComponent<Transform>();
		playertransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
	
		StartCoroutine(this.CheckState());	// 코루틴 시작
		StartCoroutine(this.CheckStateForAction());
	}
	#endregion hide
	


	// 몬스터 상태 지속적 체크하는 코루틴 함수
	IEnumerator CheckState()
	{
		while (!is_Dead)
		{
			yield return new WaitForSeconds(0.2f);

			float dist = Vector3.Distance(playertransform.position,_transform.position);	// 거리 계산

			if(dist <= attackDist){
				currstate = Current_monster.ATTACK;		// 공격모드
			}
			else if (dist <= traceDist)
			{
				currstate = Current_monster.WALK;		// 추적됬으니 적이 쫓아옴
			}
			else{
				currstate = Current_monster.IDLE;		// 적 가만히 잇음.
			}
		}
	}

	// 채크 상태 작동별 유형
	IEnumerator CheckStateForAction()
	{
		while (!is_Dead)
		{
			switch(currstate)
			{
				case Current_monster.IDLE:		// 현재 가만히 있는 경우
					nav.Stop();					// 네비게이션 중지
					anim.SetBool("is_walk",false);
				break;
				case Current_monster.WALK:		// 추적된경우
					nav.destination = playertransform.position;
					nav.Resume();				// 경로 패치
					anim.SetBool("is_walk",true);
				break;
				case Current_monster.ATTACK:
				break;


				yield return null;
			}
		}
	}



	void Update() {
		Scan();
		Move();
		delete();
		
		
	}

	/// <summary>
	/// 몬스터 이동
	/// </summary>
	void Move()
	{
		transform.LookAt(transform.position);
		
	}

	/// <summary>
	/// 몬스터 제거
	/// </summary>
	void delete()
	{
		if (is_Dead)
		{
			Item_Drop(Setrandom());		// 랜덤 확률로 아이템
			Destroy(obj,1.5f); // 제거

			is_Dead = !is_Dead;
		}
	}

	/// <summary>
	/// 확률 반환 함수
	/// </summary>
	/// <returns></returns>
	float Setrandom()
	{
		return Random.Range(random,1.0f);
	}

	/// <summary>
	/// 몹을 죽여서 아이템 증가
	/// </summary>
	/// <param name="rnd"></param>
	public void Item_Drop(float rnd)
	{
		float n = Random.Range(0.0f, 1.0f);

		// 확률 성공
		if ( rnd > n){
			//Debug.Log("Drop " + rnd);
			Instantiate(prefab,pos,Quaternion.Euler(0f,0f,0f));
		}

	}

	/// <summary>
	/// 플레이어를 스캔
	/// </summary>
	void Scan()
	{
		if(e.enemytype == 0)
		{
			// 중립 동물
			return;
		}
		else{
			
			attack();
		}
	}

	/// <summary>
	/// 공격
	/// </summary>
	void attack()
	{
		Debug.DrawRay(obj.transform.position,obj.transform.forward,Color.blue,5.0f);
		if(Physics.Raycast(obj.transform.position, obj.transform.forward, out hit, 5.0f))
		{
			Debug.Log("tag : " + hit.transform.tag);
			if(hit.transform.tag == "Player")
			{
				Debug.Log("Attack");
				anim.SetTrigger("attack");
			}
		}
	}
	/// <summary>
	/// 적에게 데미지를 주었다.
	/// </summary>
	public void Damage(int dmg)
	{
		if(is_Dead) return;		// 죽으면 효과 없음

		int n = arm;
		if(n - dmg > 0)
		{
			// 적 방어구가 나의 공격력보다 강할 경우
			Debug.Log("방어구로 인해 막혔습니다.");
		}else{
			n -= dmg;
			hp += n;			// n값이 마이너스 이므로 계산
			Debug.Log("enemy Hp " + hp + " Player Dmg " + n);
		}
		pos = transform.position;
		// 죽었는가?
		if(hp <= 0 ){
			anim.SetTrigger("dead");
			is_Dead = true;
		}
	}
}
