using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;
	public Text wavesText;

//	GameObject HUDobj = GameObject.FindGameObjectWithTag("HUD");

	public void NewGameButton(string New)
	{
		//HUDobj.GetComponent<HUD> ().NewGameStats ();
		HUD.NewGameStats();
		HUD.RP = 0;
		MenuManager.RP = 0;
		SpawnDrone.firstLevel = true;	
		SceneManager.LoadScene(New);
	}


	// Use this for initialization
	void Start () 
	{
		scoreText.text = "Score: " + HUD.score.ToString ();
		highScoreText.text = "High Score: " + HUD.highScore.ToString ();
		wavesText.text = "Waves Completed: " + HUD.waves.ToString (); 
	}

	// Update is called once per frame
	void Update () {

	}
}
