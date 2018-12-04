using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBlockScript : MonoBehaviour {

	public GameObject doorPrefab;

	private void Awake()
	{
		GameObject door = (GameObject)Instantiate(doorPrefab);
		door.transform.SetParent(this.transform);
		door.transform.localPosition = new Vector3(0, 0.02738444f, 0.4730835f);
		door.transform.Rotate(Vector3.up, this.transform.rotation.eulerAngles.y);
	}
	
}
