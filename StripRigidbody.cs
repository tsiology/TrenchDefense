using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripRigidbody : MonoBehaviour {
	
	public bool ignoreMe=true;

	// Use this for initialization
	void Start () 
	{
		
	}


	// Update is called once per frame
	void Update () {
	}
		

	void removeRB( )
	{
		if (gameObject.GetComponent<Rigidbody>()) 
		{			
			ignoreMe = true;
		}
	}

	public bool getIgnoreMe()
	{
		return ignoreMe;
	}
}
