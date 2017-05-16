using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour {

	public Text scoreText;
	public Text highScoreText;
	public Text accuracyBonusText;
	public Text rpText;
	public static int RP; //RP resource points

	public Button weaponLvl1Button;
	public Button weaponLvl2Button;
	public Button weaponLvl3Button;
	public Button armorLvl1Button;
	public Button armorLvl2Button;
	public Button armorLvl3Button;
	public Button movementLvl1Button;
	public Button movementLvl2Button;
	public Button movementLvl3Button;

	public void NextWaveButton(string NextWave)
	{
		HUD.waves++;
		SceneManager.LoadScene(NextWave);
	}

	public void WeaponLvl1Upgrade()
	{
		if (PlayerPrefs.GetInt ("WeaponUpgrade") == 0) 
		{
			RP -= 100;
			PlayerPrefs.SetInt ("WeaponUpgrade", 1);
		}
	}

	public void WeaponLvl2Upgrade()
	{
		if (PlayerPrefs.GetInt ("WeaponUpgrade") == 1) 
		{
			RP -= 200;
			PlayerPrefs.SetInt ("WeaponUpgrade", 2);
		}
	}

	public void WeaponLvl3Upgrade()
	{
		if (PlayerPrefs.GetInt ("WeaponUpgrade") == 2) 
		{
			RP -= 300;
			PlayerPrefs.SetInt ("WeaponUpgrade", 3);
		}
	}

	public void ArmorLvl1Upgrade()
	{
		if (PlayerPrefs.GetInt ("ArmorUpgrade") == 0) 
		{
			RP -= 100;
			PlayerPrefs.SetInt ("ArmorUpgrade", 1);
		}
	}

	public void ArmorLvl2Upgrade()
	{
		if (PlayerPrefs.GetInt ("ArmorUpgrade") == 1) 
		{
			RP -= 200;
			PlayerPrefs.SetInt ("ArmorUpgrade", 2);
		}
	}

	public void ArmorLvl3Upgrade()
	{
		if (PlayerPrefs.GetInt ("ArmorUpgrade") == 2) 
		{
			RP -= 300;
			PlayerPrefs.SetInt ("ArmorUpgrade", 3);
		}
	}

	public void MovementLvl1Upgrade()
	{
		if (PlayerPrefs.GetInt ("MoveUpgrade") == 0) 
		{
			RP -= 100;
			PlayerPrefs.SetInt ("MoveUpgrade", 1);
		}
	}

	// Use this for initialization
	void Start () 
	{
		RP += HUD.RP; //pull in accumulated RP from HUD for spending
		scoreText.text = "Score: " + HUD.score.ToString ();
		highScoreText.text = "High Score: " + HUD.highScore.ToString ();
		accuracyBonusText.text = "Wave Accuracy: " + (100*HUD.accuracy).ToString ("F2") + " %"; 
		rpText.text = "Avialable Resource Points: "+RP + "rp";
	}
	
	// Update is called once per frame
	void Update () 
	{
		rpText.text = "Avialable Resource Points: " + RP + "rp";
			
		//lock interactable buttons with cost and upgrade paths
		if(RP >= 100)
		{
			weaponLvl1Button.interactable = true;
			armorLvl1Button.interactable = true;
			movementLvl1Button.interactable = true;
		}
		else
		{
			weaponLvl1Button.interactable = false;
			weaponLvl2Button.interactable = false;
			weaponLvl3Button.interactable = false;

			armorLvl1Button.interactable = false;
			armorLvl2Button.interactable = false;
			armorLvl3Button.interactable = false;

			movementLvl1Button.interactable = false;
		}

		if (RP >= 200) 
		{
			if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 1) 
			{
				weaponLvl2Button.interactable = true;
			}
			else 
			{
				weaponLvl2Button.interactable = false;
			}

			if (PlayerPrefs.GetInt ("ArmorUpgrade") >= 1) 
			{
				armorLvl2Button.interactable = true;
			} 
			else 
			{
				armorLvl2Button.interactable = false;
			}

		} 
		else 
		{
			weaponLvl2Button.interactable = false;
			weaponLvl3Button.interactable = false;

			armorLvl2Button.interactable = false;
			armorLvl3Button.interactable = false;
		}	

		if (RP >= 300)
		{
			if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 2) 
			{
				weaponLvl3Button.interactable = true;
			}
			else
			{
				weaponLvl3Button.interactable = false;
			}

			if (PlayerPrefs.GetInt ("ArmorUpgrade") >= 2) 
			{
				armorLvl3Button.interactable = true;
			}
			else
			{
				armorLvl3Button.interactable = false;
			}
		}
		else 
		{
			weaponLvl3Button.interactable = false;
			armorLvl3Button.interactable = false;
		}	
	


		//make selected colors stick - weapon
		if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 1) 
		{
			weaponLvl1Button.interactable = true;
			Button b = weaponLvl1Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			weaponLvl1Button.Select ();
		}
		if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 2) 
		{
			weaponLvl2Button.interactable = true;
			Button b = weaponLvl2Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			weaponLvl2Button.Select ();
		}
		if (PlayerPrefs.GetInt ("WeaponUpgrade") >= 3) 
		{
			weaponLvl3Button.interactable = true;
			Button b = weaponLvl3Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			weaponLvl3Button.Select ();
		}

		//make selected colors stick - armor
		if (PlayerPrefs.GetInt ("ArmorUpgrade") >= 1) 
		{
			armorLvl1Button.interactable = true;
			Button b = armorLvl1Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			armorLvl1Button.Select ();
		}
		if (PlayerPrefs.GetInt ("ArmorUpgrade") >= 2) 
		{
			armorLvl2Button.interactable = true;
			Button b = armorLvl2Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			armorLvl2Button.Select ();
		}
		if (PlayerPrefs.GetInt ("ArmorUpgrade") >= 3) 
		{
			armorLvl3Button.interactable = true;
			Button b = armorLvl3Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			armorLvl3Button.Select ();
		}

		if (PlayerPrefs.GetInt ("MoveUpgrade") >= 1) 
		{
			movementLvl1Button.interactable = true;
			Button b = movementLvl1Button.GetComponent<Button> ();
			ColorBlock cb = b.colors;
			cb.normalColor = new Color32 (110,123,249,255);
			b.colors = cb;
			movementLvl1Button.Select ();
		}

			
	}//end update
}
