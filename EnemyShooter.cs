/*

This script allows for single or burst firing enemies that instantiate bullet prefabs
and apply a force in the forward direction the enemy is facing. Fire rates recieve 
random increases in time to stagger shots.

tgodwin

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShooter : MonoBehaviour {

	public Transform bullet;
	public float power = 3000f;
	public float bulletDestroyDelay = 2.0f;
	public int fireMode=0;
	public float fireRate=1.0f;
	public float lastFire=0;
	public float randomFire;
	public bool burstFire;
	public GameObject hitAnime;

	// Use this for initialization
	void Start () 
	{
		lastFire = Time.time;
	}

	void fire()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet, transform.position + (transform.forward*2.2f) + (transform.up*.60f), transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power);
		GameObject tmp = Instantiate (hitAnime, transform.position + (transform.forward*2.2f) + (transform.up*.60f), transform.rotation);
		Destroy (tmp, 0.75f);
	}


	void Update () 
	{
		if ( (Time.time - lastFire) >= fireRate) 
		{
			randomFire = fireRate * Random.value;     //add anywhere from 0.0 to 1.0 seconds until when to fire
			lastFire = Time.time + randomFire;
			if (!burstFire)
				Invoke ("fire", randomFire);
			else 
			{
				Invoke ("fire", randomFire);	
				Invoke ("fire", .25f + randomFire);	
				Invoke ("fire", .50f + randomFire);	
			}
		}

	}
}
