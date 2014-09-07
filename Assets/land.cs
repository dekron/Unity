using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class land : MonoBehaviour {

	public List<Vector3> port;
	//статистика
	public GUIText scoreDisplay;
	public int score=0;
	public int bonusCount=0;

	private bool pacman=false;
	//карты
	private FileInfo[] mapList;
	private string mapPath;
	public string[] masG;
	public TextAsset mapAsset;
	public string str;
	private string[] map;

	//меню
	public string menu="main";

	//options
	public float enemySpeed = 0.05f;
	public float playerSpeed = 0.1f;
	void Start () {

		mapPath= Application.dataPath + "/StreamingAssets/maps";
		//iOS
		//path = Application.dataPath + "/Raw/maps";
		//Android
		//path = "jar:file://" + Application.dataPath + "!/assets/maps";
		DirectoryInfo dir = new DirectoryInfo(mapPath);
		mapList = dir.GetFiles("*.txt");

		//0 шарик
		//1 блок
		//2 пусто
		//3 игрок
		//4 портал
		//5 призрак
		/*string[] mas = {  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
						{1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
						{1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1},
						{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
						{1,0,1,1,0,1,0,1,1,1,1,1,0,1,0,1,1,0,1},
						{1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,5,0,1},
						{1,1,1,1,0,1,1,1,2,1,2,1,1,1,0,1,1,1,1},
						{2,2,2,1,0,1,2,2,2,2,2,2,2,1,0,1,2,2,2},
						{1,1,1,1,0,1,2,1,1,2,1,1,2,1,0,1,1,1,1},
						{4,2,2,2,0,2,2,1,2,5,2,1,2,2,0,2,2,2,4},
						{1,1,1,1,0,1,2,1,1,1,1,1,2,1,0,1,1,1,1},
						{2,2,2,1,0,1,2,2,2,2,2,2,2,1,0,1,2,2,2},
						{1,1,1,1,0,1,2,1,1,1,1,1,2,1,0,1,1,1,1},
						{1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,5,0,0,1},
						{1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1},
						{1,0,0,1,3,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
						{1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1},
						{1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1},
						{1,0,1,1,1,1,1,1,0,1,0,1,1,1,1,1,1,0,1},
						{1,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
						{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		};
		masG = mas;*/
		loadLevel ();
	
	}

	void Update () {
		scoreDisplay.text = "Score: " + score;
		if (bonusCount <= 0) {
			Debug.Log("Выигрыш");		
		}
	}

	void OnGUI () {
		GUIStyle h2 = new GUIStyle ();
		h2.normal.textColor = Color.green;
		h2.fontSize = 16;
		GUIStyle h3 = new GUIStyle ();
		h3.normal.textColor = Color.green;
		h3.fontSize = 12;
		GUIText pauseH = GameObject.Find ("pause").guiText;
		switch (menu) {
		case "main":
			gameObject.transform.position = new Vector3(-9,100,21);
			pauseH.enabled=true;
			Time.timeScale = 0.0f;
			GUI.Label(new Rect (Screen.width/2-50, Screen.height/2-120, 100f, 50f),"Главное меню",h2);
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-80, 200f, 20f), "Продолжить")) {
				menu="game";
				Time.timeScale = 1.0f;
		
			}
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-60, 200f, 20f), "Карты")) {
				menu="maps";
			}
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-40, 200f, 20f), "Настройки")) {
				menu="options";
			}
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-20, 200f, 20f), "Выход")) {
				Application.Quit();
			}
			break;
		case "maps":
			pauseH.enabled=false;
			GUI.Box (new Rect (Screen.width/2-100, Screen.height/2-100, 200f, 200f),"");
			GUI.Label(new Rect (Screen.width/2-30, Screen.height/2-120, 100f, 50f),"Карты",h2);
			GUI.Label(new Rect (Screen.width/2-100, Screen.height/2-120, 100f, 50f),mapList.Length+"/10",h3);
			GUI.Label(new Rect (Screen.width/2+120, Screen.height/2-100, 100f, 100f),"Карты(19х21) в \n~appFolder\\StreamingAssets\\Maps\\*.txt\n" +
			          "-0 бонус\n-1 блок\n-2 пусто\n-3 игрок(макс. 1)\n-4 портал(макс. 2)\n-5 призрак",h2);
			var i = 0;
			foreach (FileInfo f in mapList) {
				if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-100+i*20, 200f, 20f), f.Name.Split('.')[0])) {
					map=f.OpenText().ReadToEnd().Split ("\n"[0]);;
					loadLevel();
					menu="game";
				}	
				i++;
			}	
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2+100, 200f, 20f), "<Назад")) {
				menu="main";
			}
			break;
		case "options":
			pauseH.enabled=false;
			GUI.Label(new Rect (Screen.width/2-35, Screen.height/2-120, 100f, 50f),"Настройки",h2);
			GUI.Label(new Rect (Screen.width / 2 -100, Screen.height / 2-100, 200f, 20f), "Скорость игрока:",h2);
			playerSpeed=float.Parse(GUI.TextField(new Rect (Screen.width / 2 -100, Screen.height / 2-80, 200f, 20f),(playerSpeed*100).ToString()))/100;

			GUI.Label(new Rect (Screen.width / 2 -100, Screen.height / 2-60, 200f, 20f), "Скорость призраков:",h2);
			enemySpeed=float.Parse(GUI.TextField(new Rect (Screen.width / 2 -100, Screen.height / 2-40, 200f, 20f),(enemySpeed*100).ToString()))/100;
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2, 200f, 20f), "OK")) {
				playerSpeed=(playerSpeed*100>25 || playerSpeed<0)?0.25f:playerSpeed;
				enemySpeed=(enemySpeed*100>25 || enemySpeed<0)?0.25f:enemySpeed;
				menu="main";
			}
			break;
		case "game":
			gameObject.transform.position = new Vector3(-9,-10,21);
			Time.timeScale = 1.0f;
			pauseH.enabled=false;
			if (GUI.Button (new Rect (0, 0, 100f, 20f), "Пауза")) {
				menu="main";

			}
			break;
		case "over":
			gameObject.transform.position = new Vector3(-9,100,21);
			GUI.Label(new Rect (Screen.width/2-35, Screen.height/2-120, 100f, 50f),"Конец игры",h2);
			if (GUI.Button (new Rect (Screen.width / 2 -100, Screen.height / 2-100, 200f, 20f), "OK")) {
				menu="main";
				loadLevel();
			}
			break;
				default:
						break;
		}


		

	}

	private void loadLevel(){
		bonusCount = 0;
		score = 0;
		GameObject[] bonusAll = GameObject.FindGameObjectsWithTag("bonus");
		foreach (GameObject item in bonusAll) {
			Destroy(item);
		}
		bonusAll = null;
		GameObject[] bordersAll = GameObject.FindGameObjectsWithTag ("border");
		foreach (GameObject item in bordersAll) {
			Destroy(item);
		}
		bordersAll= null;
		GameObject[] playerAll = GameObject.FindGameObjectsWithTag ("player");
		foreach (GameObject item in playerAll) {
			Destroy(item);
		}
		pacman=false;
		playerAll= null;
		GameObject[] enemyAll = GameObject.FindGameObjectsWithTag ("enemy");
		foreach (GameObject item in enemyAll) {
			Destroy(item);
		}
		enemyAll= null;
		GameObject[] portalAll = GameObject.FindGameObjectsWithTag ("portal");
		foreach (GameObject item in portalAll) {
			Destroy(item);
		}
		portalAll= null;
		if(map==null)map = mapAsset.text.Split ("\n"[0]);
		masG = map;
		
		Material bonusmat = Resources.Load("Bonus", typeof(Material)) as Material;
		Material pacmat = Resources.Load("Pacman", typeof(Material)) as Material;
		Material portalmat = Resources.Load("Portal", typeof(Material)) as Material;
		Material enemymat = Resources.Load("Enemy", typeof(Material)) as Material;
		Material bordermat = Resources.Load("Border", typeof(Material)) as Material;
		for (int i = 0; i < 21; i++)
		{
			for (int j = 0; j < 19; j++)
			{
				if(map[i][j].ToString()=="1"){
					var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.localScale = new Vector3(1,1,1);
					cube.transform.position = new Vector3(-j,-i);
					cube.renderer.material = new Material(bordermat);
					cube.tag="border";
				}
				if(map[i][j].ToString()=="0"){
					var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					cube.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
					cube.transform.position = new Vector3(-j,-i);	
					cube.tag="bonus";
					cube.collider.isTrigger=true;
					cube.renderer.material = new Material(bonusmat);
					bonusCount+=1;
				}
				if(map[i][j].ToString()=="3" && !pacman){
					var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					cube.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
					cube.transform.position = new Vector3(-j,-i);
					Rigidbody comp = cube.AddComponent<Rigidbody>();
					comp.useGravity = false;
					comp.freezeRotation =true;
					comp.isKinematic=true;
					cube.AddComponent ("PlayerController");
					cube.renderer.material = new Material(pacmat);
					pacman=true;
					cube.tag="player";
				}
				if(map[i][j].ToString()=="4"){
					var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
					cube.transform.position = new Vector3(-j,-i);
					port.Add(cube.transform.position);
					cube.tag="portal";
					cube.renderer.material = new Material(portalmat);
				}
				if(map[i][j].ToString()=="5"){
					var cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					cube.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
					cube.transform.position = new Vector3(-j,-i);
					cube.tag="enemy";
					cube.collider.isTrigger=true;
					cube.AddComponent ("enemyControl");
					cube.renderer.material = new Material(enemymat);
				}
			}
			
		}
		Time.timeScale = 0.0f;
	}
	
}
