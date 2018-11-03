﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {

	public float speed = 10f;
	public float step = 0.5f;
	Vector3 previousPosition;
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		
		this.previousPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		
	}
	private void FixedUpdate()
	{
		float zAxis = Input.GetAxis("Vertical") * speed;
		float xAxis = Input.GetAxis("Horizontal") * speed;
		zAxis *= Time.deltaTime;
		xAxis *= Time.deltaTime;

		transform.Translate(xAxis, 0, zAxis);

		if (Mathf.Abs((previousPosition - transform.position).magnitude) >= step)
		{
			AudioSource audio = GetComponent<AudioSource>();
			audio.pitch = Mathf.Lerp(0.8f, 1f, Random.value);
			if(!audio.isPlaying)
				audio.Play();
			previousPosition = transform.position;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}
}