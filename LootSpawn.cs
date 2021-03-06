/*

Spawn a falling loot crate at random time and location

tgodwin

*/


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LootSpawn : MonoBehaviour {

	public Transform lootDrop;
	public int dropTimer;
	public float dropLocation;
	public int dropChance;
	public bool dropped;

	// Use this for initialization
	void Start () 
	{
		dropped = false;
		int max,min;
		max = 3 + HUD.waves / 2;
		min = Random.Range (1,3);

		if (max <= min)
			max = min + 1;

		dropTimer = Random.Range (min, max);
		dropLocation = Random.Range (.1f, .3f);
		dropChance = Random.Range (1, 1);
	}


	void Update ()
	{
		GameObject GM = GameObject.FindGameObjectWithTag ("GameController");

		if (GM.GetComponent<SpawnDrone> ().spawned == dropTimer && dropped == false && HUD.waves >= 1 && dropChance==1) 
		{
			dropped = true;
			Instantiate (lootDrop, transform.position + (transform.up * 10) + (transform.forward*13) + (transform.right * (1 + 30f*dropLocation)), transform.rotation);
		}
		

	}//end update


}
