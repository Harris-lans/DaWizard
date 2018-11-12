using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell 
{

	[SerializeField]
	private float mSpellProjectileSpeed = 300;
	[SerializeField]
	private GameObject mFireBall; 
	[SerializeField]
	private FireSound mAudioComponent;

	override internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin)
	{
		GameObject fireBall = Instantiate(mFireBall, pointOfOrigin.position, pointOfOrigin.rotation);
		fireBall.GetComponent<Rigidbody>().AddForce(fireBall.transform.forward * mSpellProjectileSpeed);

		// Incrementing the used variable
		++mNumberOfTimesUsed;

		//Playing the sound effect
		if(mNumberOfTimesUsed == mMaximuimNumberOfUses)
		{
			mAudioComponent.PlaySound(true);
		} else if (mNumberOfTimesUsed < mMaximuimNumberOfUses)
		{
			mAudioComponent.PlaySound(false);
		}
	}
}
