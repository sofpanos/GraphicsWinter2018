﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Factories;
using Core.Utility;
using Core.Constructions;

public class Initializer : MonoBehaviour {
    public GameMap map;
    public GameObject wall;
    public GameObject floor;
    public GameObject player;
    public int level;
    public int width;
    public int height;
	// Use this for initialization
	void Start () {
        map = MapFactory.getNewGameMap(500, 500, 1);
        bool first = true;
        int id = 0;
        foreach (Room room in map.getRooms())
        {
            GameObject roomObject = new GameObject();
            roomObject.transform.SetParent(((GameObject)GameObject.Find("Game")).transform);
            if (first)
            {
                Position playerPosition = room.getFloorPositions()[Random.Range(0, room.getFloorPositions().Count)];
                GameObject newPlayer =(GameObject)Instantiate(player);
                newPlayer.transform.position = new Vector3(playerPosition.getX() * 2, 2, playerPosition.getY() * 2);
                newPlayer.transform.SetParent(((GameObject)GameObject.Find("Game")).transform);
            }
            foreach(Position wallPosition in room.getWallPositions()){
                GameObject roomWall = (GameObject)Instantiate(wall);
                Vector3 wallTransPos = new Vector3(wallPosition.getX() * 2, 2, wallPosition.getY() * 2 );
                roomWall.transform.position = wallTransPos;
                roomWall.transform.SetParent(roomObject.transform);
            }
            foreach (Position floorPosition in room.getFloorPositions())
            {
                GameObject roomFloor = (GameObject)Instantiate(floor);
                Vector3 floorTransPos = new Vector3(floorPosition.getX() * 2, 0, floorPosition.getY() * 2);
                roomFloor.transform.position = floorTransPos;
                roomFloor.transform.SetParent(roomObject.transform);
            }
        }

        foreach (Hallway hall in map.getHallways())
        {
            GameObject hallObject = new GameObject();
            hallObject.transform.SetParent((GameObject.Find("Game")).transform);
            foreach (Position wallPosition in hall.getWallPositions())
            {
                GameObject hallWall = (GameObject)Instantiate(wall);
                Vector3 wallTransPos = new Vector3(wallPosition.getX() * 2, 2, wallPosition.getY());
                hallWall.transform.position = wallTransPos;
                hallWall.transform.SetParent(hallObject.transform);
            }

            foreach(Position floorPosition in hall.getPath())
            {
                GameObject hallFloor = (GameObject)Instantiate(floor);
                Vector3 floorTransPos = new Vector3(floorPosition.getX() * 2, 0, floorPosition.getY() * 2);
                hallFloor.transform.position = floorTransPos;
                hallFloor.transform.SetParent(hallObject.transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
