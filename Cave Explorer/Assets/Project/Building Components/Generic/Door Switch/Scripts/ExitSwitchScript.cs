using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitchScript : MonoBehaviour {

	bool activated = false;
	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
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
				GameObject.Find("Exit").GetComponent<DoorScript>().setLocked(false);
				animator.SetBool("activate", true);
			}
			else
			{
				GameObject.Find("Exit").GetComponent<DoorScript>().setLocked(true);
				animator.SetBool("deactivate", true);
			}
		}
	}
}
