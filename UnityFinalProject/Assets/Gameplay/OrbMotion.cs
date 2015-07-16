using UnityEngine;
using System.Collections;

public class OrbMotion : MonoBehaviour {
	private int motionState = 0;
	private int moveRate =  5;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine ("stateShift");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (motionState) {
		case 0:
			transform.Translate (new Vector3 (0, 0, moveRate) * Time.deltaTime, Space.Self);
			break;
		case 1:
			transform.Translate (new Vector3 (1,0, moveRate) * Time.deltaTime, Space.Self);
			break;
		case 2:
			transform.Translate (new Vector3 (moveRate, 0, 0) * Time.deltaTime, Space.Self);
			break;
		case 3:
			transform.Translate (new Vector3 (-moveRate, 0, moveRate) * Time.deltaTime, Space.Self);
			break;
		case 4:
			transform.Translate (new Vector3 (-moveRate, 0, -moveRate) * Time.deltaTime, Space.Self);
			break;
		case 5:
			transform.Translate (new Vector3 (0, 0, -moveRate) * Time.deltaTime, Space.Self);
			break;
		}
	}
	private IEnumerator stateShift ()
	{
		yield return new WaitForSeconds (5f);
		motionState = Random.Range (0, 5);
		StartCoroutine ("stateShift");
	}
}
