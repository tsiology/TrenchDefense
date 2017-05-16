using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDmg : MonoBehaviour {

	public float bulletLife = 1.75f; //how many seconds the bullet remains in-game
	public float bulletStart;
	public GameObject hitAnime;

	// Use this for initialization
	void Start () 
	{
		bulletStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	GameObject HUD = GameObject.FindWithTag ("HUD");


		if (Time.time - bulletStart > bulletLife) 
		{
			Destroy(gameObject);
		}


	}

	void OnTriggerEnter (Collider other )
	{
		//if the bullet hits the enemy base
		if (tag != "Enemy" && other.gameObject.tag == "EnemyBase") 
		{
			HUD.shotsHit++;
			other.gameObject.GetComponent<EnemyBase> ().takeDamage (1);   //damage the base
		}

		//if the bullet is not an enemy bullet and hits an enemy 
		else if (tag != "Enemy" && other.gameObject.tag == "Enemy") {
			GameObject HUDobj = GameObject.FindWithTag ("HUD");
			HUDobj.GetComponent<HUD> ().ScoreUpdate (10);
			HUD.shotsHit++;
			if (tag == "Charged") 
			{
				Debug.Log ("Player charged bullet dealing 2 dmg to enemy");
				other.gameObject.GetComponent<MoveDrone> ().takeDamage (2);   //damage the drone
				GameObject tmp = Instantiate (hitAnime, gameObject.transform.position, Quaternion.identity);
				Destroy (tmp, 0.65f);
			} 
			else if(other.gameObject.GetComponent<MoveDrone> ()) 
			{
				Debug.Log ("Player bullet dealing 1 dmg to enemy");
				other.gameObject.GetComponent<MoveDrone> ().takeDamage (1);   //damage the drone
				
				bulletHit ();
			}
			else if(other.gameObject.GetComponent<MoveScout> ()) 
			{
				Debug.Log ("Player bullet dealing 1 dmg to enemy");
				other.gameObject.GetComponent<MoveScout> ().takeDamage (1);   //damage the drone
				
				bulletHit ();
			}
			else if(other.gameObject.GetComponent<MoveAce> ()) 
			{
				Debug.Log ("Player bullet dealing 1 dmg to enemy Ace");
				other.gameObject.GetComponent<MoveAce> ().takeDamage (1);   //damage the drone
				
				bulletHit ();
			}
		}

		//if the bullet is not an enemy bullet and hits a sandbag
		else if (tag != "Enemy" && other.gameObject.tag == "Sandbag") {
			Debug.Log ("Player bullet hit Sandbag");
			Destroy (other.gameObject);
			bulletHit ();
		}

		//if the enemy hits the player base
		else if (tag == "Enemy" && other.gameObject.tag == "PlayerBase") 
		{
			other.gameObject.GetComponent<PlayerBase> ().takeDamage (1);   //damage the base
		}

		//if the bullet is an enemy bullet and hits the player
		else if (tag == "Enemy" && other.gameObject.tag == "Player") {
			Debug.Log ("Enemy bullet dealing 1 dmg to player");
			other.gameObject.GetComponent<MovePlayer> ().takeDamage (1);
			bulletHit ();
		}

		//if the bullet is an enemy bullet and hits a sandbag
		else if (tag == "Enemy" && other.gameObject.tag == "Sandbag") {
			Debug.Log ("Enemy bullet hit Sandbag");
			Destroy (other.gameObject);
			bulletHit ();
		}

		else if (tag == "Enemy" && other.gameObject.tag == "Shield") {
			Debug.Log ("Enemy bullet hit energy shield");
			bulletHit ();
		}

		//if the bullet hits terrain
		else if (other.gameObject.tag == "Terrain")
		{
			bulletHit ();
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy" && tag == "Enemy")
		{
			Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}

	//destroys the bullet and creats a hit animation
	void bulletHit()
	{
		GameObject tmp = Instantiate (hitAnime, transform.position + Vector3.back, Quaternion.identity);
		Destroy (tmp, 0.65f);
		Destroy (gameObject);
	}

}
