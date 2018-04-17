/*

Attatch to the player object to allow them to fire with characteristics
upgraded by how many waves have been completed. 

tgodwin

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


public class Shooter : MonoBehaviour {

	public Transform bullet;
	public Transform bullet2;
	public Transform shell;
	public float power;
	public float bulletDestroyDelay = 2.0f;
	public int fireMode=0;
	public float fireRate;
	public float fireRate2;
	public float lastFire=0;
	public bool burstFire;
	public int burstRoundCnt=0;
	//public Button btn;
	public int specAmmo=0;

	public GameObject chargedFlare;
	public GameObject muzzleFlare;
	public GameObject smoke;

	public AudioClip shootSound;
	private AudioSource source;

	void Awake()
	{
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () 
	{
		//max weapon upgrades for waves 7+
		if(HUD.waves >= 7)
		{
			fireRate = 1.00f;
			fireRate2 = 1.33f;
			power = 4000f;
			burstFire=true;
		}	

		//waves 4+
		else if(HUD.waves >= 4)
		{
			fireRate = 1.00f;
			fireRate2 = 1.33f;
			power = 4000f;
			burstFire=false;	
		}

		//1+
		else if(HUD.waves >= 1)
		{
			fireRate = 1.33f;
			fireRate2 = 1.33f;
			power = 3000f;
			burstFire=false;
		}

		else
		{
			fireRate = 2.00f;
			fireRate2 = 1.33f;
			power = 3000f;
			burstFire=false;	
		}

		HUD.control.FireButton.onClick.AddListener (() => {callFire();});
		HUD.control.SpecFireButton.onClick.AddListener (() => {callSpecFire();});


	}//end of Start()
		
	void fire()
	{
		source.PlayOneShot (shootSound, 1f);
		HUD.shotsFired++;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet, transform.position + (transform.forward*2) + (transform.up*.65f) + (transform.right*.2f), transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power);

		GameObject flash = Instantiate (muzzleFlare, transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f), transform.rotation);
		Destroy (flash, 0.65f);



		if (!burstFire) 
		{
			Invoke ("EjectShell",fireRate/2.0f);
			GameObject barrelSmoke = Instantiate (smoke, transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f), transform.rotation); 
			Destroy (barrelSmoke, 1.5f);
		} 
		else 
		{
			Invoke ("EjectShell",.25f);
			burstRoundCnt++;

			//create smoke only after last shot for burst fires
			if (burstRoundCnt == 3) 
			{
				burstRoundCnt = 0;
				GameObject barrelSmoke = Instantiate (smoke, transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f), transform.rotation); 
				Destroy (barrelSmoke, 1.5f);
			}
		}

		lastFire = Time.time;
		GameObject bar = GameObject.FindGameObjectWithTag("HUD");
		bar.gameObject.GetComponent<HUD> ().cannonSlider.value = 0;
	}

	void fire2()
	{
		//HUD.shotsFired++;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet2, transform.position + (transform.forward*2) + (transform.up*.65f) + (transform.right*.2f), Quaternion.identity) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power * 300f);
		lastFire = Time.time;
		GameObject bar = GameObject.FindGameObjectWithTag("HUD");
		bar.gameObject.GetComponent<HUD> ().cannonSlider.value = 0;
	}

	void fire2Flare()
	{
		GameObject flash = Instantiate (chargedFlare, transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f), transform.rotation);
		Destroy (flash, .65f);
	}
		

	// Update is called once per frame
	void Update () {

		if ( (Input.GetButtonUp ("Jump")) && (Time.time - lastFire) >= fireRate) 
		{
			if (!burstFire) 
				{
					Invoke ("fire", 0);
					Invoke("rockBack",0.1f);
				} 
				else 
				{
					Invoke ("fire", 0);
					Invoke("rockBack",0.1f);
					Invoke ("fire", .25f);	
					Invoke("rockBack",0.35f);
					Invoke ("fire", .50f);
					Invoke("rockBack",0.6f);
				}
		}
			
		if ( (Input.GetKeyUp ("z") ||  Input.GetKeyUp (KeyCode.LeftShift)) && (specAmmo >= 1) && (Time.time - lastFire) >= fireRate2) 
		{
			lastFire = Time.time;
			fire2Flare ();
			Invoke ("fire2", 0.65f);
			Invoke("rockBack",0.75f);
			specAmmoUp (-1);
		}


		//find any spec fire flares that exist, grow their scale and keep their position with tank
		GameObject cFlare = GameObject.FindGameObjectWithTag("Flare");
		if (cFlare) {
			Vector3 scale = cFlare.transform.localScale;
			cFlare.transform.position = transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f);

			if(scale.x <= 0.5f)
			{
				scale.x += .009f;
				scale.y += .009f;
				scale.z += .009f;
				cFlare.transform.localScale = scale;
			}
		}

		//find any smoke, keep position with tank
		GameObject cSmoke = GameObject.FindGameObjectWithTag("smoke");
		if (cSmoke) {
			cSmoke.transform.position = transform.position + (transform.forward * 2) + (transform.up * .65f) + (transform.right * .2f);
		}

	}

	//CallFire button controlled by touch 
	public void callFire ()
	{
		if(Time.time - lastFire >= fireRate)
		{
			if (!burstFire) 
			{
				Invoke ("fire", 0);
				Invoke("rockBack",0.1f);
			} 
			else 
			{
				Invoke ("fire", 0);
				Invoke("rockBack",0.1f);
				Invoke ("fire", .25f);	
				Invoke("rockBack",0.35f);
				Invoke ("fire", .50f);
				Invoke("rockBack",0.6f);
			}
		}
		//Debug.Log ("Player calling Fire");

	}

	private void EjectShell()
	{
		Transform newShell = Instantiate (shell, transform.position + (transform.forward * .8f) + (transform.up * .65f) + (transform.right * .8f), transform.rotation) as Transform;
		newShell.GetComponent<Rigidbody>().AddForce(30 * (Vector3.right+Vector3.up));
		Destroy (newShell.gameObject, 9.0f);	
	}

	public void callSpecFire()
	{
		if(Time.time - lastFire >= fireRate2 && specAmmo >= 1)
		{
			fire2Flare ();
			Invoke ("fire2", 0.65f);
			Invoke("rockBack",0.75f);
			specAmmoUp (-1);
		}
		//Debug.Log ("Player calling specFire");
	}


	public void rockBack()
	{
		Vector3 target = new Vector3(-10,transform.rotation.y,transform.rotation.z);
		transform.eulerAngles = AngleLerp(transform.eulerAngles,target,.5f);
		//Invoke ("rockDown",0.25f);
	}

	public void rockDown()
	{
		Vector3 target = new Vector3(0,transform.rotation.y,transform.rotation.z);
		//transform.Rotate (target);
		transform.eulerAngles = AngleLerp(transform.eulerAngles,target,1f);
	}
		
	void OnTriggerEnter (Collider other )
	{
		/*
		if (other.gameObject.tag == "Loot") 
		{
			Destroy(other.gameObject);
			specAmmoUp (3);
		}
		*/

	}
	public void specAmmoUp(int amount)
	{
		specAmmo += amount;
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
