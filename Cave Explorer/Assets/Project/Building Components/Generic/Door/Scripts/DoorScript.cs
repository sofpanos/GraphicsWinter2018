using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class DoorScript : MonoBehaviour {

	private Animator animator;
	private AudioSource audioSource;
	private DateTime LockedTipTime;
	private TimeSpan TipTime = new TimeSpan(0, 0, 5);
	private bool Locked = true;
	private bool open = false;
	private bool toolTipShown = false;

	public AudioClip LockedSound;
	public AudioClip OpenCloseSound;

	// Use this for initialization
	void Start () {
		Locked = true;
		open = false;
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!Locked && open)
			{
				/*
				GameObject game = GameObject.Find("Game");
				foreach(Transform child in game.transform)
				{
					Destroy(child.gameObject);
				}
				Initializer initializer = game.GetComponent<Initializer>();
				initializer.startNextLevel();*/
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Locked)
		{
			audioSource.clip = LockedSound;
		}
		else
		{
			audioSource.clip = OpenCloseSound;
		}

		if (LockedTipTime != null)
		{
			if((DateTime.Now - LockedTipTime) > TipTime)
			{
				GameObject.Find("HintTooltip").GetComponent<Text>().text = "";
			}
		}
		
	}

	public void openCloseDoor(GameObject initiator)
	{
		if(initiator.tag != "Player")
		{
			return;
		}
		
		if (!Locked)
		{
			if (!open)
			{
				animator.SetBool("open", true);
				open = true;
			}
			else
			{
				animator.SetBool("close", true);
				open = false;
			}
		}
		else
		{
			GameObject toolTip = (GameObject)GameObject.Find("HintToolTip");
			toolTip.GetComponent<Text>().text = "Door Locked\nFind the Switch to unlock!";
			LockedTipTime = DateTime.Now;
			audioSource.Play();
		}
	}

	public void setLocked(bool value)
	{
		Locked = value;
	}

	public void OnOpenCloseAnimation()
	{
		audioSource.clip = OpenCloseSound;
		audioSource.Play();
	}
}
