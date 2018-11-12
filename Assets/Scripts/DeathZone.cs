using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour 
{
	[SerializeField]
	private float mDamagePenaltyWhenPlayerFallsDown = 25;

	void OnCollisionEnter(Collision other)
	{
		PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
		if(player != null)
		{
			player.DealDamage(mDamagePenaltyWhenPlayerFallsDown);
		}
	}
}
