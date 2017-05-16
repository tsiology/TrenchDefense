using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDrone : MonoBehaviour {
	
	float totalTime = 0.0f;
	public float speed = .5f;
	public float turnSpeed = 45.0f;
	float startingXPos;
	public float health = 2.0f;
	public Transform deadDrone;
	public Transform decayDrone;
	public GameObject deathAnime;
	public int killValue;

	// Use this for initialization
	void Start () {

		startingXPos = transform.position.x;
		//startingDirection = 0;


	}

	public void takeDamage(float multiplier)
	{
		health= health - 1 * multiplier;
	}


	// Update is called once per frame
	void Update () {
		Vector3 tankPos;
		totalTime += Time.deltaTime;
		//GameObject manager = GameObject.FindWithTag ("GameController"); 
		//GameObject player = GameObject.FindWithTag ("Player"); 
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		GameObject HUD = GameObject.FindWithTag ("HUD");



		if (health <= 0f) 
		{
			HUD.GetComponent<HUD> ().ScoreUpdate (killValue);

			Destroy(gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position + (transform.up*1.25f),  Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate(deathAnime,gameObject.transform.position + Vector3.up, camera.transform.rotation);

			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);

			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);
		}
			
		//if the tank has made it past the end of the level
		if (transform.position.y < -5) 
		{
			HUD.GetComponent<HUD> ().ScoreUpdate (-50);
			Destroy (gameObject);
		}

		//tank will swerve along a sinusoid
		//tankPos.x = transform.position.x + ((0.05f * Mathf.Cos (totalTime * 1.25f)));   
		tankPos.x = transform.position.x + ((0.05f * Mathf.Cos (totalTime * 1.25f)));   
		tankPos.y = transform.position.y; 
		tankPos.z = transform.position.z - speed;
		transform.position = tankPos;
 
		//rotate the tank as it swerves
		if (transform.position.x  > startingXPos) 
		{
			transform.Rotate (Vector3.up * turnSpeed * 1.0f * Time.deltaTime);
		}
		else //if ( transform.position.x + 1 > startingXPos)
		{
			transform.Rotate (Vector3.down * turnSpeed * 1.4f * Time.deltaTime);
		}

	}

	void OnTriggerEnter (Collider other)
	{

/*
		if(other.gameObject.tag == "Sandbag")
		{
			Destroy (gameObject);
			Destroy (other.gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);
			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);
		}
*/
		if (other.gameObject.tag == "Shield") 
		{
			Destroy (gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);
			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);
			
		}

		if(other.gameObject.tag == "PlayerBase")
		{
			Debug.Log("enemy ramming base");
			Destroy (gameObject);
			other.gameObject.GetComponent<PlayerBase> ().takeDamage (2);   //damage the base
			Transform dDrone = Instantiate (deadDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);
			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);
		}
	}
}
