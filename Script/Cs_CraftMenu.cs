using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// CraftMenu
[System.Serializable]
public class Craft{         // 그릇이 될 제작
	public string craftname;		// 제작이름
	public GameObject go_Prefab; //실제 설치될 프리팹
    public GameObject go_PreviewPrefab; // 미리보기 프리팹
    public int needscore;               // 필요 스코어
}

public class Cs_CraftMenu : MonoBehaviour {
    
	//상태변수
    private bool isActivated = false;			// 활성화상태인가
    private bool isPreviewActivated = false;	// 미리 활성화?
    
	[SerializeField]
    private GameObject go_BaseUi; // 기본 베이스 UI

	// 탭키
	[Header("크래프트메뉴")]
	[Tooltip("Craft")]
    [SerializeField]
    public Craft[] craft_fire; // Tab Base

	private GameObject go_Preview; //미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수
    private int need_score;        // 필요 스코어
	[SerializeField]
    private Transform tf_Player; //플레이어 위치

	 //Raycast 필요 변수 선언
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

	[SerializeField]
    private float range; //범위

    // TextMash
    public TextMeshProUGUI needtxt;
    float delay =  5.0f;
    
 
	/// <summary>
	/// 크레프트 메뉴에 있는 조합할 거 클릭
	/// </summary>
	/// <param name="_slotNumber"></param>
	public void SlotClick(int _slotNumber)
	{
        if(craft_fire[_slotNumber].needscore > Player.Cs_PlayerStatus.Point_Score){needtxt.text ="NEED Point : " + craft_fire[_slotNumber].needscore; return;}
		
        // 제작할 재료 클릭시
		go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + (tf_Player.forward * 2f), Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        need_score = craft_fire[_slotNumber].needscore;
        isPreviewActivated = true;
        WeaponList.Cs_WeaponControl.waitfire = true;
        Cursor.lockState = CursorLockMode.Locked;
        
        go_BaseUi.SetActive(false);
	}

	void Update()
    {
        delay -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window();	// CraftMenu UI 틀기

        if (isPreviewActivated)
            PreviewPositionUpdate();	//

        if (Input.GetButtonDown("Fire1")) //마우스누르면
            Build(); // 건설

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();	// 취소

        if(delay <= 0.2f){
            delay = 5.0f;
            needtxt.text = "";
        }
    }
	
	/// <summary>
	/// 건설
	/// </summary>
	private void Build() 
    {
        
        //if (isPreviewActivated && go_Preview.GetComponent<Cs_PreviewObject>().isBuildable())
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, tf_Player.position , Quaternion.identity); // 생성
            Destroy(go_Preview);	// 건설전 미리 보여준 프리팹 지우기
            Debug.Log(isPreviewActivated);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            Player.Cs_PlayerStatus.Point_Score -= need_score;
            WeaponList.Cs_WeaponControl.waitfire = false;
        }

    }

	/// <summary>
	/// 건설 위치 지정해주는 위치정보
	/// </summary>
	private void PreviewPositionUpdate()
    {
        Debug.DrawRay(tf_Player.position, tf_Player.forward,Color.black,range);
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = tf_Player.position;  // 레이저 맞은 곳 좌표

                go_Preview.transform.position = _location;
               
            }

        }

    }	

	/// <summary>
	/// 건설을 취소 시킬려고 함
	/// </summary>
	private void Cancel() //취소시키는 것
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        WeaponList.Cs_WeaponControl.waitfire = false;
        go_BaseUi.SetActive(false);
    }

	/// <summary>
	/// 크래프트창
	/// </summary>
	private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

	private void OpenWindow()
    {
        isActivated = true;
        WeaponList.Cs_WeaponControl.waitfire = true;
        Cursor.lockState = CursorLockMode.None;
        go_BaseUi.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        WeaponList.Cs_WeaponControl.waitfire = false;
        Cursor.lockState = CursorLockMode.Locked;
        go_BaseUi.SetActive(false);
    }

} // class
