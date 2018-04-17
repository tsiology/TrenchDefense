/*

Move an enemy forward in a straight line.
Control their health and collision detection.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScout : MonoBehaviour {
	
	float totalTime = 0.0f;
	public float speed = .35f;
	public float turnSpeed = 45.0f;
//	float startingXPos;
	public float health = 2.0f;
	public Transform deadDrone;
	public Transform decayDrone;
	public Transform deadWheel;
	public GameObject deathAnime;
	public int killValue=10;

	public AudioClip hitSound;
	private AudioSource sourceHit;

	// Use this for initialization
	void Start () 
	{
//		startingXPos = transform.position.x;
		sourceHit  = GetComponent<AudioSource> ();
	}

	public void takeDamage(float multiplier)
	{
		health = health - 1 * multiplier;
		if(health >= 1)
			sourceHit.PlayOneShot (hitSound, 0.8f);
	}


	// Update is called once per frame
	void Update () {

		Vector3 scoutPos;
		totalTime += Time.deltaTime;
		//GameObject manager = GameObject.FindWithTag ("GameController"); 
		//GameObject player = GameObject.FindWithTag ("Player"); 
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		GameObject HUDobj = GameObject.FindWithTag ("HUD");



		if (health <= 0f) 
		{
			HUDobj.GetComponent<HUD> ().ScoreUpdate (killValue);

			Destroy(gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position + (transform.up*1.25f),  Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate(deathAnime,gameObject.transform.position + Vector3.up, camera.transform.rotation);

			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);

			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);

			Transform wheel1 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 90) ) as Transform;
			Destroy (wheel1.gameObject, 3.0f);

			Transform wheel2 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 90) ) as Transform;
			Destroy (wheel2.gameObject, 3.0f);

		}
			
		//if the tank has made it past the end of the level
		if (transform.position.y < -5) 
		{
			HUDobj.GetComponent<HUD> ().ScoreUpdate (-50 * HUD.waves);
			Destroy (gameObject);
		}

		//scout will drive straight ahead
		scoutPos.x = transform.position.x;
		scoutPos.y = transform.position.y; 
		scoutPos.z = transform.position.z - speed*Time.deltaTime;
		transform.position = scoutPos;
	
	}

	void FixedUpdate()
	{
		//if flips over somehow
		if(transform.eulerAngles.z > 120 && transform.eulerAngles.z < 240 && transform.eulerAngles.x > -10 && transform.eulerAngles.x < 10)
		{
			//Debug.Log ("angles messed up: z = " + transform.eulerAngles.z);
			health = 0;
		}

		//if somehow in the air
		if (transform.position.y > 2.5f) 
		{
			health = 0;
		}

		//correct rotation if moved off track
		if (transform.eulerAngles.y < -181 || transform.eulerAngles.y > -179) 
		{
			Vector3 target = new Vector3(transform.rotation.x,-180,transform.rotation.z);
			transform.eulerAngles = AngleLerp(transform.eulerAngles,target,.75f);
		}
	}

	private Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
	{        
		float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
		float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
		float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
		Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
		return Lerped;
	}

	void OnTriggerEnter (Collider other)
	{
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

			Transform wheel1 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (wheel1.gameObject, 3.0f);

			Transform wheel2 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (wheel2.gameObject, 3.0f);
		}


		else if(other.gameObject.tag == "Shield")
		{
			GameObject player = GameObject.FindWithTag ("Player");
			player.GetComponent<MovePlayer> ().visualizeShield ();
			Destroy (gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (tmp, 1.2f);
			Destroy (dDrone.gameObject, 3.0f);
			Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (decDrone.gameObject, 3.0f);

			Transform wheel1 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (wheel1.gameObject, 3.0f);

			Transform wheel2 = Instantiate (deadWheel, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
			Destroy (wheel2.gameObject, 3.0f);
			
		}
		else if (other.gameObject.tag == "Player") 
		{
			Debug.Log ("Player rams enemy. 2 and 0.5 dmg");
			other.gameObject.GetComponent<MovePlayer> ().takeDamage (0.5f);
			takeDamage (2.0f);
		}
	}
}
