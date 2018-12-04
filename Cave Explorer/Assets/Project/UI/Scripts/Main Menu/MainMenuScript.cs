using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
	public GameObject WidthSlider;
	public GameObject HeightSlider;
	public GameObject HeightValueLabel;
	public GameObject WidthValueLabel;
	public const int HeightSliderIndex = 1;
	public const int WidthSliderIndex = 0;


	// Use this for initialization
	private void Start()
	{
		if(SceneManager.GetSceneByName("GameScene").isDirty)
			SceneManager.UnloadSceneAsync("GameScene");
		
		
		WidthSlider.GetComponent<Slider>().value = Initializer.width;
		HeightSlider.GetComponent<Slider>().value = Initializer.height;
	}

	public void OnSliderValueChange(int sliderIndex)
	{
		if(sliderIndex == WidthSliderIndex)
		{
			Initializer.width = (int)WidthSlider.GetComponent<Slider>().value;
			WidthValueLabel.GetComponent<Text>().text = Initializer.width.ToString();
		}
		else
		{
			Initializer.height = (int)HeightSlider.GetComponent<Slider>().value;
			HeightValueLabel.GetComponent<Text>().text = Initializer.height.ToString();
		}
	}

	public void OnStart()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}
