/*

Attach to an enemy prefab to have it move and swereve along a sinusoidal path.
Also controls enemy health, collision detection with the player, and death conditions.

tgodwin

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDrone : MonoBehaviour {
	
	float totalTime;
	public float speed = 5f;
	public float turnSpeed = 5f;
	float startingXPos;
	public float health = 2.0f;
	public Transform deadDrone;
	public Transform decayDrone;
	public GameObject deathAnime;
	public GameObject deathSplash;
	public int killValue;
	public int direction;
	public AudioClip hitSound;
	public AudioClip deathSound;
	private AudioSource sourceHit;
	private AudioSource sourceDead;

	void Awake()
	{
		sourceHit  = GetComponent<AudioSource> ();
		sourceDead = GetComponent<AudioSource> ();
	}


	// Use this for initialization
	void Start () 
	{
		totalTime = 0.0f;
		startingXPos = transform.position.x;
		GameObject GM = GameObject.FindWithTag ("GameController");
		if (GM.GetComponent<SpawnDrone>().enemyTotalSpawn % 2 == 1 && HUD.waves < 9) 
		{
			direction = -1;
		}
		else
		{
			direction = 1;
		}	
	}

	public void takeDamage(float multiplier)
	{
		sourceHit.PlayOneShot (hitSound, 1f);
		health= health - 1 * multiplier;
	}


	// Update is called once per frame
	void Update () {
		Vector3 tankPos;
		totalTime += Time.deltaTime;

		if (health <= 0f) 
		{
			sourceDead.PlayOneShot (deathSound, 1f);
			GameObject HUD = GameObject.FindWithTag ("HUD");
			HUD.GetComponent<HUD> ().ScoreUpdate (killValue);

			spawnDeadDrone ();
			Destroy(gameObject);
		}
			
		//if the tank has made it past the end of the level
		if (transform.position.z < -20) 
		{
			GameObject HUDobj = GameObject.FindWithTag ("HUD");
			HUDobj.GetComponent<HUD> ().ScoreUpdate (-50 * (HUD.waves+1));
			Destroy (gameObject);
		}

		//tank will swerve along a sinusoid   
		tankPos.x = transform.position.x + direction * ((3.5f * Mathf.Cos (totalTime))) * Time.deltaTime;   
		tankPos.y = transform.position.y; 
		tankPos.z = transform.position.z - speed * Time.deltaTime;
		transform.position = tankPos;
 
		//rotate the tank as it swerves
		if (transform.position.x - 0.0f  > startingXPos)// && transform.rotation.y <= 130) 
		{
			transform.Rotate (Vector3.up * turnSpeed * Time.deltaTime);
		}
		else if ( transform.position.x + 0.0f < startingXPos)// && transform.rotation.y >= 230)
		{
			transform.Rotate (Vector3.down * turnSpeed * Time.deltaTime);
		}
			
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
	}


	void OnTriggerEnter (Collider other )
	{
		if(other.gameObject.tag == "Sandbag")
		{
			Destroy (gameObject);
			Destroy (other.gameObject);
			spawnDeadDrone ();
		}
		else if(other.gameObject.tag == "Shield")
		{
			GameObject player = GameObject.FindWithTag ("Player");
			if(player)
				player.GetComponent<MovePlayer> ().visualizeShield ();
			Destroy (gameObject);
			spawnDeadDrone ();
		}
		else if (other.gameObject.tag == "Player") 
		{
			Debug.Log ("Player rams enemy. 2 and 0.5 dmg");
			other.gameObject.GetComponent<MovePlayer> ().takeDamage (0.5f);
			takeDamage (2.0f);
		}
	}

	public void spawnDeadDrone()
	{
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		Transform dDrone = Instantiate (deadDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
		Transform decDrone = Instantiate (decayDrone, transform.position,  Quaternion.Euler (0, 0, 0) ) as Transform;
		GameObject tmp = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, camera.transform.rotation);

		Invoke ("splash", 0.65f);


		Destroy (dDrone.gameObject, 3.5f);
		Destroy (decDrone.gameObject, 3.5f);		
		Destroy (tmp, 1.2f);
	}

	void splash()
	{
		GameObject mark = Instantiate (deathSplash, gameObject.transform.position + 2*Vector3.up + Vector3.right + 1*Vector3.forward, Quaternion.Euler (90, 0, 0));
	}

}
