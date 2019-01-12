using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingPanelScript : MonoBehaviour
{
	public Slider ProgressBar;
	public Text LoadingText;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(LoadLevel());    
    }

	IEnumerator LoadLevel()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(1);

		while (!async.isDone)
		{
			ProgressBar.value = Mathf.Clamp01(async.progress / .9f) * 100;
			LoadingText.text = "Loading... " + ProgressBar.value + "%";
			yield return null;
		}
	}
}
