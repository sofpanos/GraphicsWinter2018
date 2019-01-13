using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Factories;
using Core.Utility;
using System;
using UnityEngine.UI;
using Core.Constructions;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour {
	//Game Map
	public GameMap map;
	//Game Components
	public GameObject exitSwitch;
	public GameObject exit;
    public GameObject torchSwitch;
    public GameObject torch;
	public GameObject roof;
    public GameObject wall;
    public GameObject floor;
    public GameObject player;
	//Game Properties
	public static int level;
    public static int width = 200;
    public static int height = 100;
	private DateTime startTime;
	public static List<TimeSpan> LevelTimes = new List<TimeSpan>();
	public static int score = 0;
	//Helper Properties
	private List<Position> worldWallPositions;
	// Use this for initialization
	void Start () {
		startNextLevel();
		GameObject.Find("Time").GetComponent<TimeScript>().StartTime = DateTime.Now;	
	}
	public void startNextLevel()
	{
		if (level == 3)
		{
			float maxScoreMultiplier = (30 * 60) / (200 * 100);
			float maxScore = maxScoreMultiplier * width * height;
			TimeSpan total = new TimeSpan(0, 0, 0);
			foreach (TimeSpan levelTime in LevelTimes)
			{
				total += levelTime;
			}
			score = (int)(maxScore - total.TotalSeconds);
			SceneManager.LoadScene("ScoreScene");
		}
		else
		{
			level++;
			LevelTimes.Add(GameObject.Find("Time").GetComponent<TimeScript>().GetCurrentTime());
			ClearLevel();
			createLevel();
			GameObject.Find("Time").GetComponent<TimeScript>().StartTime = DateTime.Now;
		}
	}

	private void ClearLevel()
	{
		foreach(Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void createLevel()
	{
		if(width == 0 || height == 0)
		{
			width = 200;
			height = 100;
		}
		map = MapFactory.getNewGameMap(width, height, level);
		worldWallPositions = new List<Position>();
		createRooms();
		createHallways();
		worldWallPositions.Clear();
		worldWallPositions = null;
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
			worldWallPositions.Add(wallBlock.Key);

			switch (wallBlock.Value) {
				case BlockType.Exit:
					GameObject exitObject = (GameObject)Instantiate(exit);
					exitObject.name = "Exit";
					exitObject.transform.position = wallTransPos;
					rotateToFloor(exitObject,wallBlock.Key, floorPositions);
					exitObject.transform.SetParent(wallParent.transform);
					break;
				case BlockType.ExitSwitch:
					GameObject exitSwitchObj = (GameObject)Instantiate(exitSwitch);
					exitSwitchObj.name = "ExitSwitch";
					exitSwitchObj.transform.position = wallTransPos;
					rotateToFloor(exitSwitchObj, wallBlock.Key, floorPositions);
					exitSwitchObj.transform.SetParent(wallParent.transform);
					break;
                case BlockType.Switch:
                    GameObject torchSwitchObj = (GameObject)Instantiate(torchSwitch);
                    torchSwitchObj.name = "TorchSwitch";
                    torchSwitchObj.transform.position = wallTransPos;
                    rotateToFloor(torchSwitchObj, wallBlock.Key, floorPositions);
                    torchSwitchObj.transform.SetParent(wallParent.transform);
                    break;
                case BlockType.Light:
                    GameObject torchObj = (GameObject)Instantiate(torch);
                    torchObj.name = "Torch";
                    torchObj.transform.position = wallTransPos;
                    rotateToFloor(torchObj, wallBlock.Key, floorPositions);
                    torchObj.transform.SetParent(wallParent.transform);
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
			if (worldWallPositions.Contains(wallPosition))
			{
				continue;
			}
			
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
	private void rotateToFloor(GameObject obj,Position objPosition, List<Position> floor)
	{
		foreach(Position neighbor in getNeighbors(objPosition))
		{
			if (floor.Contains(neighbor))
			{
				if(neighbor.getX() == objPosition.getX() && neighbor.getY() == objPosition.getY() - 1)
				{
					obj.transform.Rotate(Vector3.up, 180f);
					break;
				}
				else if(neighbor.getY() == objPosition.getY())
				{
					if(neighbor.getX() == objPosition.getX() + 1)
					{
						obj.transform.Rotate(Vector3.up, 90f);
						break;
					}
					else if(neighbor.getX() == objPosition.getX() - 1)
					{
						obj.transform.Rotate(Vector3.up, -90f);
						break;
					}
				}
			}
		}
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
