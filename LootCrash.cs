/*
Simple script attached to falling loot crates.
Destroy the crate if it hits the ground, give the player ammo if it hits the player.

tgodwin
6/4/17

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrash : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter (Collider other )
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Shooter>().specAmmoUp (3);
			Destroy (gameObject);
		}
		else if (other.gameObject.tag == "Terrain") 
		{
			Destroy (gameObject);
		}
	}
}
