using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	//Player State variables
	private int playerState = 0;
	public bool driftEnabled = true;
	public bool trailEnabled =  true;
	//Motion variables
	private float PM_speed = 2f;//Distance per second
	private int PM_rotationRate = 180;
	private int VT_maxSpeed = 20;
	private float VT_accelRate = 1.02f;
	private int tiltLimitX = 30;//ConstantMotion
	//Trail variables
	public GameObject trail;
	public GameObject trailSpawnPoint;
	private float trailSpawnInterval = 1.2f;
	private int trailSize = 100;
	private ArrayList trailArray = new ArrayList();
	private int trailGrowRate=25;
	//Drift variables
	float xForce;
	float zForce;
	public ParticleSystem driftEffect;
	//Target Variables
	public GameObject spawnPointShell;
	public GameObject target;
	private ArrayList sPointList = new ArrayList ();
	private int activeTargets = 0;
	//Score Variables
	public Text scoreBox;

	void Start()
	{
		driftEffect.enableEmission = false;
		PlayerPrefs.SetInt ("Score", 0);
		targetList ();
	}
	void OnTriggerEnter (Collider other)
	{
		switch (other.tag)
		{
		case "LethalObject":
			Application.LoadLevel ("GameOver");
			break;
		case "JumpPad":
			other.gameObject.GetComponent<ParticleSystem>().Play();
			//transform.
			break;
		case "Target":
			other.gameObject.GetComponent<ParticleSystem>().Play();
			Debug.Log ("active");
			StartCoroutine(targetFade(other));
			other.enabled = false;
			trailSize +=trailGrowRate;
			activeTargets--;
			PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score") +1);
			//New High Score depending on level being played
			if (PlayerPrefs.GetInt("High Score") < PlayerPrefs.GetInt("Score"))//new High Score + save prefs
			{
				PlayerPrefs.SetInt("High Score", PlayerPrefs.GetInt("Score"));
				PlayerPrefs.Save(); 
			}
			switch (PlayerPrefs.GetString("level"))
			{
			case "Gameplay":
				if (PlayerPrefs.GetInt("High Score") < PlayerPrefs.GetInt("Score"))//new High Score + save prefs
				{
					PlayerPrefs.SetInt("High Score", PlayerPrefs.GetInt("Score"));
					PlayerPrefs.Save(); 
				}
				break;
			case "Map_Classic":
				if (PlayerPrefs.GetInt("Classic High Score") < PlayerPrefs.GetInt("Score"))//new Classic High Score + save prefs
				{
					PlayerPrefs.SetInt("Classic High Score", PlayerPrefs.GetInt("Score"));
					PlayerPrefs.Save(); 
				}
				break;
			case "Map_SkyReach":
				if (PlayerPrefs.GetInt("SkyReach High Score") < PlayerPrefs.GetInt("Score"))//new Classic High Score + save prefs
				{
					PlayerPrefs.SetInt("SkyReach High Score", PlayerPrefs.GetInt("Score"));
					PlayerPrefs.Save(); 
				}
				break;
			}
			scoreBox.text = "Score: " + PlayerPrefs.GetInt("Score");
			break;
		}
	}
	void Update () 
	{
		spawnTarget ();
		switch(playerState)
		{
		case -1://Freeze

			break;
		case 0://Standard motion
			constantMotion();
			mobileControl();
			//Test Input
			rotationControl();
			if(Input.GetKeyDown(KeyCode.Space) && driftEnabled)
			{
				xForce = PM_speed * Mathf.Sin (transform.eulerAngles.y*Mathf.Deg2Rad);
				zForce = PM_speed * Mathf.Cos (transform.eulerAngles.y*Mathf.Deg2Rad);
				PM_rotationRate = 270;
				playerState=1;
		
			}
			if(trailEnabled)
			{
				StartCoroutine("trailSpawn");
			}

			break;
		case 1://Drift state
			driftEffect.enableEmission = true;
			//trailSpawnPoint.transform.RotateAround(new Vector3(transform.eulerAngles.x,
			//                                                   transform.eulerAngles.y,
			//                                                   transform.eulerAngles.z), new Vector3(0,1,0), 40 * Time.deltaTime);
			transform.Translate(new Vector3(xForce,0,zForce)* Time.deltaTime,Space.World);
			rotationControl();
			mobileControl();
			if(Input.GetKeyUp(KeyCode.Space))
			{
				PM_rotationRate = 180;
				playerState=0;
				driftEffect.enableEmission = false;
			}
			break;
		}
	}


	//Motion
	private void constantMotion()//Prevents X Tilt: Kills motion unless level
	{
		if (PM_speed < VT_maxSpeed)
		{
			PM_speed =Mathf.Pow(PM_speed,VT_accelRate);;
		}
		if (transform.eulerAngles.x < tiltLimitX && 
		    transform.eulerAngles.x > -tiltLimitX)
		{
			transform.Translate (new Vector3(0, 0, PM_speed)* Time.deltaTime, 
			                     Space.Self);
		}
	}
	//General Input
	private void rotationControl (){
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(new Vector3 (0,PM_rotationRate,0) * Time.deltaTime, Space.Self);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(new Vector3 (0,-PM_rotationRate,0) * Time.deltaTime, Space.Self);
		}
	}
	private void mobileControl ()
	{
		for(var i = 0; i<Input.touchCount; i++)
		{
			//Left rotate
			if (Input.GetTouch(i).position.x < Screen.width / 4 )
			{
				if (Input.GetTouch(i).phase != TouchPhase.Ended)
				{
					transform.Rotate(new Vector3 (0,-PM_rotationRate,0) * Time.deltaTime, Space.Self);
				}
			}
			if (Input.GetTouch(i).position.x >= Screen.width / 4 && 
			    Input.GetTouch(i).position.x < Screen.width/2)
			{
				if (Input.GetTouch(i).phase != TouchPhase.Ended)
				{
					transform.Rotate(new Vector3 (0,PM_rotationRate,0) * Time.deltaTime, Space.Self);
				}
			}
			//Right rotate
			//Drift Activation
			if (Input.GetTouch(i).position.x >= (Screen.width / 2) && driftEnabled)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					xForce = PM_speed * Mathf.Sin (transform.eulerAngles.y*Mathf.Deg2Rad);
					zForce = PM_speed * Mathf.Cos (transform.eulerAngles.y*Mathf.Deg2Rad);
					PM_rotationRate = 270;
					playerState=1;
				}
				//Drift End
				else if (Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					driftEffect.enableEmission = false;
					PM_rotationRate = 180;
					playerState=0;
				}
			}
		}
	}
	//Trail Spawn Coroutine
	private IEnumerator trailSpawn(){
		float spawnDelay = (PM_speed * Time.deltaTime) * 
							(Mathf.Pow (Time.deltaTime, 2) / 
			 				(PM_speed * Time.deltaTime)) * 
							(trailSpawnInterval / PM_speed);
		yield return new WaitForSeconds (spawnDelay);
		if (trailArray.Count <= trailSize) {
			GameObject trailPiece = Instantiate (trail, trailSpawnPoint.transform.position, transform.rotation) as GameObject;//Adjust Quaternion to mimic player quaternion rotation
			//Push piece to array
			trailArray.Add (trailPiece);
			//limit spawn to max array size int
		} 
		if (trailArray.Count > trailSize) 
		{
			GameObject killTrail = (GameObject) trailArray[0];
			trailArray.RemoveAt(0);
			Destroy(killTrail);
		}
	}
	//Target Spawn and Collection
	private void targetList ()
	{
		for (var j = 0; j<spawnPointShell.transform.childCount; j++) 
		{
			sPointList.Add(spawnPointShell.transform.GetChild(j).gameObject);//Adds each child's transform to the array(sPointList)
		}
	}
	private void spawnTarget()
	{
		if (activeTargets < 2)//Max targets here
		{
			int pointIndex = Random.Range(0,sPointList.Count-1);
			GameObject point = sPointList[pointIndex]as GameObject;
			Instantiate(target,point.transform.position, Quaternion.identity);
			activeTargets++;//Disable for some challenge and fun!
		}
	}
	private IEnumerator targetFade(Collider other)
	{
		yield return new WaitForSeconds (5f);
		Destroy (other.gameObject);
	}
}
