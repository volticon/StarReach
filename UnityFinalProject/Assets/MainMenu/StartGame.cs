using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour {
	public Text score;
	public Text highScore;
	void Update ()
	{
		if (Application.loadedLevelName == "GameOver")
		{
			score.text = "Score: " + PlayerPrefs.GetInt("Score");
			switch (PlayerPrefs.GetString("level"))
			{
			case "Gameplay":
				highScore.text = "High Score: " + PlayerPrefs.GetInt("High Score");
				break;
			case "Map_Classic":
				highScore.text = "High Score: " + PlayerPrefs.GetInt("Classic High Score");
				break;
			case "Map_SkyReach":
				highScore.text = "High Score: " + PlayerPrefs.GetInt("SkyReach High Score");
				break;
			}
		}
	}

	public void loadToybox()
	{
		PlayerPrefs.SetString("level", "Gameplay");
		Application.LoadLevel(PlayerPrefs.GetString("level"));
	}
	public void loadClassic ()
	{
		PlayerPrefs.SetString("level", "Map_Classic");
		Application.LoadLevel(PlayerPrefs.GetString("level"));
	}
	public void loadSkyReach ()
	{
		PlayerPrefs.SetString("level", "Map_SkyReach");
		Application.LoadLevel(PlayerPrefs.GetString("level"));
	}
	public void Replay ()
	{
		Application.LoadLevel(PlayerPrefs.GetString("level"));
	}
	public void loadLvlSelect ()
	{
		Application.LoadLevel("LevelSelect");
	}
}