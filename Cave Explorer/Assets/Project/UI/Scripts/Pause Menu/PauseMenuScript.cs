using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PauseMenuScript : MonoBehaviour {
	public GameObject PauseMenu;
	public GameObject TimeUI;
	public static bool GamePaused = false;

	public void OnPause()
	{
		Time.timeScale = 0f;
		GamePaused = true;
		TimeUI.GetComponent<TimeScript>().PauseTime = DateTime.Now;
		CharacterControllerScript.cursorLocked = false;
		PauseMenu.SetActive(true);
	}

	public void OnResume()
	{
		GamePaused = false;
		Time.timeScale = 1f;
		CharacterControllerScript.cursorLocked = true;
		PauseMenu.SetActive(false);
	}

	public void OnMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GamePaused)
			{
				OnResume();
			}
			else
			{
				OnPause();
			}
		}
	}
}
