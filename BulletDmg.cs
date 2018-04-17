/*

This class can be attached to either player or enemy bullets and then uses collisions
to call approriate takeDamage scripts from the objects the bullet hits.

Also provides prefab for destroyed barricade.

tgodwin
6/4/17

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDmg : MonoBehaviour {

	public float bulletLife = 2.0f; //how many seconds the bullet remains in-game
	public float bulletStart;       //game time when bullet is created
	public GameObject hitAnime;     
	public GameObject deadBag;

	// Use this for initialization
	void Start () 
	{
		bulletStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time - bulletStart > bulletLife) 
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter (Collider other )
	{
		//Debug.Log ("hit something");

		//if the bullet is not an enemy bullet and hits an enemy 
		if (tag != "Enemy" && other.gameObject.tag == "Enemy") 
		{
			GameObject HUDobj = GameObject.FindWithTag ("HUD");
			HUDobj.GetComponent<HUD> ().ScoreUpdate (10);
	
			if (tag == "Charged")
			{
				if (other.gameObject.GetComponent<MoveDrone>())
				{
					other.gameObject.GetComponent<MoveDrone> ().takeDamage (5);   //damage the drone	
				}
				else if(other.gameObject.GetComponent<MoveScout>())
				{
					other.gameObject.GetComponent<MoveScout> ().takeDamage (5);   //damage the scout
				}
			} 		
			else
			{
				if (other.gameObject.GetComponent<MoveDrone>())
				{				
					other.gameObject.GetComponent<MoveDrone> ().takeDamage (1);   //damage the drone
					bulletHit ();
					HUD.shotsHit++;
				}
				else if(other.gameObject.GetComponent<MoveScout>())
				{
					other.gameObject.GetComponent<MoveScout> ().takeDamage (1);   //damage the scout
					bulletHit ();
					HUD.shotsHit++;
				}
			}
		}

		//if the bullet is an enemy bullet and hits the player
		else if (tag == "Enemy" && other.gameObject.tag == "Player") 
		{
			//Debug.Log ("Enemy bullet dealing dmg to player");
			other.gameObject.GetComponent<MovePlayer> ().takeDamage (1);
			bulletHit ();
		}

		else if (tag == "Enemy" && other.gameObject.tag == "Shield") 
		{
			GameObject player = GameObject.FindWithTag ("Player");
			if (player)
				player.GetComponent<MovePlayer> ().visualizeShield ();

			bulletHit ();
		}

		//if the bullet hits a sandbag
		else if (other.gameObject.tag == "Sandbag") 
		{
			Destroy (other.gameObject);
			GameObject bag1 = Instantiate (deadBag, gameObject.transform.position - (transform.up), Quaternion.Euler (0, 0, 90));
			GameObject bag2 = Instantiate (deadBag, gameObject.transform.position - (transform.up*.75f), Quaternion.Euler (0, 0, 90));
			GameObject bag3 = Instantiate (deadBag, gameObject.transform.position - (transform.up*.50f), Quaternion.Euler (0, 0, 90));
			GameObject bag4 = Instantiate (deadBag, gameObject.transform.position - (transform.up*.25f), Quaternion.Euler (0, 0, 90));
			Destroy (bag1, 3.5f);
			Destroy (bag2, 3.5f);
			Destroy (bag3, 3.5f);
			Destroy (bag4, 3.5f);

			if (tag != "Charged") 
			{
				bulletHit ();
			}
		}

		//if the bullet hits terrain
		else if (other.gameObject.tag == "Terrain" && tag != "Charged")
		{
			bulletHit ();
		}

	}

	//create a temporary explosion animation and destroy the bullet
	void bulletHit()
	{
		//GameObject tmp = Instantiate (hitAnime, gameObject.transform.position, Quaternion.identity);
		//Destroy (tmp, 0.75f);
		Destroy (gameObject);
	}


}
