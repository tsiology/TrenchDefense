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

	// Use this for initialization
	void Start () {

	}

	void fire()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Transform newBullet = Instantiate(bullet, transform.position + (transform.forward*2.2f) + (transform.up*1.10f), transform.rotation) as Transform;
		newBullet.GetComponent<Rigidbody>().AddForce(fwd * power);
	}




	// Update is called once per frame
	void Update () {


		//single shot
		if ( (Time.time - lastFire) >= fireRate) 
		{
			randomFire = fireRate * Random.value;     //add anywhere from 0.0 to 1.0 seconds until when to fire
			lastFire = Time.time + randomFire;
			Invoke ("fire", randomFire);
		}


	}
}
