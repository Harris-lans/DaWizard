using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour 
{
	private GameObject[] mPlayers;
	private bool mNoPlayers;
	private Vector3 mAveragePositionOfPlayers;


	// Update is called once per frame
	void Update () 
	{
		// Checking if there are any players in the scene to avoid null references
		if (PlayerController.ActivePlayers.Count > 0)
		{
			// Calculating the average position between all the players
			mAveragePositionOfPlayers = GetAveragePositionOfPlayers();
			transform.position = Vector3.Lerp(transform.position, mAveragePositionOfPlayers, 0.1f);
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, Vector3.zero, 0.05f);
		}
	}

	private Vector3 GetAveragePositionOfPlayers()
	{
		Vector3 sumOfPositions = Vector3.zero;
		for(int i = 0; i < PlayerController.ActivePlayers.Count; i++)
		{
			sumOfPositions += PlayerController.ActivePlayers[i].transform.position;
		}
		sumOfPositions = sumOfPositions / PlayerController.ActivePlayers.Count;
		return sumOfPositions;
	}

	internal float GetDistanceOfTheFarthestPlayer()
	{
		float farthestDistance = 0;
		foreach(PlayerController player in PlayerController.ActivePlayers)
		{
			float distance = Vector3.Magnitude(player.transform.position - transform.position);
			if (distance > farthestDistance)
			{
				farthestDistance = distance;
			}
		}
		return farthestDistance;
	} 
}
