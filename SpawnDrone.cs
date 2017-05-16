using UnityEngine;
using System.Collections;

public class SpawnDrone : MonoBehaviour {

	private float spawnClock;
	private float nextTime = 2.0f;
	public float period = 5.0f;
	public GameObject tankDrone;
	public GameObject tankDroneCpt;
	public GameObject enemyScout;
	public GameObject enemyAce;
	public int enemyTotalSpawn = 3;
	public bool enemyBaseAlive;
	public bool enemiesAlive;
	public static bool firstLevel=true;
	public bool allowLevelExit;
	public int spawned;

	// Use this for initialization
	void Start ()
	{
		spawnClock = 0.0f;
		period = 5.0f;



		if (HUD.waves >= 7) 
		{
			enemyTotalSpawn = HUD.waves - 4;
		}
		else 
		{
			//spawn 3 additional enemies per wave
			enemyTotalSpawn += 3 * HUD.waves;
		}

		allowLevelExit = false;
		spawned = 0;
	}


	void Update ()
	{
		enemyBaseAlive = GameObject.FindWithTag ("EnemyBase");
		enemiesAlive = GameObject.FindWithTag ("Enemy");
		spawnClock += Time.deltaTime;

		//time to spawn
		if (spawnClock > nextTime && enemyTotalSpawn != 0 && enemyBaseAlive) 
		{
			nextTime += period;
			spawned++;
			enemyTotalSpawn--;

			//spawn scouts on the first spawn if 3 waves are completed
			if (HUD.waves >= 3 && spawned == 1) 
			{
				Instantiate (enemyScout, new Vector3 (-2.5f, 0.1f, 41.5f), Quaternion.Euler (0, 0, 0));
				Instantiate (enemyScout, new Vector3 (0, 0.1f, 41.5f), Quaternion.Euler (0, 0, 0));
				Instantiate (enemyScout, new Vector3 (2.5f, 0.1f, 41.5f), Quaternion.Euler (0, 0, 0));

				nextTime -= 3.5f;
			}

			else if(HUD.waves>=7)
			{
				Instantiate (enemyAce, new Vector3 (0, 0.1f, 41.5f), Quaternion.Euler (0, 180, 0));
				period = 12.0f; 
			}

			else if (enemyTotalSpawn == 0 && HUD.waves>=5) 
			{
				Instantiate (enemyAce, new Vector3 (0, 0.1f, 41.5f), Quaternion.Euler (0, 180, 0));
			} 

			//make a Captain on every 3rd spawn. also every one if waves>5
			else if (enemyTotalSpawn%3==0 || (HUD.waves>=5)) 
			{
				Instantiate (tankDroneCpt, new Vector3 (0, 0.1f, 41.5f), Quaternion.Euler (0, 180, 0));
				//captainSpawned = true;
			} 
			
			//otherwise spawn a drone
			else 
			{
				Instantiate (tankDrone, new Vector3 (0, 0.1f, 41.5f), Quaternion.Euler (0, 180, 0));
			}
		}//end of time to spawn
			

		//if enemy base no longer exists
		if (!enemyBaseAlive) 
		{
			firstLevel = false;
			allowLevelExit = true;
		}

		//if the player is destroyed
		if(GameObject.FindWithTag ("Player") == null &&  GameObject.FindWithTag ("Dead") == null)
		{
			Application.LoadLevel ("GameOverMenu");
		}

		//if the player base is destroyed
		if(GameObject.FindWithTag ("PlayerBase") == null)
		{
			Application.LoadLevel ("GameOverMenu");
		}

	}//end update




	void OnTriggerEnter (Collider other )
	{

		//if(captainSpawned == true )
		//{
			if (other.gameObject.layer == 9) 
			{
			Application.LoadLevel ("UpgradeMenu");
			//Instantiate (tankDroneCpt, new Vector3 (0, 0.5f, 38), Quaternion.Euler (0, 180, 0));
			}
		//}
	}



}
