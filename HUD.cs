using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Slider healthSlider;
	public Slider cannonSlider;
	public Slider playerBaseSlider;
	public Slider enemyBaseSlider;
	public Text scoreText;
	public Text reloadText;
	public Text shieldText;
	public static int score;
	public static int highScore;
	public static int shotsFired;
	public static float shotsHit;
	public static float accuracy;
	public static int waves;
	public static int RP;
	public Image bloodFlash;

	public static HUD control;

	//ensure only once instance of object with HUD exists at a time, and that values are saved through
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

		RP = 0;
	}

	// Use this for initialization
	// these are set only the first time the level loads
	void Start () 
	{
		NewGameStats();

		scoreText.text = "Score: " + score.ToString () + "\nWave: " + (waves+1).ToString();
		reloadText.text = "[ Main Gun Loading... ]";
		shieldText.text = "Energy Shield Unavailable";
		highScore = PlayerPrefs.GetInt ("highScore");

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		healthSlider.maxValue = player.GetComponent<MovePlayer> ().playerHealth;
		healthSlider.value = player.GetComponent<MovePlayer> ().playerHealth;
		cannonSlider.value = 0.0f;

		GameObject pBase = GameObject.FindGameObjectWithTag("PlayerBase");
		playerBaseSlider.maxValue = pBase.gameObject.GetComponent<PlayerBase> ().playerBaseHealth;
		playerBaseSlider.value = pBase.gameObject.GetComponent<PlayerBase> ().playerBaseHealth;

		GameObject eBase = GameObject.FindGameObjectWithTag("EnemyBase");
		enemyBaseSlider.maxValue = eBase.gameObject.GetComponent<EnemyBase> ().enemyBaseHealth;
		enemyBaseSlider.value = eBase.gameObject.GetComponent<EnemyBase> ().enemyBaseHealth;

		
		ScoreUpdate (0);
		/*
		shotsHit = 0;
		shotsFired = 0;
		accuracy = 0;
		*/
	}

	void OnLevelWasLoaded()
	{
		shotsHit = 0;
		shotsFired = 0;
	}

	public static void NewGameStats()
	{
		shotsHit = 0;
		shotsFired = 0;
		accuracy = 0;
		waves = 0;
		score = 0;
		RP = 0;
		PlayerPrefs.SetInt ("currentScore", 0);
		control.ScoreUpdate (0);
		PlayerPrefs.SetInt ("WeaponUpgrade", 0);
		PlayerPrefs.SetInt ("ArmorUpgrade", 0);
		PlayerPrefs.SetInt ("MoveUpgrade", 0);
	}

	void Update ()
	{
		GameObject Player = GameObject.FindGameObjectWithTag ("Player");
		if (Player != null) 
		{
			healthSlider.value = Player.gameObject.GetComponent<MovePlayer> ().playerHealth;

			if (PlayerPrefs.GetInt("ArmorUpgrade") == 3 && (Time.time - Player.gameObject.GetComponent<MovePlayer>().lastShield) > Player.gameObject.GetComponent<MovePlayer>().shieldCooldown) 
			{
				shieldText.text = "[Energy Shield Available!]";
			}
			else shieldText.text = "[Energy Shield Unavailable.]";		
		}
		else
		{
			PlayerPrefs.SetInt ("currentScore",0);
			RP = 0;
		}

		GameObject pBase = GameObject.FindGameObjectWithTag("PlayerBase");
		GameObject eBase = GameObject.FindGameObjectWithTag("EnemyBase");
		if (pBase != null) 
		{
			playerBaseSlider.value = pBase.gameObject.GetComponent<PlayerBase> ().playerBaseHealth;
			playerBaseSlider.maxValue = pBase.gameObject.GetComponent<PlayerBase> ().maxHealth;
		}
		else
		{
			PlayerPrefs.SetInt ("currentScore",0);
			RP = 0;
		}

		if (eBase != null) 
		{
			enemyBaseSlider.value = eBase.gameObject.GetComponent<EnemyBase> ().enemyBaseHealth;
			enemyBaseSlider.maxValue = eBase.gameObject.GetComponent<EnemyBase> ().maxHealth;
		}


		if( cannonSlider.value < 2.0f )
		{
			if (PlayerPrefs.GetInt ("WeaponUpgrade") == 1) 
			{
				cannonSlider.value += 1.66f * Time.deltaTime;
			}

			else if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 2) 
			{
				cannonSlider.value += 2.0f * Time.deltaTime;
			}

			else 
			{
				cannonSlider.value += Time.deltaTime;
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

		accuracy = shotsHit / shotsFired;

		/* increase health slider as upgraded. should be easy.... stupid bs.
		if (PlayerPrefs.GetInt ("ArmorUpgrade") == 1) 
		{
			healthSlider.maxValue = 5.0f;		
			healthSlider.transform.position.x = new Vector3(65,10,0);
			
			RectTransform sliderRect = healthSlider.GetComponent<RectTransform>();		
			int h = sliderRect.rect.height;
			int w = sliderRect.rect.width;
			sliderRect.rect.sizeDelta(w*1.25f,h);
		}
		*/


	}//end update

	public void ScoreUpdate(int points)
	{
		if (points == 0) 
		{
			RP = 0;
		} 
		else 
		{
			RP += points;
		}

		score += points;
		scoreText.text = "Score: " + score.ToString () + "\nWave: " + (waves+1).ToString();
		PlayerPrefs.SetInt ("currentScore",score);

		if (score >= PlayerPrefs.GetInt("highScore"))
		{
			PlayerPrefs.SetInt("highScore",score);
			highScore = PlayerPrefs.GetInt ("highScore");
		}

	}

	public void VisualizeRedDmg ()
	{
		bloodFlash.color = new Color32 (229,37,37,64);
		Invoke("ClearRedDmg",.2f);
	}

	public void ClearRedDmg ()
	{
		bloodFlash.color = new Color32 (229, 37, 37, 0);
	}

}
