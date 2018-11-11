﻿using System.Collections;
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
	public GameObject exitSwitch;
	public GameObject exit;
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

		createRooms();
		createHallways();

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

	private void createRooms()
	{
		bool first = true;
		foreach (Room room in map.getRooms())
		{
			GameObject roomObject = new GameObject();
			roomObject.transform.SetParent(transform);
			roomObject.name = room.getID();

			if (first)
			{
				Position playerPosition = room.getFloorPositions()[UnityEngine.Random.Range(0, room.getFloorPositions().Count)];
				GameObject newPlayer = (GameObject)Instantiate(player);
				newPlayer.name = "Player";
				newPlayer.transform.position = new Vector3(playerPosition.getX() * 2, 2, playerPosition.getY() * 2);
				newPlayer.transform.SetParent(transform);
				first = false;
			}

			GameObject wallParent = new GameObject();
			wallParent.name = room.getID() + "_Wall";
			wallParent.transform.SetParent(roomObject.transform);

			GameObject roofParent = new GameObject();
			roofParent.name = room.getID() + "_Roof";
			roofParent.transform.SetParent(roomObject.transform);

			createWall(room.getWallBlocks(), room.getFloorPositions(), wallParent, roofParent);

			GameObject floorParent = new GameObject();
			floorParent.name = room.getID() + "_Floor";
			floorParent.transform.SetParent(roomObject.transform);

			createFloor(room.getFloorPositions(), floorParent, roofParent);
		}
	}

	private void createHallways()
	{
		foreach (Hallway hall in map.getHallways())
		{
			GameObject hallObject = new GameObject();
			hallObject.transform.SetParent((GameObject.Find("Game")).transform);
			hallObject.name = hall.getID();

			GameObject wallParent = new GameObject();
			wallParent.name = hall.getID() + "_Wall";
			wallParent.transform.SetParent(hallObject.transform);

			//Create Roof Parent
			GameObject roofParent = new GameObject();
			roofParent.name = hallObject.name + "_Roof";
			roofParent.transform.SetParent(hallObject.transform);

			createWall(hall.getWallPositions(), wallParent, roofParent);

			GameObject floorParent = new GameObject();
			floorParent.name = hallObject.name + "_Floor";
			floorParent.transform.SetParent(hallObject.transform);

			createFloor(hall.getPath(), floorParent, roofParent);
		}
	}

	private void createFloor(List<Position> floorPositions, GameObject floorParent, GameObject roofParent)
	{
		foreach (Position floorPosition in floorPositions)
		{
			GameObject roomFloor = (GameObject)Instantiate(floor);
			Vector3 floorTransPos = new Vector3(floorPosition.getX() * 2, 0, floorPosition.getY() * 2);
			roomFloor.name = floorParent.name + "_" + floorPosition.getX() + "_" + floorPosition.getY();
			roomFloor.transform.position = floorTransPos;
			roomFloor.transform.SetParent(floorParent.transform);

			//Create Roof
			GameObject roofObj = (GameObject)Instantiate(roof);
			roofObj.name = roofParent.name + "_" + floorPosition.getX() + "_" + floorPosition.getY();
			roofObj.transform.position = new Vector3(floorPosition.getX() * 2, 4, floorPosition.getY() * 2);
			roofObj.transform.SetParent(roofParent.transform);
		}
	}

	private void createWall(Dictionary<Position, BlockType> wallBlocks, List<Position> floorPositions, GameObject wallParent, GameObject roofParent)
	{
		foreach (KeyValuePair<Position, BlockType> wallBlock in wallBlocks)
		{

			Vector3 wallTransPos = new Vector3(wallBlock.Key.getX() * 2, 2, wallBlock.Key.getY() * 2);
			switch (wallBlock.Value) {
				case BlockType.Exit:
					GameObject exitObject = (GameObject)Instantiate(exit);
					exitObject.name = "Exit";
					exitObject.transform.position = wallTransPos;
					exitObject.transform.rotation = rotateToFloor(exitObject,wallBlock.Key, floorPositions);
					break;
				case BlockType.ExitSwitch:
					GameObject exitSwitchObj = (GameObject)Instantiate(exitSwitch);
					exitSwitchObj.name = "ExitSwitch";
					exitSwitchObj.transform.position = wallTransPos;
					exitSwitchObj.transform.rotation = rotateToFloor(exitSwitchObj, wallBlock.Key, floorPositions);
					break;
				default:
					GameObject roomWall = (GameObject)Instantiate(wall);
					roomWall.name = wallParent.name + "_" + wallBlock.Key.getX() + "_" + wallBlock.Key.getY();
					roomWall.transform.position = wallTransPos;
					roomWall.transform.SetParent(wallParent.transform);

					if (UnityEngine.Random.Range(0, 100) < 30)
					{
						roomWall.transform.Rotate(Vector3.up, 90f);
					}
					else if (UnityEngine.Random.Range(0, 100) < 60)
					{
						roomWall.transform.Rotate(Vector3.up, -90f);
					}
					break;
			}

			//Create Roof over wall, avoiding light through the edges
			GameObject roofObj = (GameObject)Instantiate(roof);
			roofObj.name = roofParent.name + "_" + wallBlock.Key.getX() + "_" + wallBlock.Key.getY();
			roofObj.transform.position = new Vector3(wallBlock.Key.getX() * 2, 4, wallBlock.Key.getY() * 2);
			roofObj.transform.SetParent(roofParent.transform);
		}
	}

	private void createWall(List<Position> wallPositions, GameObject wallParent, GameObject roofParent)
	{
		foreach (Position wallPosition in wallPositions)
		{
			GameObject hallWall = (GameObject)Instantiate(wall);
			Vector3 wallTransPos = new Vector3(wallPosition.getX() * 2, 2, wallPosition.getY() * 2);
			hallWall.name = wallParent.name + "_" + wallPosition.getX() + "_" + wallPosition.getY();
			hallWall.transform.position = wallTransPos;
			hallWall.transform.SetParent(wallParent.transform);

			//Create Roof over wall to avoid light passing through the edges
			GameObject roofObj = (GameObject)Instantiate(roof);
			roofObj.name = roofParent.name + "_" + wallPosition.getX() + "_" + wallPosition.getY();
			roofObj.transform.position = new Vector3(wallPosition.getX() * 2, 4, wallPosition.getY() * 2);
			roofObj.transform.SetParent(roofParent.transform);
		}
	}

	/**
	 * Rotates an Object towards the first found neighboring floor object
	 */
	private Quaternion rotateToFloor(GameObject obj,Position objPosition, List<Position> floor)
	{
		foreach(Position neighbor in getNeighbors(objPosition))
		{
			if (floor.Contains(neighbor))
			{
				Transform objTransPos = obj.transform;
				Vector3 neighboorTransPos = new Vector3(neighbor.getX(), 0, neighbor.getY());
				Vector3 targetDir = neighboorTransPos - objTransPos.position;
				Vector3 newDirection = Vector3.RotateTowards(obj.transform.forward, targetDir, 360f, 360f);
				return Quaternion.LookRotation(newDirection);
			}
		}
		return new Quaternion();
	}

	/**
	 * Gets the vertical and horizontal neighboring Objects
	 */
	private List<Position> getNeighbors(Position pos)
	{
		List<Position> positions = new List<Position>();
		for(int x = pos.getX() - 1; x <= pos.getX()+1; x++)
		{
			for(int y = pos.getY() - 1; y <= pos.getY() + 1; y++)
			{
				if(!pos.Equals(new Position(x,y)) && (x == pos.getX() || y == pos.getY()))
				{
					positions.Add(new Position(x, y));
				}
			}
		}
		return positions;
	}
}
