using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Count
	{
		//Minimum value for the Count class
		public int minimum; 
		//Maximum value for the Count class
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	//Number of columns on the map
	public int columns = 8;

	//Number of rows on the map
	public int rows = 8;

	//Limit the number of walls per level
	public Count wallCount = new Count (5, 9);

	//Limit the number of food spawns per level
	public Count foodCount = new Count (1,5);

	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;

	//Lists possible locations for tile spawns
	private List <Vector3> gridPositions = new List<Vector3> ();

	//Clear and prepare to generate a new map
	void InitialiseList()
	{
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) 
		{
			for (int y = 1; y < rows - 1; y++) 
			{
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	//Setup the outer walls and floor on the map
	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}
	}

	//Return a random position fron the list of gridPositions
	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	//Initialises the level and calls the previous functions
	public void SetupScene(int level)
	{
		//Create the outer walls and floor
		BoardSetup ();

		//Resets the list of gridPositions
		InitialiseList ();

		//Spawns the wall and food tiles at random positions
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

		//Determines the number of enemies based on the level
		int enemyCount = (int)Mathf.Log (level, 2f);

		//Spawns the enemies at random positions
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

		//Spawns the exit tile in the top right corner of the map
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0F), Quaternion.identity);
	}
}
