using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToolTipScript : MonoBehaviour {
	public static string tooltipText;
	public static bool showLocked;
	private DateTime TipTime;
	private TimeSpan MaxShowTime = new TimeSpan(0, 0, 5);
	private bool tipShown;

	// Use this for initialization
	void Start () {
		tooltipText = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (showLocked)
		{
			GetComponent<Text>().text = tooltipText;
			showLocked = false;
			tipShown = true;
			TipTime = DateTime.Now;
		}
		else
		{
			if (tipShown)
			{
				if((DateTime.Now - TipTime) > MaxShowTime)
				{
					GetComponent<Text>().text = "";
					tipShown = false;
				}
			}
		}
		

	}
}
