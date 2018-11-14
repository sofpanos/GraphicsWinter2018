using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour {

	public DateTime StartTime;
	private bool timePaused = false;
	public DateTime PauseTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PauseMenuScript.GamePaused)
		{
			timePaused = true;
		}
		else
		{
			if (timePaused)
			{
				timePaused = false;
				StartTime = StartTime.Add(DateTime.Now - PauseTime);
			}
			TimeSpan timeEllapsed = DateTime.Now - StartTime;
			GameObject.Find("Time").GetComponent<Text>().text 
				= String.Format("Time: {0:D2}:{1:D2}:{2:D2}", timeEllapsed.Hours, timeEllapsed.Minutes, timeEllapsed.Seconds);
		}
	}

	public TimeSpan GetCurrentTime()
	{
		return DateTime.Now - StartTime;
	}
}
