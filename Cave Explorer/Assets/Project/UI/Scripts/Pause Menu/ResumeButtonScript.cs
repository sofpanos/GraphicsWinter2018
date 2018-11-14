using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButtonScript : MonoBehaviour {
	public GameObject PauseMenu;

	public void OnResumeButtonClicked()
	{
		PauseMenu.GetComponent<Animator>().SetBool("exit", true);
	}

	public void OnResumeAnimationComplete()
	{
		PauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}

}
