using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorScript : MonoBehaviour {

	private Animator animator;
	public bool Locked = true;//θα γίνουν private
	public bool open = false;//και αυτό, τα άφησα public για λόγους debugging να τα παρακολουθω από το inspector window
	
	// Use this for initialization
	void Start () {
		Locked = true;
		open = false;
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!Locked && open)
			{
				GameObject game = GameObject.Find("Game");
				foreach(Transform child in game.transform)
				{
					Destroy(child.gameObject);
				}
				Initializer initializer = game.GetComponent<Initializer>();
				initializer.startNextLevel();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
		
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
	}

	public void setLocked(bool value)
	{
		Locked = value;
	}
}
