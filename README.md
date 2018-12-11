# Hallym_UntiyProject (학교수업 프로젝트)


# 게임이름 : Simple Survive
### 팀원 : Jeehoon4545
### 장르 : 생존, RPG 
### 사용하는 언어 : C# Language
### 사용하는 프로그램 : MagicaVoxel, Blender, Unity 5

MagicaVoxel은 복셀3D 모델만드는 프로그램. [링크](http://ephtracy.github.io/) <br>
Blender는 복셀3D의 뼈대 만들어줄 용도로 사용. [링크](https://store.steampowered.com/app/365670/Blender/) <br>
Unity 5 게임엔진을 이용하여 게임을 제작. [링크](https://store.unity.com/kr/download?ref=personal) <br>
Sfxr 은 사운드 8비트를 만들어주는 프로그램 [링크](http://www.drpetter.se/project_sfxr.html) <br>\
[무료BGM](https://dova-s.jp/)<br>
{<br>
라이선스
음악은 자유롭게 상업적으로 이용할 수도 있고, 필요에 따라선 페이드아웃이나 루프 등 기본적인 편집도 허용한다고 나와 있네요. 심지어 음원 사용 시 출처 표기가 의무가 아니라는 조항,
사이트 자체의 이용약관과는 별개로, 작곡가 개인은 '음원 사용 시 자신에게도 알려달라'는 요구사항<br>
}<br>

<br>
<br>
#### 강좌 : [블렌더](http://dlgnlfus.tistory.com/326?category=704959), [매지카복셀](https://www.youtube.com/watch?v=VPnQNp-GU9I)
<br>
<br>

![Screen](./img/back_0.PNG)
![Screen](./img/back_1.PNG)

## 유용 오브젝트 누르고 Ctrl + Shift + F 누르면 오브젝트가 해당 보는 위치로 


### 1. 스토리
우주선에 탈출하여, 불시착을 한 상황이며 생존자는 단 1명 뿐이 었다.<br>
구조 요청을 보냈고 길어도 이 행성 시간 기준점으로 30일 걸릴거로 예상하고 그 때까지 혼자 살아남는 이야기이다.<br>
하지만 이곳 행성은 나무가 푸른색이며, 다른 동물들은 거의 호전적인 성향을 가지고 있으며, 주인공은 30일까지 무사히 버틸 수 있을까..<br>

### 2. 주요기능
 + 인벤토리 기능
 + 목마름, 배고픔 구현
 + 건축 시스템 도입
 + 네비게이션을 이용한 몬스터 행동패턴
 + 난이도 기능

### 3. 개발목적
 + 유니티의 다양한 기술들을 배우고 싶어서 생존게임을 만들게 되었다.
 
 
### 4. 핵심코드
 +<b> Cs_TitleMenu </b> = 타이틀 화면을 총괄리 해주는 스크립트이며, 타이틀 화면, 옵션 화면을 자유롭게 오갈수 있도록 한다.<br>
 +<b> Cs_PlayerControl </b> = 플레이어의 걷기, 달리기, 점프, 카메라 회전을 할 수 있다. 즉, 직접 플레이어를 조종하는 주요 스크립트<br>
 +<b> Cs_PlayerStatus </b> = 플레이어의 HP,SP,배고픔,목마름 등 HUD와 현 상태에 대해 알 수 있으며, 이 스크립트는 플레이어의 심장부 역활을 한다.<br>
 +<b> Cs_ItemPickup </b> = 아이템 상자에 넣는 스크립트이며, 아이템을 먹을 수 있도록 유도해주는 스크립트이다.<br>
 +<b> Cs_Inventory </b> = 아이템 창 UI를 구현한 스크립트이며, I 키를 누르면 인벤토리 창이 나타나게 된다.<br>
 +<b> Cs_Slot </b> = 아이템 창의 슬롯들에 넣어지는 스크립트이며, 아이템 추가, 드래그, 먹기 등 다양한 기능이 들어있는 스크립트이다.<br>
 +<b> Cs_DragSlot </b> = 아이템창 슬롯에 있는 아이템을 드래그 시킬 수 있는 기능 스크립트<br>
 +<b> Cs_Cloude </b> = 이 스크립트는 구름을 회전시키고, 태양을 이용하여 밤 낮을 추가시켜 실감나게 플레이 할 수 있도록 만들어진 스크립트이다.<br>
 +<b> Cs_SoundManager </b> = 사운드 총괄하는 스크립트이며, 모든 스크립트에서 사용 가능하도록 제작되어 있다.<br>
 +<b> LowPolyWater </b> = 파도치는 물 (AssetStore)에서 구해왔다.<br>
 +<b> Item </b> = 아이템 파일을 만들어 해당 아이템의 정보를 담은 스크립트<br>
 +<b> Enemy </b> = 몬스터 파일을 만들어 해당 몬스터의 정보를 담은 스크립트<br>
 +<b> Cs_EnemyHud </b> = 몬스터를 때릴 경우, 몬스터의 HP를 화면에 표시시키는 UI Script <br>
 +<b> Cs_CrosbarHUD </b> = 플레이어 조준점 만들어주는 스크립트<br>
 +<b> Cs_WeaponControl </b> = 마우스 왼쪽버튼클릭하면 총알
### 5. 



https://unity3d.com/kr/learn/tutorials/projects/survival-shooter/player-character?playlist=17144


