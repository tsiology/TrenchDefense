using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAce : MonoBehaviour {
	
	public float health = 12.0f;
	public Transform deadDrone;
	public Transform decayDrone;
	public GameObject deathAnime;
	public int killValue=250;
	public float power = 3000f;
	public float speed=2;
	public float turnSpeed=2;
	public int moveOption=0;
	public float moveRate;
	public float lastMove;
	public Transform bullet;

	private Transform target;
	private Quaternion targetRotation;
	

	// Use this for initialization
	void Start () 
	{
		lastMove = 0;
		moveRate = 4.0f;
		turnSpeed = 2f;
	}

	public void takeDamage(float multiplier)
	{
		health= health - 1 * multiplier;
	}


	// Update is called once per frame
	void Update ()
	{

		Vector3 tankPos;
		GameObject enemyBase = GameObject.FindWithTag ("EnemyBase"); 
		GameObject playerBase = GameObject.FindWithTag ("PlayerBase"); 
		GameObject player = GameObject.FindWithTag ("Player"); 
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		GameObject HUD = GameObject.FindWithTag ("HUD");

		if (health <= 0f) {
			HUD.GetComponent<HUD> ().ScoreUpdate (killValue);

			Destroy (gameObject);
			Transform dDrone = Instantiate (deadDrone, transform.position + (transform.up * 1.25f), Quaternion.Euler (0, 0, 0)) as Transform;
			GameObject tmp = Instantiate(deathAnime,gameObject.transform.position + Vector3.up, camera.transform.rotation);

			Destroy (tmp, 3.0f);
			Destroy (dDrone.gameObject, 5.0f);

			Transform decDrone = Instantiate (decayDrone, transform.position, Quaternion.Euler (0, 0, 0)) as Transform;
			Destroy (decDrone.gameObject, 5.0f);
		}
			
		//if the tank has made it past the end of the level
		if (transform.position.y < -5) {
			HUD.GetComponent<HUD> ().ScoreUpdate (-50);
			Destroy (gameObject);
		}

		//Ace will drive forward a certain distance in different directions ahead
		//tankPos.x = transform.position.x;
		//tankPos.y = transform.position.y; 
		//tankPos.z = transform.position.z - speed;
		//transform.position = tankPos;
		
		//time to rotate to new target
		if (Time.time - lastMove > moveRate && player != null && playerBase != null) 
		{
			lastMove = Time.time;
			if (moveOption % 4 == 0 || moveOption % 4 == 2 ) 
			{
				moveOption++;
				targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
				Invoke("fire",1f);
				Invoke("fire",3f);
				//targetRotation *= Quaternion.Euler(0,-90,0);
			}
			else if (moveOption % 4 == 1) 
			{
				moveOption++;
				targetRotation = Quaternion.LookRotation(playerBase.transform.position - transform.position);
				Invoke("fire",2f);
				//targetRotation *= Quaternion.Euler(0,-90,0);
			}
			else
			{
				moveOption++;
				targetRotation = Quaternion.LookRotation(enemyBase.transform.position - transform.position);
				//targetRotation *= Quaternion.Euler(0,-90,0);
			}
		}
		transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,turnSpeed*Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * speed;
	}


	void fire()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet, transform.position + (transform.forward*2.2f) + (transform.up) - (transform.right*0.15f) , transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power);
	}


	/*
	void OnTriggerEnter (Collider other)
	{

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
	*/
}
