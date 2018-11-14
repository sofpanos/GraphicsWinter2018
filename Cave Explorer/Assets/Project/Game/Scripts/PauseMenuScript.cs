using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
	public GameObject PauseMenu;

	public void OnPause()
	{
		Time.timeScale = 0f;
		PauseMenu.SetActive(true);
		PauseMenu.GetComponent<Animator>().SetBool("enter", true);
	}

	public void OnQuitButtonClicked()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OnMainMenuButtonClicked()
	{
		SceneManager.LoadScene(0);
	}
}
