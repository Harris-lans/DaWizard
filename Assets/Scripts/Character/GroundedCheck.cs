using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundedCheck : MonoBehaviour 
{
	[SerializeField]
	float mGroundCheckHeight = 5f;
	[SerializeField]
	float mDistanceBetweenPlayerAndRay = 3f;
	[SerializeField]
	bool mDebug = true;
	[SerializeField]
	private LayerMask mLayersToMask;


	private bool mGrounded;
	private bool mPlayedFallingSound;
	private RaycastHit hit;
	private UnityEvent playerFallingEvent;

	internal delegate void playerHitDeathZoneAction (PlayerController player);
	private event playerHitDeathZoneAction playerHitDeathZoneEvent;


	#region Unity Functions

		void Start()
		{
			playerFallingEvent = new UnityEvent();
		}

		void Update()
		{
			// Resetting the grounded every frame
			mGrounded = false;

			// A single raycast hit is enough to be grounded
			mGrounded |= Physics.Raycast(transform.position + mDistanceBetweenPlayerAndRay * (transform.right), transform.TransformDirection(Vector3.down), mGroundCheckHeight, mLayersToMask);
			mGrounded |= Physics.Raycast(transform.position + mDistanceBetweenPlayerAndRay * (-transform.right), transform.TransformDirection(Vector3.down), mGroundCheckHeight, mLayersToMask);
			mGrounded |= Physics.Raycast(transform.position + mDistanceBetweenPlayerAndRay * (transform.forward), transform.TransformDirection(Vector3.down), mGroundCheckHeight, mLayersToMask);
			mGrounded |= Physics.Raycast(transform.position + mDistanceBetweenPlayerAndRay * (-transform.forward), transform.TransformDirection(Vector3.down), mGroundCheckHeight, mLayersToMask);

			// Checking if the player is falling
			if (transform.position.y < -1)
			{

			}

			if (mDebug)
			{
				Debug.DrawRay(transform.position + mDistanceBetweenPlayerAndRay * (transform.forward), transform.TransformDirection(Vector3.down) * mGroundCheckHeight, Color.yellow);
				Debug.DrawRay(transform.position + mDistanceBetweenPlayerAndRay * (-transform.forward), transform.TransformDirection(Vector3.down) * mGroundCheckHeight, Color.yellow);
				Debug.DrawRay(transform.position + mDistanceBetweenPlayerAndRay * (transform.right), transform.TransformDirection(Vector3.down) * mGroundCheckHeight, Color.yellow);
				Debug.DrawRay(transform.position + mDistanceBetweenPlayerAndRay * (-transform.right), transform.TransformDirection(Vector3.down) * mGroundCheckHeight, Color.yellow);
			}
		}

		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.GetComponent<DeathZone>() != null)
			{
				PlayerController mPC = GetComponent<PlayerController>();
				//Disabling all the players attacks when they hit the death zone
				mPC.mDefaultAttack.CanCast = false;
				if(mPC.Spell1 != null) mPC.mSpell1.CanCast = false;
				if(mPC.mSpell2 != null) mPC.mSpell2.CanCast = false;
				// Telling all the listeners that the respective player has hit the death zone
				playerHitDeathZoneEvent.Invoke(mPC);
			}
		}

	#endregion

	#region  Class Functions

		internal void SubscribeToPlayerFallingEvent(UnityAction action)
		{
			playerFallingEvent.AddListener(action);
		}

		internal void SubscribeToPlayerHitDeathZoneEvent(playerHitDeathZoneAction action)
		{
			playerHitDeathZoneEvent += action;
		}

	#endregion

	internal bool IsGrounded
	{
		get
		{
			return mGrounded;
		}
	}
}
