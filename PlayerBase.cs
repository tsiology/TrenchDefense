using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {
	public float playerBaseHealth = 5;
	public float maxHealth;
	public GameObject baseExplosion;
	public GameObject baseFire;
	public GameObject destroyedBase;
	public bool isDestroyed;

	public void Start()
	{
		playerBaseHealth += (HUD.waves+1) * 1;
		maxHealth = playerBaseHealth;
		isDestroyed = false;
	}

	public void takeDamage(float multiplier)
	{
		playerBaseHealth = playerBaseHealth - 1* multiplier;
		Debug.Log ("playerBaseHealth is:" + playerBaseHealth);
	}


	void Update () 
	{
		if (playerBaseHealth <= 0 && !isDestroyed) 
		{
			Invoke ("BaseDestruction", 0);
			Invoke ("BaseDestruction", 1);
			Invoke ("BaseDestruction", 2);
			Invoke ("BaseDestruction", 2.5f);
			Invoke ("BaseDestruction", 3);

			isDestroyed = true;

			Invoke ("BaseSwap",5);
			Destroy(gameObject,6);
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
			GameObject explosion = Instantiate(baseExplosion,transform.position + 2*Vector3.forward - 4*Vector3.right + (xPos*Vector3.right) + (yPos*Vector3.up), transform.rotation);
			Destroy (explosion, 5f);
			GameObject fire = Instantiate (baseFire, transform.position + 2*Vector3.forward - 4*Vector3.right + (xPos*Vector3.right) + (yPos*Vector3.up),  transform.rotation);
			Destroy (fire, 5f);
		}
	}

	void BaseSwap()
	{
		GameObject destroyed = Instantiate(destroyedBase,gameObject.transform.position, transform.rotation);
	}

}
