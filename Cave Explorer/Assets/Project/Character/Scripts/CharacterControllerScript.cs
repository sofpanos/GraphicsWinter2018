using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {

	public float speed = 10f;
	public float step = 0.5f;
	Vector3 previousPosition;
	private bool cursorLocked;
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		
		this.previousPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

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
	private void FixedUpdate()
	{
		
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
					exit.GetComponent<DoorScript>().openCloseDoor(transform.gameObject);
				}
				else if (hit.transform.tag == "ExitSwitch")
				{
					GameObject exitSwitch = GameObject.Find("ExitSwitch");
					exitSwitch.GetComponent<ExitSwitchScript>().initiateSwitch(transform.gameObject);
				}
			}
		}
	}

	private void InternalLockUpdate()
	{
		
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			cursorLocked = false;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			cursorLocked = true;
		}
		if (!cursorLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			
		}
	}

}
