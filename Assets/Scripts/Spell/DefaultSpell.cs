using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSpell : Spell 
{

	[SerializeField]
	private float mSpellProjectileSpeed = 300;
	[SerializeField]
	private GameObject mNormalSpell; 

	override internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin)
	{
		GameObject normalSpellBall = Instantiate(mNormalSpell, pointOfOrigin.position, pointOfOrigin.rotation);
		normalSpellBall.GetComponent<NormalSpellBall>().Initialize(spellOwner.GetComponent<PlayerHealth>());
		normalSpellBall.GetComponent<Rigidbody>().AddForce(normalSpellBall.transform.forward * mSpellProjectileSpeed);
	}
}