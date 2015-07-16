using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {//Completely remake
	public Transform VT_cameraTarget;

	void Update ()
	{
		//setting position and angle
		if (transform.localPosition.y <10)
		{
			transform.localPosition = new Vector3 (transform.localPosition.x,
			                                       transform.localPosition.y+.5f,
			                                       transform.localPosition.z);
		}
		else if (transform.localPosition.y >15)
		{
			transform.localPosition = new Vector3 (transform.localPosition.x,
			                                       transform.localPosition.y-.5f,
			                                       transform.localPosition.z);
		}
		//Add z axis adjustment + X axis

		transform.LookAt(VT_cameraTarget);
	}
}
