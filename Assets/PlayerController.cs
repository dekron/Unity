using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveX;
	public float moveY;
	public string move ="stop";
	public string moveNext="stop";

	public float stX;
	public float stY;
	public int normX;
	public int normY;
	public string masU;
	public land cam;

	void Update(){

		if(Input.GetKeyUp(KeyCode.Escape)){
			if(cam.menu =="main") {
				cam.menu ="game";
			}
			else if(cam.menu =="game"){
				cam.menu ="main";
			}
		}
	}

	void FixedUpdate () {
		cam= GameObject.Find("Camera").GetComponent<land>();
		stX = Input.GetAxis("Horizontal");
		stY = Input.GetAxis("Vertical");

		//right
		if(stX > 0) {
			moveNext ="right";
		}
		//left
		if(stX < 0) {
			moveNext ="left";
		}
		//up
		if(stY > 0) {
			moveNext ="up";
		}
		//down
		if(stY < 0) {
			moveNext ="down";
		}

		var normPos = gameObject.transform.position;
		normX = (int)Mathf.Round(-normPos.x);
		normY = (int)Mathf.Round(-normPos.y);

		var Port = cam.port;
		switch (moveNext) {
		case "right":
			masU = cam.masG[normY][normX+1].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<0.02){
				int ss=0;
				if(Port[ss].x==-normX-1 && Port[ss].y==-normY) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="right" || move=="left") {
				if(masU.ToString()=="1" && Mathf.Abs(normX+normPos.x)<0.02){
					move="stop";
					Move (move,-normX,-normY);
				}
				else{
					move="right";
					Move (move,-normX,-normY);
				}
			}
			else {
				if (masU.ToString()=="1" ) {
					if((cam.masG[normY+1][normX].ToString()=="1" || cam.masG[normY-1][normX].ToString()=="1") && Mathf.Abs(normY+normPos.y)<0.02){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normY+normPos.y)>0.02) Move (move,-normX,-normY);
					else move="right";}
			}

			break;
		case "left":
			masU = cam.masG[normY][normX-1].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<0.02){
				int ss=0;
				if(Port[ss].x==-normX+1 && Port[ss].y==-normY) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="right" || move=="left") {
				if(masU.ToString()=="1" && Mathf.Abs(normX+normPos.x)<0.02){
					move="stop";
					Move (move,-normX,-normY);
				}
				else{
					move="left";
					Move (move,-normX,-normY);
				}
			}
			else {
				if (masU.ToString()=="1") {
					if((cam.masG[normY+1][normX].ToString()=="1" || cam.masG[normY-1][normX].ToString()=="1") && Mathf.Abs(normY+normPos.y)<0.02){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normY+normPos.y)>0.02) Move (move,-normX,-normY);
					else move="left";}
			}	
			break;
		case "up":

			masU = cam.masG[normY-1][normX].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<0.02){
				int ss=0;
				if(Port[ss].x==-normX && Port[ss].y==-normY+1) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="down" || move=="up") {
				if(masU.ToString()=="1" && Mathf.Abs(normY+normPos.y)<0.02){
					move="stop";
					Move (move,-normX,-normY);
				}
				else{
					move="up";
					Move (move,-normX,-normY);
				}
			}
			else {
				if (masU.ToString()=="1") {
					if((cam.masG[normY][normX+1].ToString()=="1" || cam.masG[normY][normX-1].ToString()=="1") && Mathf.Abs(normX+normPos.x)<0.02){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normX+normPos.x)>0.02) Move (move,-normX,-normY);
					else move="up";}
			}

			break;
		case "down":
			masU = cam.masG[normY+1][normX].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<0.02){
				int ss=0;
				if(Port[ss].x==-normX && Port[ss].y==-normY-1) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="down" || move=="up") {
				if(masU.ToString()=="1" && Mathf.Abs(normY+normPos.y)<0.02){
					move="stop";
					Move (move,-normX,-normY);
				}
				else{
					move="down";
					Move (move,-normX,-normY);
				}
			}
			else {
				if (masU.ToString()=="1") {
					if((cam.masG[normY][normX+1].ToString()=="1" || cam.masG[normY][normX-1].ToString()=="1") && Mathf.Abs(normX+normPos.x)<0.02){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normX+normPos.x)>0.02) Move (move,-normX,-normY);
					else move="down";}
			}
			break;
			default:
			break;
		}

     
	}

	void Move(string move,int x, int y){
		gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		if(move=="right" || move=="left") gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		else gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;

		switch (move) {
		case "right":
			gameObject.transform.Translate(new Vector3(-cam.playerSpeed, 0));
			break;
		case "left":
			gameObject.transform.Translate(new Vector3(cam.playerSpeed, 0));
			break;
		case "up":
			gameObject.transform.Translate(new Vector3(0, cam.playerSpeed));
			break;
		case "down":
			gameObject.transform.Translate(new Vector3(0, -cam.playerSpeed));
			break;
		case "stop":
			gameObject.transform.position=new Vector3(x,y);
			gameObject.transform.Translate(new Vector3());
			break;
		default:
			break;
		}
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "bonus") {
			Destroy(other.gameObject);
			cam.score +=100;
			cam.bonusCount-=1;

		}
		if (other.gameObject.tag == "enemy") {
			Destroy(gameObject);
			Time.timeScale = 0.0f;
			cam.menu ="over";
		}
	}
}



