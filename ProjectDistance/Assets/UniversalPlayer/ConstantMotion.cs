using UnityEngine;
using System.Collections;

public class ConstantMotion : MonoBehaviour {
	public int constantMotionSpeed = 1;
	private int playerState = 0;//May require greater scope
	// Use this for initialization
	void Start () 
	{

	}

	void OnTriggerEnter(Collider other) //For now, Load next level.  ---Later, add post-game screen (Maybe with just UI)
	{
		switch(other.tag)
		{
		case "Finish":
			playerState=1;
			Application.LoadLevel (Application.loadedLevel + 1);//Temporary measure- Only functions in this prototype
			break;
		case "Obstacle":
			playerState=-1;
			break;
		}

	}

	void Update () //Later, add player  states
	{
		switch(playerState)
		{
		case -1://Death
			Application.LoadLevel (Application.loadedLevel);//For now, only instantly restarts map
			break;
		case 0://Standard Gameplay
			transform.Translate (new Vector3(constantMotionSpeed, 0, 0)* Time.deltaTime, 
			                     Space.Self);
			break;
		case 1://Level Finish reached
			break;
		}
		//Relative motion rather than global
	}
}
