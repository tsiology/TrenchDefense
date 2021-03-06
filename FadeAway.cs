/* 
attach this script to a game object to have it start fading from view slowly overtime. 

tgodwin
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour {

	public float fadeRate = 2.0f;
	private float startTime;
	public float fadeDelay=1.5f;

	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;
	}


	// Update is called once per frame
	void Update () 
	{
		if(Time.time - startTime >= fadeDelay)
		{	
			Color temp = gameObject.GetComponent<Renderer>().material.color;
			if (fadeRate >= 1) 
			{	
				//decrease the game objects alpha
				temp.a -= Time.deltaTime / fadeRate;
				gameObject.GetComponent<Renderer>().material.color = temp;
			}
		}
	}
		

	public void setFade(float rate)
	{
		fadeRate = rate;
	}

}
