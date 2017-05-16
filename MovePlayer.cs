using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour {
	public float speed;
	public float rotateSpeed;
	public float playerHealth;
	public GameObject deathAnime;
	public GameObject deadPlayer;
	public GameObject shield;
	public bool allowShield;
	public float shieldLife = 3.0f;
	public float shieldCooldown = 7.5f;
	public float lastShield;
	public Transform turret;
	public float turretRotateSpeed;

	public void Start ()
	{
		if (PlayerPrefs.GetInt ("ArmorUpgrade") == 1) 
		{
			playerHealth = 5.0f;
		} 
		else if (PlayerPrefs.GetInt ("ArmorUpgrade") == 2) 
		{
			playerHealth = 7.0f;
		} 
		else if (PlayerPrefs.GetInt ("ArmorUpgrade") == 3) 
		{
			playerHealth = 7.0f;
			allowShield = true;	
		} 
		else 
		{
			playerHealth = 4.0f;
			allowShield = false;
		}

		if (PlayerPrefs.GetInt ("MoveUpgrade") == 1) 
		{
			speed = 4.0f;
			rotateSpeed = 1.0f;
			turretRotateSpeed = 75f;
		} 
		else 
		{
			speed = 3.0f;
			rotateSpeed = 0.85f;
			turretRotateSpeed = 65f;
		}

		GameObject HUD = GameObject.FindGameObjectWithTag("HUD");
		HUD.gameObject.GetComponent<HUD> ().healthSlider.maxValue = playerHealth;
		
	}

	public void takeDamage (float multiplier)
	{
		GameObject HUD = GameObject.FindGameObjectWithTag ("HUD");
		playerHealth = playerHealth - 1 * multiplier;
		if (multiplier >= 1) 
		{
			HUD.GetComponent<HUD> ().VisualizeRedDmg ();
		}
	}


	void Update ()
	{	
		if (playerHealth <= 0) {
			GameObject explosion = Instantiate (deathAnime, gameObject.transform.position + Vector3.up, transform.rotation);
			Destroy (explosion, 3.0f);
			GameObject dead = Instantiate (deadPlayer, transform.position, transform.rotation * Quaternion.Euler (0, 90, 0));
			Destroy (dead, 5.0f);
			Destroy (gameObject);
		}

		// Rotate around y - axis
		CharacterController controller = GetComponent<CharacterController> ();		
		transform.Rotate (0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);

		// Move forward / backward
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		float curSpeed = speed * Input.GetAxis ("Vertical");
		controller.SimpleMove (forward * curSpeed);

		if (Input.GetKey ("e")) 
		{
			turret.Rotate(0,0,turretRotateSpeed*Time.deltaTime);
		}
		else if(Input.GetKey ("q")) 
		{
			turret.Rotate(0,0,-turretRotateSpeed*Time.deltaTime);
		}

		//spawn shield if unlocked and after cooldown
		if (Input.GetKeyUp("left shift") && allowShield && (Time.time - lastShield) > shieldCooldown) 
		{
			lastShield = Time.time;
			GameObject tempShield = Instantiate(shield, transform.position, transform.rotation);
			Destroy(tempShield,shieldLife);
		}

		//if the shield is found, match the player objects position
		GameObject tShield = GameObject.FindGameObjectWithTag("Shield");
		if(tShield)
			tShield.transform.position = transform.position;
	}

    void OnTriggerEnter (Collider other )
	{
		GameObject GM = GameObject.FindGameObjectWithTag("GameController");
		
		if (other.gameObject.layer != 8 && other.gameObject.GetComponent<MoveDrone> () != null) 
		{
			Debug.Log ("Player rams enemy. 2 and 0.5 dmg");
			other.gameObject.GetComponent<MoveDrone> ().takeDamage (1);
			takeDamage (0.5f);
		}
		
		else if (other.gameObject.layer != 8 && other.gameObject.GetComponent<MoveScout> () != null) 
		{
			Debug.Log ("Player rams scout. 2 and 0.5 dmg");
			other.gameObject.GetComponent<MoveScout> ().takeDamage (1);
			takeDamage (0.5f);
		}

		else if (other.gameObject.layer != 8 && other.gameObject.GetComponent<MoveAce> () != null) 
		{
			Debug.Log ("Player rams ace. 1 and 0.5 dmg");
			other.gameObject.GetComponent<MoveAce> ().takeDamage (1);
			takeDamage (0.5f);
		}

		else if(other.gameObject.tag == "PlayerTunnel" && GM.GetComponent<SpawnDrone>().allowLevelExit)
		{
			Application.LoadLevel ("UpgradeMenu");	
		}


	}
}
