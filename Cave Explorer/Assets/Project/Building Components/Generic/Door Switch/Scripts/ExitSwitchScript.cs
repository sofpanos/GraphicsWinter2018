using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitchScript : MonoBehaviour {

	private bool activated = false;
	private Animator animator;
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void initiateSwitch(GameObject initiator)
	{
		if(initiator.tag == "Player")
		{
			if (!activated)
			{
				GameObject.Find("Exit").GetComponentInChildren<DoorScript>().setLocked(false);
				animator.SetBool("activate", true);
				activated = true;
			}
			else
			{
				GameObject.Find("Exit").GetComponentInChildren<DoorScript>().setLocked(true);
				animator.SetBool("deactivate", true);
				activated = false;
			}
		}
	}

	public void OnPlaySound()
	{
		audioSource.Play();
	}
}
