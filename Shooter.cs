using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooter : MonoBehaviour {

	public Transform bullet;
	public Transform bullet2;
	public Transform gunEnd;
	public float power;
	public float bulletDestroyDelay = 2.0f;
	public int fireMode=0;
	public float fireRate;
	public float fireRate2;
	public float lastFire=0;
	public bool burstFire;


	// Use this for initialization
	void Start () 
	{
		if (PlayerPrefs.GetInt ("WeaponUpgrade") == 1) 
		{
			fireRate = 1.33f;
			fireRate2 = 1.33f;
			power = 3000f;
			burstFire=false;
		}

		else if (PlayerPrefs.GetInt ("WeaponUpgrade") == 2) 
		{
			fireRate = 1.00f;
			fireRate2 = 1.00f;
			power = 4000f;
			burstFire=false;
		}

		else if (PlayerPrefs.GetInt ("WeaponUpgrade") == 3) 
		{
			fireRate = 1.00f;
			fireRate2 = 1.00f;
			power = 4000f;
			burstFire=true;
		}


		else
		{
			fireRate = 2.00f;
			fireRate2 = 2.00f;
			power = 3000f;
			burstFire=false;
		}


		
	}
		
	void fire()
	{
		/*
		HUD.shotsFired++;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet, transform.position + (transform.forward*2.5f) + (transform.up*1.1f) , transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power);
*/
		HUD.shotsFired++;
		Vector3 direction = transform.TransformDirection(Vector3.down);
		//Transform newBullet = Instantiate(bullet, transform.position, transform.rotation* Quaternion.Euler (-25, 0, 0)) as Transform;
		Transform newBullet = Instantiate(bullet, gunEnd.transform.position, gunEnd.transform.rotation * Quaternion.Euler (90, 0, 0)) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(direction * power);

	}

	void fire2()
	{
		HUD.shotsFired++;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet2, transform.position + (transform.forward*2.5f) + (transform.up*1.1f), transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power * 20.0f);

	}
		
	
	// Update is called once per frame
	void Update () {
		/*
		//fire mode options if first wave is complete
		if (fireMode==0 && Input.GetKeyUp("z") && HUD.waves == 1) 
		{
			fireMode++;
		}
		else if (fireMode==1 && Input.GetKeyUp("z") && HUD.waves == 1) 
		{
			fireMode--;
		}

		//fire mode options after 2nd wave is complete
		if (Input.GetKeyUp("z") && HUD.waves >= 2) 
		{
			fireMode++;
		}
		*/
			
		//3 round burst
		//if (Input.GetButtonUp ("Jump") && fireMode % 3 == 1 && (Time.time - lastFire) >= fireRate) 
		if (Input.GetButtonUp ("Jump") && burstFire && (Time.time - lastFire) >= fireRate) 
		{
			lastFire = Time.time;
			Invoke ("fire", 0);

			GameObject HUD = GameObject.FindGameObjectWithTag("HUD");
			HUD.gameObject.GetComponent<HUD> ().cannonSlider.value = 0;

			Invoke ("fire", .25f);
			Invoke ("fire", .50f);
		}
		//charged shot
/*
		else if (Input.GetButtonUp ("Jump") && fireMode % 3 == 2 && (Time.time - lastFire) >= fireRate2) 
		{
			lastFire = Time.time + 1.0f;
			Invoke ("fire2", 1.0f);

			GameObject HUD = GameObject.FindGameObjectWithTag("HUD");
			HUD.gameObject.GetComponent<HUD> ().cannonSlider.value = 0;
		}
*/
		//otherwise default to single shot
		//else if (Input.GetButtonUp ("Jump") && fireMode % 3 == 0 && (Time.time - lastFire) >= fireRate) 
		else if (Input.GetButtonUp ("Jump")  && (Time.time - lastFire) >= fireRate) 
		{
			lastFire = Time.time;
			Invoke ("fire", 0);

			GameObject HUD = GameObject.FindGameObjectWithTag("HUD");
			HUD.gameObject.GetComponent<HUD> ().cannonSlider.value = 0;
		}

	}
}
