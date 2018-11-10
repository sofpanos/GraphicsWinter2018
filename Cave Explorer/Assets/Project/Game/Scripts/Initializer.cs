using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Factories;
using Core.Utility;
using System;
using UnityEngine.UI;
using Core.Constructions;

public class Initializer : MonoBehaviour {
    public GameMap map;
	public GameObject roof;
    public GameObject wall;
    public GameObject floor;
    public GameObject player;
    public int level;
    public int width;
    public int height;
	public DateTime startTime;

	// Use this for initialization
	void Start () {
		map = MapFactory.getNewGameMap(width, height, 1);
		bool first = true;
		foreach (Room room in map.getRooms())
		{
			GameObject roomObject = new GameObject();
            roomObject.transform.SetParent(((GameObject)GameObject.Find("Game")).transform);
			roomObject.name = room.getID();

			if (first)
            {
                Position playerPosition = room.getFloorPositions()[UnityEngine.Random.Range(0, room.getFloorPositions().Count)];
				GameObject newPlayer = (GameObject)Instantiate(player);
				newPlayer.name = "Player";
                newPlayer.transform.position = new Vector3(playerPosition.getX() * 2, 2, playerPosition.getY() * 2);
                newPlayer.transform.SetParent(((GameObject)GameObject.Find("Game")).transform);
				first = false;
            }

			GameObject wallParent = new GameObject();
			wallParent.name = room.getID() + "_Wall";
			wallParent.transform.SetParent(roomObject.transform);

			GameObject roofParent = new GameObject();
			roofParent.name = room.getID() + "_Roof";
			roofParent.transform.SetParent(roomObject.transform);

			foreach (Position wallPosition in room.getWallPositions()){
                GameObject roomWall = (GameObject)Instantiate(wall);
                Vector3 wallTransPos = new Vector3(wallPosition.getX() * 2, 2, wallPosition.getY() * 2);
				roomWall.name = room.getID() + "_Wall_" + wallPosition.getX() + "_" + wallPosition.getY();
                roomWall.transform.position = wallTransPos;
                roomWall.transform.SetParent(wallParent.transform);
				if(UnityEngine.Random.Range(0, 100) < 30)
				{
					roomWall.transform.Rotate(Vector3.up, 90f); 
				}
				else if(UnityEngine.Random.Range(0, 100) < 60)
				{
					roomWall.transform.Rotate(Vector3.up, -90f);
				}
				//Create Roof over wall, avoiding light through the edges
				GameObject roofObj = (GameObject)Instantiate(roof);
				roofObj.name = roomObject.name + "_Roof_" + wallPosition.getX() + "_" + wallPosition.getY();
				roofObj.transform.position = new Vector3(wallPosition.getX() * 2, 4, wallPosition.getY() * 2);
				roofObj.transform.SetParent(roofParent.transform);
            }

			GameObject floorParent = new GameObject();
			floorParent.name = room.getID() + "_Floor";
			floorParent.transform.SetParent(roomObject.transform);

			foreach (Position floorPosition in room.getFloorPositions())
            {
                GameObject roomFloor = (GameObject)Instantiate(floor);
                Vector3 floorTransPos = new Vector3(floorPosition.getX() * 2, 0, floorPosition.getY() * 2);
				roomFloor.name = room.getID() + "_Floor_" + floorPosition.getX() + "_" + floorPosition.getY();
                roomFloor.transform.position = floorTransPos;
                roomFloor.transform.SetParent(floorParent.transform);

				//Create Roof
				GameObject roofObj = (GameObject)Instantiate(roof);
				roofObj.name = roomObject.name + "_Roof_" + floorPosition.getX() + "_" + floorPosition.getY();
				roofObj.transform.position = new Vector3(floorPosition.getX() * 2, 4, floorPosition.getY() * 2);
				roofObj.transform.SetParent(roofParent.transform);
			}
        }

        foreach (Hallway hall in map.getHallways())
        {
            GameObject hallObject = new GameObject();
            hallObject.transform.SetParent((GameObject.Find("Game")).transform);
			hallObject.name = hall.getID();

			GameObject wallObject = new GameObject();
			wallObject.name = hall.getID() + "_Wall";
			wallObject.transform.SetParent(hallObject.transform);

			//Create Roof Parent
			GameObject roofParent = new GameObject();
			roofParent.name = hallObject.name + "_Roof";
			roofParent.transform.SetParent(hallObject.transform);

            foreach (Position wallPosition in hall.getWallPositions())
            {
                GameObject hallWall = (GameObject)Instantiate(wall);
                Vector3 wallTransPos = new Vector3(wallPosition.getX() * 2, 2, wallPosition.getY() * 2);
				hallWall.name = hall.getID() + "_Wall_" + wallPosition.getX() + "_" + wallPosition.getY();
                hallWall.transform.position = wallTransPos;
                hallWall.transform.SetParent(wallObject.transform);
				
				//Create Roof over wall to avoid light passing through the edges
				GameObject roofObj = (GameObject)Instantiate(roof);
				roofObj.name = hallObject.name + "_Roof_" + wallPosition.getX() + "_" + wallPosition.getY();
				roofObj.transform.position = new Vector3(wallPosition.getX() * 2, 4, wallPosition.getY() * 2);
				roofObj.transform.SetParent(roofParent.transform);
			}

			GameObject floorObject = new GameObject();
			floorObject.name = hallObject.name + "_Floor";
			floorObject.transform.SetParent(hallObject.transform);

            foreach(Position floorPosition in hall.getPath())
            {
                GameObject hallFloor = (GameObject)Instantiate(floor);
                Vector3 floorTransPos = new Vector3(floorPosition.getX() * 2, 0, floorPosition.getY() * 2);
				hallFloor.name = hall.getID() + "_Floor_" + floorPosition.getX() + "_" + floorPosition.getY();
                hallFloor.transform.position = floorTransPos;
                hallFloor.transform.SetParent(floorObject.transform);

				//Create Roof
				GameObject roofObj = (GameObject)Instantiate(roof);
				roofObj.name = hallObject.name + "_Roof_" + floorPosition.getX() + "_" + floorPosition.getY();
				roofObj.transform.position = new Vector3(floorPosition.getX() * 2, 4, floorPosition.getY() * 2);
				roofObj.transform.SetParent(roofParent.transform);
			}
        }
		startTime = DateTime.Now;
		
	}
	public void startNextLevel()
	{
		if (level == 3)
		{
			//calculate score and end game
		}
		else
		{
			level++;
			Start();
		}
	}
	
	// Update is called once per frame
	void Update () {
		TimeSpan timeEllapsed = DateTime.Now - startTime;
		GameObject.Find("Time").GetComponent<Text>().text = String.Format("Time: {0,2:D2}:{0,2:D2}:{0,2:D2}", timeEllapsed.Hours, timeEllapsed.Minutes, timeEllapsed.Seconds);
	}
}
