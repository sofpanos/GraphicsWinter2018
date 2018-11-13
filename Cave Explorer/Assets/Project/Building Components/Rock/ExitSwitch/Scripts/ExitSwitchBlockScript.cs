using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitchBlockScript : MonoBehaviour {

	public GameObject SwitchPrefab;

	private void Awake()
	{
		GameObject switchObj = (GameObject)Instantiate(SwitchPrefab);
		switchObj.transform.SetParent(this.transform);
		switchObj.transform.localPosition = new Vector3(0, 0, 1.03f);
		switchObj.transform.Rotate(Vector3.up, this.transform.rotation.eulerAngles.y);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
