using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour {


	// Use this for initialization
	void Start() 
	{
		float xFactor = Screen.width / 3f;
		float yFactor = Screen.height  / 2f;
		Camera.main.rect = new Rect(0,0,1,xFactor/yFactor);

	}

	// Update is called once per frame
	void Update ()
	{	

		

	}
}
