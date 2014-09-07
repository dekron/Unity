using UnityEngine;
using System.Collections;

public class enemyControl : MonoBehaviour {

	public string move ="stop";
	public string moveNext="stop";
	public string[] moveMas={"left","up","right","down"};
	public int normX;
	public int normY;
	public string masU;
	public land cam;
	public float timer=0;
	private int r;
	private double howLong = 1.0;
	private double nextUpdate = 0.0;
	private float leps=0.05f;
	void FixedUpdate () {

		cam= GameObject.Find("Camera").GetComponent<land>();
		leps = cam.enemySpeed > 0.10 ? 0.2f : 0.05f;
		if (Time.time > nextUpdate || move=="stop") {
			nextUpdate = Time.time + (Random.value * howLong);
			r=Random.Range (0, 4);
			if(r==4)r-=1;
		}

		timer -= Time.deltaTime;

		var normPos = gameObject.transform.position;
		normX = (int)Mathf.Round(-normPos.x);
		normY = (int)Mathf.Round(-normPos.y);

		moveNext=moveMas[r];

		var Port = cam.port;
		switch (moveNext) {
		case "right":
			masU = cam.masG[normY][normX+1].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<leps){
				int ss=0;
				if(Port[ss].x==-normX-1 && Port[ss].y==-normY) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="right" || move=="left") {
				if(masU.ToString()=="1" && Mathf.Abs(normX+normPos.x)<leps){
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
					if((cam.masG[normY+1][normX].ToString()=="1" || cam.masG[normY-1][normX].ToString()=="1") && Mathf.Abs(normY+normPos.y)<leps){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normY+normPos.y)>leps) Move (move,-normX,-normY);
					else move="right";}
			}
			
			break;
		case "left":
			masU = cam.masG[normY][normX-1].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<leps){
				int ss=0;
				if(Port[ss].x==-normX+1 && Port[ss].y==-normY) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="right" || move=="left") {
				if(masU.ToString()=="1" && Mathf.Abs(normX+normPos.x)<leps){
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
					if((cam.masG[normY+1][normX].ToString()=="1" || cam.masG[normY-1][normX].ToString()=="1") && Mathf.Abs(normY+normPos.y)<leps){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normY+normPos.y)>leps) Move (move,-normX,-normY);
					else move="left";}
			}	
			break;
		case "up":
			
			masU = cam.masG[normY-1][normX].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<leps){
				int ss=0;
				if(Port[ss].x==-normX && Port[ss].y==-normY+1) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="down" || move=="up") {
				if(masU.ToString()=="1" && Mathf.Abs(normY+normPos.y)<leps){
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
					if((cam.masG[normY][normX+1].ToString()=="1" || cam.masG[normY][normX-1].ToString()=="1") && Mathf.Abs(normX+normPos.x)<leps){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normX+normPos.x)>leps) Move (move,-normX,-normY);
					else move="up";}
			}
			
			break;
		case "down":
			masU = cam.masG[normY+1][normX].ToString();
			if(masU.ToString()=="4" && Mathf.Abs(normX+normPos.x)<leps){
				int ss=0;
				if(Port[ss].x==-normX && Port[ss].y==-normY-1) ss=1;
				gameObject.transform.position=new Vector3(Port[ss].x,Port[ss].y);
			}
			if(move=="stop" || move=="down" || move=="up") {
				if(masU.ToString()=="1" && Mathf.Abs(normY+normPos.y)<leps){
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
					if((cam.masG[normY][normX+1].ToString()=="1" || cam.masG[normY][normX-1].ToString()=="1") && Mathf.Abs(normX+normPos.x)<leps){
						move="stop";
					}
					Move (move,-normX,-normY);
				}else {
					if(Mathf.Abs(normX+normPos.x)>leps) Move (move,-normX,-normY);
					else move="down";}
			}
			break;
		default:
			break;
		}

	}

	void Move(string move,int x, int y){
//		gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		//if(move=="right" || move=="left") gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		//else gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
		
		switch (move) {
		case "right":
			gameObject.transform.position=new Vector3(gameObject.transform.position.x,y);
			gameObject.transform.Translate(new Vector3(-cam.enemySpeed, 0));
			break;
		case "left":
			gameObject.transform.position=new Vector3(gameObject.transform.position.x,y);
			gameObject.transform.Translate(new Vector3(cam.enemySpeed, 0));
			break;
		case "up":
			gameObject.transform.position=new Vector3(x,gameObject.transform.position.y);
			gameObject.transform.Translate(new Vector3(0, cam.enemySpeed));
			break;
		case "down":
			gameObject.transform.position=new Vector3(x,gameObject.transform.position.y);
			gameObject.transform.Translate(new Vector3(0, -cam.enemySpeed));
			break;
		case "stop":
			gameObject.transform.position=new Vector3(x,y);
			gameObject.transform.Translate(new Vector3());
			break;
		default:
			break;
		}
		
	}
}
