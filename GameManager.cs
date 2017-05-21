using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	//Time to wait before starting the next level
	public float levelStartDelay = 2f;
	//Time delay between each player turn
	public float turnDelay = .1f;
	//Starting value of Player's food score
	public int playerFoodPoints = 100;
	public static GameManager instance = null;
	public BoardManager boardScript;

	//Check if it's the players turn
	[HideInInspector] public bool playersTurn = true;

	//Display text for current level
	private Text levelText;
	//Background image for levelText
	private GameObject levelImage;
	//Current level
	private int level = 0;
	//List of all enemies
	private List<Enemy> enemies;
	//Check if enemies are moving
	private bool enemiesMoving;
	//Check if map is being setup
	private bool doingSetup;

	void Awake () 
	{
		//Check if instance already exists
		if (instance == null)
			//if not, set instance to this
			instance = this;
		//If instance already exists and it's not this:::
		else if (instance != this)
			//Then destroy this
			Destroy (gameObject);
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);

		//Assign enemies to a new List of Enemy objects
		enemies = new List<Enemy> ();

		boardScript = GetComponent<BoardManager> ();

	}

	//This is called each time a scene is loaded
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//Add one to the level number
		level++;
		//Call InitGame to initialise the level
		InitGame();
	}

	void OnEnable()
	{
		//Tells the 'OnLevelFinishedLoading' function to start listening for a scene change event
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tells the 'OnLevelFinishedLoading' function to stop listening for a scene change event
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	//Initialises the game for each level.
	void InitGame()
	{
		//While doingSetup is true the player can't move
		doingSetup = true;

		//Get a reference to our image LevelImage by finding it by name
		levelImage = GameObject.Find("LevelImage");

		//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent
		levelText = GameObject.Find("LevelText").GetComponent<Text>();

		//Set the text of levelText to the string "Day" and append the current level number
		levelText.text = "Day " + level;

		//Set levelImage to active blocking player's view of the game board during setup
		levelImage.SetActive(true);

		//Call the HideLevelImage function with a delay in seconds of levelStartDelay
		Invoke("HideLevelImage", levelStartDelay);

		//Clear any Enemy objects in our List to prepare for next level
		enemies.Clear();

		//Call the SetupScene function of the BoardManager script
		boardScript.SetupScene(level);
	}

	//Hides black image between levels
	private void HideLevelImage()
	{
		//Disable the levelImage gameObject
		levelImage.SetActive(false);
		//Set doingSetup to false allowing player to move
		doingSetup = false;
	}

	//Update is called once per frame
	void Update () 
	{
		//Check that playersTurn or enemiesMoving or doingSetup are not currently true
		if(playersTurn || enemiesMoving || doingSetup)
			//If any of these are true, return and do not start MoveEnemies
			return;
		//Start moving enemies
		StartCoroutine (MoveEnemies ());
	}

	//Call GameOver when the player's food reaches 0
	public void GameOver()
	{
		//Display number of levels passed and game over message
		levelText.text = "After " + level + " days, you starved!";
		//Display black background image
		levelImage.SetActive (true);
		//Disable this GameManager
		enabled = false;
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) 
		{
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies [i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
