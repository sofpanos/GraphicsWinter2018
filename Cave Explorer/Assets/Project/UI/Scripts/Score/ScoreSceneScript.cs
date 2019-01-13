using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSceneScript : MonoBehaviour
{
	public Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
		if(Cursor.lockState != CursorLockMode.None)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		ScoreText.text = String.Format("Your time is: {0:D2}:{1:D2}:{2:D2}", Initializer.score.Hours, Initializer.score.Minutes, Initializer.score.Seconds);
    }

	public void OnPlayAgain()
	{
		Initializer.level = 0;
		Initializer.LevelTimes = new List<System.TimeSpan>();
		SceneManager.LoadScene("LoadingScene");
	}

	public void OnMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
