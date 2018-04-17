/*

Script for controlling player movement, health, and collision with enemies

tgodwin
6/4/17

*/

using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {
	
	public float speed ;
	public float rotateSpeed;
	public float playerHealth;
	public GameObject deathAnime;
	public GameObject deadPlayer;
	public GameObject shield;
	public bool allowShield;
	public float shieldLife = 1.75f;
	public float shieldCooldown;
	public float lastShield = 0.0f;
	private bool canCollide;
	private bool isCollide;
	private float collideTime = 0.5f;
	private float collideTimer = 0;


	public AudioClip shieldSound;
	private AudioSource sourceShield;

	void Awake()
	{
		sourceShield = GetComponent<AudioSource> ();
	}

	public void Start()
	{
		
		
		//max armor upgrades for waves 8+
		if(HUD.waves >= 8)
		{
			speed = 5;
			playerHealth = 6.5f;

			//start with 0.0 to spawn shield immediately
			allowShield = true;
			shieldCooldown = 0.0f;
		}	
			
		else if(HUD.waves >= 6)
		{
			speed = 5;
			playerHealth = 6.5f;
			allowShield = false;		
		}

		//waves 5+
		else if(HUD.waves >= 5)
		{
			speed = 4;
			playerHealth = 6.5f;
			allowShield = false;		
		}

		//2+
		else if(HUD.waves >= 3)
		{
			speed = 4;
			playerHealth = 4.5f;
			allowShield = false;		
		}

		//2+
		else if(HUD.waves >= 2)
		{
			speed = 3;
			playerHealth = 4.5f;
			allowShield = false;		
		}

		//wave 1 default
		else
		{
			speed = 3;
			playerHealth = 3.5f;
			allowShield = false;
		}		
	}//end of Start()

	public void takeDamage(float multiplier)
	{
		GameObject HUD = GameObject.FindGameObjectWithTag ("HUD");
		playerHealth = playerHealth - 1 * multiplier;
		HUD.GetComponent<HUD> ().VisualizeRedDmg (0.5f);
	}


	void Update () 
	{
		if (playerHealth <= 0) 
		{
			GameObject explosion = Instantiate(deathAnime,gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (explosion, 3.5f);
			GameObject dead = Instantiate (deadPlayer, transform.position,  transform.rotation * Quaternion.Euler(0,90,0));
			Destroy (dead, 4.0f);
			Destroy(gameObject);
		}
			
		collideTimer += Time.deltaTime;
		if (collideTimer >= collideTime) 
		{
			canCollide = true;
		} 
		else 
		{
			canCollide = false;
		}


		// Move forward / backward
		CharacterController controller = GetComponent<CharacterController>();		
		Vector3 right = transform.TransformDirection(Vector3.right);
		float curSpeed = speed * Input.GetAxis ("Horizontal");
		controller.SimpleMove(right * curSpeed);
		if (curSpeed > 0f)
		{
			rockLeft ();
		}
		else if(curSpeed < 0f)
		{
			rockRight ();
		}
		else
		{
			rockCenter ();
		}

		if(Input.touchCount > 0)
		{
			if(Input.GetTouch(0).position.x < (Screen.width)/2.0f - 300)
			{
				curSpeed = speed * -1;
				controller.SimpleMove(right * curSpeed);
				rockRight ();
			}
			else if(Input.GetTouch(0).position.x > (Screen.width)/2.0f + 300)
			{
				curSpeed = speed * 1;
				controller.SimpleMove(right * curSpeed);
				rockLeft ();
			}

		}
		//recenter rotation if no movement
		else
		{
			rockCenter ();
		}

		//spawn invisible shield if unlocked and after cooldown
		if (allowShield && (Time.time - lastShield) > shieldCooldown) 
		{
			shieldCooldown = 8.0f;
			GameObject invsShield = Instantiate(shield, transform.position, transform.rotation);
			invsShield.GetComponent<Renderer> ().enabled = false;
			allowShield = false;
		}

		//if the shield is found, match the player objects position
		GameObject tShield = GameObject.FindGameObjectWithTag("Shield");
		if(tShield)
			tShield.transform.position = transform.position;

	}

	//show the shield for shieldLife seconds after it's been hit
	public void visualizeShield()
	{
		GameObject tShield = GameObject.FindGameObjectWithTag("Shield");
		if (tShield) 
		{
			tShield.GetComponent<Renderer> ().enabled = true;
			sourceShield.PlayOneShot (shieldSound, 1f);
		}
		lastShield = Time.time + shieldLife;
		allowShield = true;
		Destroy (tShield, shieldLife);
	}

	void OnDestroy()
	{
		foreach(Transform child in transform.GetChild(0))
		{
			//Debug.Log ("checking childs");
			if (child.CompareTag("treadMark")) 
			{
				Debug.Log ("disconnecting treadmark");
				child.parent = null;
			}
		}
	}

    void OnTriggerEnter (Collider other )
	{
		/*
		if (canCollide)
		{
			if (other.gameObject.GetComponent<MoveDrone> () != null) 
			{
				Debug.Log ("Player rams enemy. 2 and 0.5 dmg");
				collideTimer = 0;
				other.gameObject.GetComponent<MoveDrone> ().takeDamage (2);
				takeDamage (0.5f);
			}
			else if (other.gameObject.GetComponent<MoveScout> () != null) 
			{
				Debug.Log ("Player rams enemy. 2 and 0.5 dmg");
				collideTimer = 0;
				other.gameObject.GetComponent<MoveScout> ().takeDamage (2);
				takeDamage (0.5f);
			}
		}
		*/

	}



	public void rockLeft()
	{
//		Vector3 target = new Vector3(transform.rotation.x,transform.rotation.y,2);
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,2.5f*Vector3.forward,1f);
//		transform.RotateAround(transform.position,Vector3.forward,2);
	}

	public void rockRight()
	{
//		Vector3 target = new Vector3(transform.rotation.x,transform.rotation.y,-2);
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,-2.5f*Vector3.forward,1f);
//		transform.RotateAround(transform.position,Vector3.forward,-2);
	}

	public void rockCenter()
	{
		Vector3 target = new Vector3(transform.rotation.x,transform.rotation.y,0);
		transform.eulerAngles = AngleLerp(transform.eulerAngles,target,.5f);
	}
		
	private Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
	{        
		float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
		float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
		float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
		Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
		return Lerped;
	}
}
