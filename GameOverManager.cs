/*  

This script acts as manager for game over scene, allowing reload from a checkpoint, 
return to title screen, or start of new game.

tgodwin  

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;
	public Text wavesText;
	public Text checkpointText;

	public void NewGameButton(string New)
	{
		if (HUD.waves >= 3) 
		{
			LoadCheckpointButton ();
		} 
		else 
		{
			HUD.NewGameStats();
			//SpawnDrone.firstLevel = true;	
			SceneManager.LoadScene(New);	
		}
	}

	public void LoadCheckpointButton()
	{
		GameObject HUDobj = GameObject.FindGameObjectWithTag ("HUD");
		HUDobj.GetComponent<HUD> ().ScoreUpdate (-100*(HUD.waves+1));

		if (HUD.waves >= 9) 
		{
			HUD.waves = 9;
		}
		else if (HUD.waves >= 6) 
		{
			HUD.waves = 6;
		}
		else if (HUD.waves >= 3) 
		{
			HUD.waves = 3;
		}

		SceneManager.LoadScene("scene");
	}


	public void TitleScreenButton ()
	{
		HUD.NewGameStats();
		SceneManager.LoadScene("TitleMenu");
	}

	// Use this for initialization
	void Start () 
	{
		scoreText.text = "Score: " + HUD.score.ToString ();
		highScoreText.text = "High Score: " + HUD.highScore.ToString ();
		wavesText.text = "Waves Completed: " + HUD.waves.ToString (); 

		if (SpawnDrone.turboLevel) 
		{
			SpawnDrone.turboLevel = false;
			HUD.NewGameStats ();
			SceneManager.LoadScene ("TitleMenu");
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (HUD.waves >= 3 && !SpawnDrone.turboLevel) 
		{
			checkpointText.text = "Start from Checkpoint";
		}
		else
			checkpointText.text = "Start New Game";
	}
}
