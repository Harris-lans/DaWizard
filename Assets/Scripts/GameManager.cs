using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInputDotNetPure;

public class GameManager : MonoBehaviour 
{
	public static bool GameStarted;

	#region Global Variables

		[Header("Player Spawning Information")]
		[SerializeField]
		private List<PlayerController> mPlayerControllerPrefabs;
		[SerializeField]
		float mTimeDelayBeforePlayerSpawn = 3;
		[SerializeField]
		float mGameStartTimer = 3;
		[SerializeField]
		float mPlayerInvincibiltyAfterRespawn = 3.0f;
		[SerializeField]
		private float mTimeBeforeRespawn = 5;

		[Space]
		[Header("UI Information")]
		[SerializeField]
		private GameObject mGameOverCanvas;
		[SerializeField]
		private GameObject mPauseScreenCanvas;
		[SerializeField]
		private Canvas mPlayerHUDCanvas;

		[Header("Announcer Audio and Music")]
		[SerializeField]
		private GameObject[] mEliminationSounds;
		[SerializeField]
		private AudioClip mVictoryMusic;
		[SerializeField]
		private AudioSource mMusicSource;

		private int mNumberOfPlayers;
		private List<PlayerIndex> mAvailablePlayerIndices;
		private List<GameObject> mSpawnPoints;
		private List<GameObject> mSpawnPointsToUse;
		private bool mGameOver = false;
		private bool mGameOverActive;


	#endregion
	
	#region Unity Functions

	void Awake()
	{
		// Resetting the static lists of the player-controller class
		PlayerController.Players = new List<PlayerController>();
		PlayerController.ActivePlayers = new List<PlayerController>();
		PlayerHUD.PlayerHUDList = new List<PlayerHUD>();

		// Hiding the game over screen in case it was active
		if (mGameOverCanvas != null)
		{
			mGameOverCanvas.SetActive(false);
			mGameOverActive = false;
		}

		// Checking for available controllers
		mAvailablePlayerIndices = GetAvailableControllers();

		// Debugging Code
		print("Number of controllers connected: " + mAvailablePlayerIndices.Count);

		// Finding the spawn points in the scene
		mSpawnPoints = GetAvailableSpawnPoints();
		mSpawnPointsToUse = mSpawnPoints;
		InitializeGame();

		// Initializing Variables
		GameStarted = false;
	}

	void Update () 
	{
		// Checking if a player has disconnected
		foreach (PlayerController player in PlayerController.Players)
		{
			GamePadState testState = GamePad.GetState(player.PlayerIndex);
			if (!testState.IsConnected)
			{
				mPauseScreenCanvas.GetComponent<PauseMenu>().PauseGame();
			}
		}

		// Checking if the game is over
		if(PlayerController.Players.Count == 1 && GameStarted)
		{
			//Announcing the player who has won the match
			mGameOver = true;
			if(mGameOverCanvas != null && mGameOverActive == false)
			{
				mPlayerHUDCanvas.enabled = false;
				mGameOverCanvas.SetActive(true);
				mGameOverActive = true;
				mMusicSource.clip = mVictoryMusic;
				mMusicSource.Play();
				mGameOverCanvas.GetComponentInChildren<GameOverMenu>().AnnounceWinner((int)PlayerController.Players[0].PlayerIndex);
			}
		}
	}

	#endregion

	#region Class Functions

		void InitializeGame()
		{
			StartCoroutine(SpawnInPlayers());
		}

		List<GameObject> GetAvailableSpawnPoints()
		{
			return GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();
		}

