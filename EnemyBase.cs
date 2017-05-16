using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {
	public float enemyBaseHealth = 5;
	public GameObject baseExplosion;
	public GameObject baseFire;
	public GameObject destroyedBase;
	public float maxHealth;
	public bool isDestroyed;

	public void Start()
	{
		enemyBaseHealth += (HUD.waves+1) * 1;
		maxHealth = enemyBaseHealth;
		isDestroyed = false;
	}

	public void takeDamage(float multiplier)
	{
		enemyBaseHealth = enemyBaseHealth - 1* multiplier;
		Debug.Log ("enemyBaseHealth is:" + enemyBaseHealth);
	}


	void Update () 
	{
		if (enemyBaseHealth <= 0 && !isDestroyed) 
		{
			Invoke ("BaseDestruction", 0);
			Invoke ("BaseDestruction", 1);
			Invoke ("BaseDestruction", 2);
			Invoke ("BaseDestruction", 2.5f);
			Invoke ("BaseDestruction", 3);

			isDestroyed = true;

			Invoke ("BaseSwap",3);
			Destroy(gameObject,4);
		}
	}

	void BaseDestruction()
	{
		float xPos;
		float yPos;

		//spawn explosion gifs 3 random positions
		for(int x=0; x<3; x++)
		{
			xPos = 6.5f*Random.value;
			yPos = 5.0f*Random.value;
			//all the additional vector3 is to move random xy positions for explosions over the entire object position
			GameObject explosion = Instantiate(baseExplosion,transform.position - 2*Vector3.forward - 4*Vector3.right + (xPos*Vector3.right) + (yPos*Vector3.up), transform.rotation);
			Destroy (explosion, 5f);
			GameObject fire = Instantiate (baseFire, transform.position - 2*Vector3.forward - 4*Vector3.right + (xPos*Vector3.right) + (yPos*Vector3.up),  transform.rotation);
			Destroy (fire, 5f);
		}
	}

	void BaseSwap()
	{
		GameObject destroyed = Instantiate(destroyedBase,gameObject.transform.position, transform.rotation);
	}

}
