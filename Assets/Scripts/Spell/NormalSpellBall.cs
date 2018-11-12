using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSpellBall : MonoBehaviour 
{
	[SerializeField]
	private float mDamage= 10f;
	private PlayerHealth mSpellOwner = null;

	internal void Initialize(PlayerHealth spellOwner)
	{
		mSpellOwner = spellOwner;
	}
	void OnTriggerEnter(Collider other)
	{
		PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
		if(player == mSpellOwner)
		{
			return;
		}
		// Deal damage if a player is hit
		if (player != null)
		{
			player.DealDamage(mDamage);
		}
		
		Destroy(gameObject);
		
	}
}
