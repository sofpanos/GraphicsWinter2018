using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCycleScript : MonoBehaviour {

	public GameObject CanvasObject;

	private void Start()
	{
		if (SceneManager.GetSceneByName("MainMenu").isLoaded)
		{
			SceneManager.UnloadSceneAsync(0);
		}
	}

	public void OnPause()
	{
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (PauseMenuScript.GamePaused)
			{
				CanvasObject.GetComponent<PauseMenuScript>().OnPause();
			}
			else
			{
				CanvasObject.GetComponent<PauseMenuScript>().OnResume();
			}
		}
	}
}
