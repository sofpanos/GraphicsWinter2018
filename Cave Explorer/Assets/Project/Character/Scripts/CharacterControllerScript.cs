using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {

	public float speed = 10f;
	public float step = 0.5f;
	Vector3 previousPosition;
	public static bool cursorLocked;
	// Use this for initialization
	void Start () {
		cursorLocked = true;//For initial cursor lock
		this.previousPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (PauseMenuScript.GamePaused)
		{
			InternalLockUpdate();
			return;
		}

		float zAxis = Input.GetAxis("Vertical") * speed;
		float xAxis = Input.GetAxis("Horizontal") * speed;
		zAxis *= Time.deltaTime;
		xAxis *= Time.deltaTime;

		transform.Translate(xAxis, 0, zAxis);

		if (Mathf.Abs((previousPosition - transform.position).magnitude) >= step)
		{
			AudioSource audio = GetComponent<AudioSource>();
			audio.pitch = Mathf.Lerp(0.8f, 1f, Random.value);
			if (!audio.isPlaying)
				audio.Play();
			previousPosition = transform.position;
		}
		
		InternalLockUpdate();
		checkRayCast();

	}
	
	private void checkRayCast()
	{
		RaycastHit hit;
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(this.transform.GetChild(0).position, this.transform.GetChild(0).forward, out hit, 2f))
			{
				if (hit.transform.tag == "Exit")
				{
					GameObject exit = GameObject.Find("Exit");

                    if (exit == null)
                        exit = GameObject.Find("MountainExit");

                    exit.GetComponentInChildren<DoorScript>().openCloseDoor(transform.gameObject);
				}
				else if (hit.transform.tag == "ExitSwitch")
				{
					GameObject exitSwitch = GameObject.Find("ExitSwitch");

                    if (exitSwitch == null)
                        exitSwitch = GameObject.Find("MountainExitSwitch");

                    exitSwitch.GetComponentInChildren<ExitSwitchScript>().initiateSwitch(transform.gameObject);
				}
                else if (hit.transform.tag == "Lever")
                {
                    hit.transform.parent.GetComponent<TorchSwitchScript>().onLeverClick();
                }
			}
		}
	}

	private void InternalLockUpdate()
	{
		if (!cursorLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			
		}
	}

}
