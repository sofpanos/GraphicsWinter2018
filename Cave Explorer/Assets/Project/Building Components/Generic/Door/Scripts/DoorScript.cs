using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorScript : MonoBehaviour {

	private Animator animator;
	public bool Locked = true;
	public bool open = false;
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
}
