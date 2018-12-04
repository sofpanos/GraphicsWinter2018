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
		CharacterControllerScript.cursorLocked = false;
		Time.timeScale = 0f;
		GamePaused = true;
		TimeUI.GetComponent<TimeScript>().PauseTime = DateTime.Now;
		PauseMenu.SetActive(true);
	}

	public void OnResume()
	{
		CharacterControllerScript.cursorLocked = true;
		GamePaused = false;
		Time.timeScale = 1f;
		PauseMenu.SetActive(false);
	}

	public void OnMenu()
	{
		GamePaused = false;
		Time.timeScale = 1f;
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
