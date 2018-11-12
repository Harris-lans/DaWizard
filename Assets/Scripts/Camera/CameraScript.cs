using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{
	#region Global Variables

		[SerializeField]
		private float mTopOffset = 10;
		[SerializeField]
		private float mSidesOffset = 5;
		[SerializeField]
		private float mZoomingSpeed = 10.0f;
		[SerializeField]
		private float mMaximumZoomDistance = 70.0f;
		[SerializeField]
		private float mMinimumZoomDistance = 17.0f;


		private Camera mCamera;
		private float mScreenSizeX;
		private float mScreenSizeY;
		private List<Vector3> mPlayerPositions;
		private int mPlayersInCenter;

	#endregion

	#region Unity Functions

		// Use this for initialization
		void Start () 
		{
			mCamera = Camera.main;
			mScreenSizeX = Screen.currentResolution.width;
			mScreenSizeY = Screen.currentResolution.height;
		}
		
		// Update is called once per frame
		void LateUpdate () 
		{
			if (PlayerController.ActivePlayers.Count <= 0)
			{
				if (transform.localPosition.y < 70)
				{
					transform.localPosition += new Vector3(0, 0.01f, 0); 
				}
				return;
			}

			mPlayersInCenter = 0;

			foreach(PlayerController player in PlayerController.ActivePlayers)
			{
				Vector3 playerPosition = mCamera.WorldToScreenPoint(player.transform.position);
				if (transform.position.y < mMinimumZoomDistance)
				{
					// Checking if there are players out of bounds on the top and bottom
					if(((playerPosition.y < (((mTopOffset + 5) / 100.0f ) * mScreenSizeY)) || (playerPosition.y > (((100 - mTopOffset)) / 100.0f ) * mScreenSizeY)))
					{
						ZoomOut(Time.deltaTime * mZoomingSpeed);
					}

					// Checking if there are players out of bounds on the sides
					if(((playerPosition.x < ((mSidesOffset / 100.0f ) * mScreenSizeX)) || (playerPosition.y > (((100 - mSidesOffset) / 100.0f ) * mScreenSizeY))))
					{
						ZoomOut(Time.deltaTime * mZoomingSpeed);
					}
				}

				//Zoom in
				if (transform.position.y > mMaximumZoomDistance)
				{
					//Check if player is in the center 40% on both axes
					if ((playerPosition.y > (0.3f * mScreenSizeY)) && (playerPosition.y < (0.7f * mScreenSizeY)) && (playerPosition.x > (0.3f * mScreenSizeX)) && (playerPosition.x < (0.7f * mScreenSizeX)))
					{
						mPlayersInCenter++;
					}
				}
			}

			//If all players are in the center, zoom in
			if(mPlayersInCenter == PlayerController.ActivePlayers.Count)
			{
				ZoomIn(Time.deltaTime * mZoomingSpeed);
			}
		}

	#endregion
	
	#region Class Functions
	
		private void ZoomOut(float distance)
		{
			transform.position -= transform.forward * distance;
		}

		private void ZoomIn(float distance)
		{
			transform.position += transform.forward * distance;
		}

	#endregion
}
