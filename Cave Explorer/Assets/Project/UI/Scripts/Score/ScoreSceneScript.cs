using System.Collections;
using System.Collections.Generic;
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

		ScoreText.text = "Your Score is: " + Initializer.score;
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
