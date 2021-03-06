/*

Title screen manager with simple button animation

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour {

	public Text highScoreText;
	public Button StartButton;
	public Button InfoButton;
	public Button QuitButton;
	public Button OK;
	public Button TurboButton;
	private float startTime;
	public Canvas menu;
	public Canvas info;
	public int turboHidden = 0;

	private bool sound1played = false;
	private bool sound2played = false;
	private bool sound3played = false;

	public AudioClip shootSound;
	private AudioSource source1;
	public AudioClip hitSound;
	//private AudioSource source2;
	public AudioClip deathSound;
	//private AudioSource source3;

	void Awake()
	{
		source1 = GetComponent<AudioSource> ();
	}

	public void callStartButton()
	{
		SpawnDrone.turboLevel = false;
		SceneManager.LoadScene("scene");
	}

	public void callQuitButton ()
	{
		Application.Quit();
	}	

	//hide menu UI, display info
	public void callInfoButton()
	{
		menu.enabled = false;
		info.gameObject.SetActive(true);
		turboHidden++;
	}

	//hide info UI, display menu
	public void callOKButton()
	{
		menu.enabled = true;
		info.gameObject.SetActive(false);
	}

	public void callTurboButton()
	{
		HUD.waves = 99;
		//SpawnDrone.firstLevel = true;
		SpawnDrone.turboLevel = true;
		SceneManager.LoadScene("scene");
	}

	// Use this for initialization
	void Start() 
	{
		HUD.highScore = PlayerPrefs.GetInt ("highScore");
		highScoreText.text = "High Score: " + HUD.highScore.ToString ();

		StartButton.interactable = false;
		InfoButton.interactable = false;
		QuitButton.interactable = false;
		TurboButton.interactable = false;
		info.gameObject.SetActive(false);

		startTime = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{	
		//move buttons into place
		//magic number -78 due to parent object canvas position
		//if (StartButton.transform.position.x < 300-78) 
		if(Time.time - startTime < 0.9f)
		{
			StartButton.transform.Translate (Time.deltaTime * 220, 0, 0);
			//StartButton.transform.Translate (4, 0, 0);
		}
		else if(!sound1played)
		{
			sound1played = true;
			source1.PlayOneShot (shootSound, 1);
		}	
		
		//if (InfoButton.transform.position.x < 400-78) 
		if(Time.time - startTime < 1.6f)
		{
			InfoButton.transform.Translate (Time.deltaTime * 220, 0, 0);
			//InfoButton.transform.Translate (4, 0, 0);
		}
		else if(!sound2played)
		{
			sound2played = true;
			source1.PlayOneShot (hitSound, 1);
		}	

		//if (QuitButton.transform.position.x < 500-78) 
		if(Time.time - startTime < 2.2f)
		{
			QuitButton.transform.Translate (Time.deltaTime * 220, 0, 0);
			//QuitButton.transform.Translate (4, 0, 0);
		}
		else if(!sound3played)
		{
			sound3played = true;
			source1.PlayOneShot (deathSound, 1);
		}	

		else 
		{
			StartButton.interactable = true;
			InfoButton.interactable = true;
			QuitButton.interactable = true;
		}

		if (turboHidden >= 3) 
		{
			TurboButton.interactable = true;
		}

	}
}
