/*

HUD tracks player score and waves completed as static variables.
Only one instance of the HUD can be created per game.

tgodwin

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

	public Slider healthSlider;
	public Slider cannonSlider;
	public Text scoreText;
	public Text reloadText;
	public Text shieldText;
	public static int score;
	public static int highScore;
	public static int shotsFired;
	public static float shotsHit;
	public static float accuracy;
	public static bool turbo;
	public static int waves;
	public Image bloodFlash;
	public Button FireButton;
	public Button SpecFireButton;
	private bool firstFrame;
	public static HUD control;
	private bool allowShield;
	private float lastShield;
	private float shieldCooldown;
	public GameObject SpecAmmoUI;
	public Canvas HUDCanvas;

	//ensure only one instance of object with HUD exists at a time, and that values are saved through
	//multiple level loads
	void Awake()
	{
		if(control == null)
		{
			DontDestroyOnLoad (gameObject);
			control = this;
		}
		else if(control != this)
		{
			Destroy (gameObject);
		}
	}

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
//		Debug.Log("Level Loaded");
//		Debug.Log(scene.name);
//		Debug.Log(mode);
		scoreText.text = "Score: " + score.ToString () + "\nWave: " + (waves+1).ToString();
		shotsHit = 0;
		shotsFired = 0;
		firstFrame = true;
	}


	// Use this for initialization
	//these are set only the first time the level loads since control is not destroyed
	void Start ()
	{
		NewGameStats ();
		
		scoreText.text = "Score: " + score.ToString () + "\nWave: " + (waves + 1).ToString ();
		reloadText.text = "[ Main Gun Loading... ]";
		highScore = PlayerPrefs.GetInt ("highScore");

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		healthSlider.maxValue = player.GetComponent<MovePlayer> ().playerHealth;
		healthSlider.value = player.GetComponent<MovePlayer> ().playerHealth;
		cannonSlider.value = 0.0f;

		ScoreUpdate(0);
	}

	//these are set every time level reloads
	void OnLevelWasLoaded()
	{
		if (waves >= 8) 
		{
			shieldText.text = "[ Auto-Shield engaged! ]";
		} 
		else 
		{
			shieldText.text = "";
		}
	}

	public static void NewGameStats()
	{
		shotsHit = 0;
		shotsFired = 0;
		accuracy = 0;

		if(!SpawnDrone.turboLevel)
			waves = 0;

		score = 0;
		PlayerPrefs.SetInt ("currentScore", 0);
		control.ScoreUpdate (0);
	}
		
	void Update ()
	{
		GameObject Player = GameObject.FindGameObjectWithTag("Player");

		if (firstFrame && Player!=null) 
		{
			healthSlider.maxValue = Player.GetComponent<MovePlayer> ().playerHealth;
			firstFrame = false;
		}

		if (Player != null) 
		{
			HUDCanvas.enabled = true;
			healthSlider.value = Player.gameObject.GetComponent<MovePlayer> ().playerHealth;
			allowShield = Player.gameObject.GetComponent<MovePlayer>().allowShield;
			lastShield = Player.gameObject.GetComponent<MovePlayer>().lastShield;
			shieldCooldown = Player.gameObject.GetComponent<MovePlayer>().shieldCooldown;

			if(Player.gameObject.GetComponent<Shooter>().specAmmo <= 0)
			{
				SpecAmmoUI.SetActive (false);
			}
			else
				SpecAmmoUI.SetActive (true);
		}
		else
		{	
			HUDCanvas.enabled = false;
			PlayerPrefs.SetInt ("currentScore",0);
		}


		if( cannonSlider.value < 2.0f )
		{
			if (HUD.waves < 1) 
			{
				cannonSlider.value += Time.deltaTime;
			} 
			else if (HUD.waves < 3) 
			{
				cannonSlider.value += 1.66f * Time.deltaTime;
			} 
			else if (HUD.waves < 7) 
			{
				cannonSlider.value += 2.00f * Time.deltaTime;
			} 
			else 
			{
				cannonSlider.value += 2.00f * Time.deltaTime;
			}	

		}

		if (cannonSlider.value == cannonSlider.maxValue) 
		{
			reloadText.text = "[ Main Gun Ready! ]";
		} 
		else 
		{
			reloadText.text = "[ Main Gun Loading... ]";
		}

		if (allowShield && (Time.time - lastShield) > shieldCooldown) 
		{
			shieldText.text = "[ Auto-Shield engaged! ]";
		} 
		else if(allowShield && GameObject.FindGameObjectWithTag("Shield")==null)
		{
			shieldText.text = "[ Shield recharging... ]";
		} 

		accuracy = shotsHit / shotsFired;

	}//end update

	public void ScoreUpdate(int points)
	{
		score += points;

		if (score < 0)
			score = 0;

		scoreText.text = "Score: " + score.ToString () + "\nWave: " + (waves+1).ToString();
		PlayerPrefs.SetInt ("currentScore",score);

		if (score >= PlayerPrefs.GetInt("highScore") && !SpawnDrone.turboLevel)
		{
			PlayerPrefs.SetInt("highScore",score);
			highScore = PlayerPrefs.GetInt ("highScore");
		}

	}

	public void VisualizeRedDmg (float clearTime)
	{
		bloodFlash.color = new Color32 (229,37,37,64);
		Invoke("ClearRedDmg",clearTime);
	}

	public void ClearRedDmg ()
	{
		bloodFlash.color = new Color32 (229, 37, 37, 0);
	}

	public void ClickFireButton ()
	{
		GameObject Player = GameObject.FindGameObjectWithTag("Player");

		if (Player != null) 
		{
			Player.GetComponent<Shooter>().callFire();
		}
	}

	public void ClickSpecFireButton ()
	{
		GameObject Player = GameObject.FindGameObjectWithTag("Player");

		if (Player != null) 
		{
			Player.GetComponent<Shooter>().callSpecFire();
		}
	}

}
