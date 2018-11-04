﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
	GameObject character;
	public float sensitivity = 5f;
	public float smoothing = 2f;
	public float MinimumX = -90f;
	public float MaximumX = 90f;
	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
		mouseLook += smoothV;

		transform.localRotation = ClampRotationAroundXAxis(Quaternion.AngleAxis(-mouseLook.y, Vector3.right));
		character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

	}
	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

		angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}
}
