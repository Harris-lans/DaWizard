using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpell : Spell 
{
	[SerializeField]
	private GameObject mWallPrefab;

	override internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin)
	{
		Vector3 WallPosition = new Vector3 (pointOfOrigin.position.x, 0, pointOfOrigin.position.z);
		GameObject Wall = Instantiate(mWallPrefab, (WallPosition + pointOfOrigin.forward * 5), pointOfOrigin.rotation);
		Wall.GetComponentInChildren<StoneWall>().Raise();
		
		// Incrementing the used variable
		++mNumberOfTimesUsed;
		spellOwner.TauntSound();
	}
}