		List<PlayerIndex> GetAvailableControllers()
		{
			List<PlayerIndex> availablePlayerIndices = new List<PlayerIndex>();
			for (int i = 0; i < 4; i++)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					availablePlayerIndices.Add(testPlayerIndex);
				}
			}
			return availablePlayerIndices;
		}

		private void DeathAnnouncer(int PlayerNumber)
		{
			// Not Playing the elimination sound if the player is the last player to be eliminated
			if(PlayerController.Players.Count > 2)
			{
				//Instantiate empty game object that plays the elimination sound
				Instantiate(mEliminationSounds[PlayerNumber], transform.position, transform.rotation);
			}
		}

		private void RespawnPlayer(PlayerController playerToRespawn)
		{
			// Disabling the object for some Time
			if (playerToRespawn != null)
			{
				// Checking if the player has died after falling down
				playerToRespawn.InvokeRespawnEvent(mTimeBeforeRespawn);
				StartCoroutine(RespawnTimer(playerToRespawn));
			}
		}

		private Transform GetMostOptimalSpawnPoint()
		{
			Transform chosenSpawnPoint = null;
			float largest_SmallestDistanceFromPlayerToSpawnPoint = 0;

			foreach(GameObject spawnPoint in mSpawnPoints)
			{
				float smallestDistanceFromPlayerToSpawnPoint = Mathf.Infinity;
				Vector3 spawnPointPosition = spawnPoint.transform.position;

				foreach(PlayerController player in PlayerController.ActivePlayers)
				{
					float distanceFromSpawnPointToPlayer = Vector3.Distance(spawnPointPosition, player.transform.position);

					// Finding the minimum distance between the spawn point and all the active players
					if (distanceFromSpawnPointToPlayer < smallestDistanceFromPlayerToSpawnPoint)
					{
						smallestDistanceFromPlayerToSpawnPoint = distanceFromSpawnPointToPlayer;
					}
				}

				// Checking if this is the maximum closest distance
				if (smallestDistanceFromPlayerToSpawnPoint > largest_SmallestDistanceFromPlayerToSpawnPoint)
				{
					largest_SmallestDistanceFromPlayerToSpawnPoint = smallestDistanceFromPlayerToSpawnPoint;
					chosenSpawnPoint = spawnPoint.transform;
				}
			}

			return chosenSpawnPoint;
		}

	#endregion

	#region  Properties

		bool IsGameOver
		{
			get
			{
				return mGameOver;
			}
		}

	#endregion

	#region Co-Routines

		IEnumerator RespawnTimer(PlayerController playerToRespawn)
		{
			yield return new WaitForSeconds(0.1f);

			if(playerToRespawn == null || playerToRespawn.gameObject == null)
			{
				yield break;
			}


			playerToRespawn.gameObject.SetActive(false);
			
			// Respawning the player after a given amount of time
			yield return new WaitForSeconds(mTimeBeforeRespawn);

			Transform suitableSpawnPoint = GetMostOptimalSpawnPoint();
			playerToRespawn.gameObject.SetActive(true);
			playerToRespawn.transform.position = suitableSpawnPoint.position;
			playerToRespawn.GetComponent<PlayerHealth>().TurnOnInvincibilty(mPlayerInvincibiltyAfterRespawn);
		}

		IEnumerator SpawnInPlayers()
		{
			yield return new WaitForSeconds(2);

			for (int i = 0; i < mAvailablePlayerIndices.Count; i++)
			{
				// Selecting SpawnPoint
				int spawnPoint = Random.Range(0, mSpawnPointsToUse.Count);

				// Spawning a player in a random spawn point and not using the same spawn point again
				PlayerController player = Instantiate(mPlayerControllerPrefabs[i], mSpawnPointsToUse[spawnPoint].transform.position, mSpawnPointsToUse[spawnPoint].transform.rotation);
				player.Initialize(mAvailablePlayerIndices[i]);
				
				// Subscribing to the required events to know when to announce deaths and respawn players
				player.GetComponent<PlayerHealth>().subscribeToPlayerDeathEvent(DeathAnnouncer);
				player.GetComponent<GroundedCheck>().SubscribeToPlayerHitDeathZoneEvent(RespawnPlayer);
				
				// Ignoring used spawnpoints to spawn players in different directions 
				mSpawnPointsToUse.Remove(mSpawnPointsToUse[spawnPoint]);
				
				// Finding the correct HUD and activating it
				for (int j = 0; j < PlayerHUD.PlayerHUDList.Count; j++)
				{
					if (PlayerHUD.PlayerHUDList[j].LinkedToPlayer == i + 1)
					{
						PlayerHUD.PlayerHUDList[j].SpawnPlayerUI();
						break;
					}
				}

				// Waiting for sometime before spawning in the players
				yield return new WaitForSeconds(mTimeDelayBeforePlayerSpawn);
			}

			// Starting Timer
			GameUI gameUI = mPlayerHUDCanvas.GetComponent<GameUI>();
			if (gameUI != null)
			{
				gameUI.StartGameTimer(mGameStartTimer);
			}

			yield return new WaitForSeconds(mGameStartTimer);
			GameStarted = true;
		}

	#endregion
}
