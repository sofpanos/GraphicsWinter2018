using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSliderScript : MonoBehaviour {

	public GameObject ValueLabel;
	public bool IsWidth;
	// Use this for initialization
	void Start () {
		OnValueChange();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnValueChange()
	{
		ValueLabel.GetComponent<Text>().text = GetComponent<Slider>().value.ToString();
		if (IsWidth)
		{
			Initializer.width = (int)GetComponent<Slider>().value;
		}
		else
		{
			Initializer.height = (int)GetComponent<Slider>().value;
		}
	}
}
