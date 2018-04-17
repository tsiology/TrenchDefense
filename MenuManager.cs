/*

Menu manager for in between levels.
Display stats, allow player to continue or return to title.

tgodwin

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;
	public Text accuracyBonusText;
	public Text upgradeMsg;
	public Text checkpointMsg;
	private string[] upgrades = {"Main Gun reload speed upgraded!","Tank armor upgraded!","Tank movement speed upgraded!",
		"Main Gun reload and bullet speed upgraded!","Tank armor upgraded!","Tank movement speed upgraded!",
	    "Main Gun upgraded with 3-Round-Burst!","Tank armor upgraded with Auto-Shield!"};


	public void NextWaveButton(string NextWave)
	{
		HUD.waves++;
		SceneManager.LoadScene(NextWave);
	}
		
	public void TitleReturn()
	{
		HUD.NewGameStats ();
		SceneManager.LoadScene("TitleMenu");
	}

	// Use this for initialization
	void Start () 
	{
		scoreText.text = "Score: " + HUD.score.ToString ();
		highScoreText.text = "High Score: " + HUD.highScore.ToString ();
		accuracyBonusText.text = "  Wave Accuracy: " + (100*HUD.accuracy).ToString ("F2") + "%"; 
		if(HUD.waves <= 7)
			upgradeMsg.text = upgrades [HUD.waves];
		else
			upgradeMsg.text = "Your tank is fully upgraded!";

		if (HUD.waves == 2 || HUD.waves == 5 || HUD.waves == 8)
			checkpointMsg.text = "***Checkpoint Achieved***";
		else
			checkpointMsg.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
