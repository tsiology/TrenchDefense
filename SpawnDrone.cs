/*

Controls frequency, location, and type of enemy spawns based on waves completed.
tgodwin
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpawnDrone : MonoBehaviour {

	private float spawnClock;
	private float nextTime = 2.0f;
	public float period = 5.5f;
	public Transform tankDrone;
	public Transform tankDroneSgt;
	public Transform tankDroneLt;
	public Transform enemyScout;
	public Transform tankDroneSqd;
	public int enemyTotalSpawn = 3;
	public GameObject endOfEnemies;
	//public static bool firstLevel=true;
	public static bool turboLevel=false;
	public GameObject[] enemyArray;
	public int spawned;
	private int direction;

	// Use this for initialization
	void Start () 
	{
		spawnClock = 0.0f;
		enemyTotalSpawn += HUD.waves;
		spawned = 0;

		if (HUD.waves < 3) 
		{
			period = 7.5f;
		}
		else if (HUD.waves < 7) 
		{
			period = 5.5f;
		}
		//else if(HUD.waves < 7) 
		else
		{
			period = 3.5f;
		}

	}


	void Update ()
	{
		spawnClock += Time.deltaTime;

		if (enemyTotalSpawn % 2 == 0 && HUD.waves < 9) 
		{
			direction = 1;
		}
		else
		{
			direction = -1;
		}

		//send scouts if all enemies are cleared
		enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemyArray.Length==0 && HUD.waves >= 3 && spawned > 1 && enemyTotalSpawn != 0) 
		{
			nextTime+=0.5f;
			Instantiate (enemyScout, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
			Instantiate (enemyScout, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
			Instantiate (enemyScout, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
		}

		//time to spawn
		if (spawnClock > nextTime && enemyTotalSpawn != 0) 
		{	
				nextTime += period;
				enemyTotalSpawn--;
				spawned++;
				/*
				//randomize all spawns
				if (HUD.waves >= 13) 
				{	
					//speed up spawns
					nextTime-=1;

					int rand = Random.Range(1,6);
					
					if(rand==1)
					{
						Instantiate (tankDrone, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
					}
					else if(rand==2)
					{
						Instantiate (enemyScout, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
						Instantiate (enemyScout, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
						Instantiate (enemyScout, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					}
					else if(rand==3)
					{
						Instantiate (tankDroneSgt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
					}
					else if(rand==4)
					{
						Instantiate (tankDroneLt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
					}
					else if(rand==5)
					{
						Instantiate (tankDroneSqd, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
						Instantiate (tankDroneSqd, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
						Instantiate (tankDroneSqd, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					}

				}
				*/

				//if after 3 waves and on first spawn
				if (HUD.waves >= 3 && spawned==1)
				{
					nextTime -= period / 2;
					Instantiate (enemyScout, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (enemyScout, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (enemyScout, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
				}

				//if after 5 waves, every 3rd enemy spawn Sgt and not the last or second to last spawn
				else if (HUD.waves >= 5 && spawned%3 == 0 && enemyTotalSpawn != 0 && enemyTotalSpawn != 1) 
				{
					Instantiate (tankDroneSgt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				}

				//if after 6 waves, last enemy spawn, make a Lt
				else if (HUD.waves >= 6 && enemyTotalSpawn == 0) 
				{
					Instantiate (tankDroneLt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				} 

				//if after 3 waves and on 2nd last spawn
				else if (HUD.waves >= 4 && enemyTotalSpawn == 1)
				{
					Instantiate (tankDroneSqd, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (tankDroneSqd, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (tankDroneSqd, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
				}
				
				//if after 12 waves, only squads
				else if (HUD.waves >= 11)
				{
					Instantiate (tankDroneSqd, new Vector3 (-2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (tankDroneSqd, new Vector3 (0, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
					Instantiate (tankDroneSqd, new Vector3 (2.5f, 0.75f, 41f), Quaternion.Euler (0, 180, 0));
				}

				//if after 11 waves, only Lt's
				else if (HUD.waves >= 10)
				{
					Instantiate (tankDroneLt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				}

				//if after 10 waves, no more drones, only sgt's
				else if (HUD.waves >= 9)
				{
					Instantiate (tankDroneSgt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				}

				//last enemy spawn, make a Sgt
				else if (enemyTotalSpawn == 0 && HUD.waves > 0) 
				{
					Instantiate (tankDroneSgt, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				} 

				//otherwise spawn a drone
				else 
				{
					Instantiate (tankDrone, new Vector3 (0, 0.5f, 41), Quaternion.Euler (0, 180 + (direction * 30), 0));
				}



			//}
		}//end of time to spawn

		//level over after last enemy dies
		if(enemyTotalSpawn == 0)
		{
			endOfEnemies = GameObject.FindWithTag ("Enemy");
			if (endOfEnemies == null) 
			{
				//firstLevel = false;
				//Application.LoadLevel ("UpgradeMenu");
				SceneManager.LoadScene("UpgradeMenu");
			}
		}

		//game over after player dies
		if(GameObject.FindWithTag ("Player") == null &&  GameObject.FindWithTag ("Dead") == null)
		{
			SceneManager.LoadScene("GameOverMenu");
		}

	}//end update


}
