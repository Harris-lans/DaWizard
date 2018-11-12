using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : Spell 
{
	[SerializeField]
	private GameObject mIceCone; 

	override internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin)
	{
		//Rotate the cone by -90 on the x axis
		Vector3 rot = pointOfOrigin.rotation.eulerAngles;
 		rot = new Vector3(-90,rot.y,rot.z);
 		pointOfOrigin.rotation = Quaternion.Euler(rot);

		GameObject mSpell = Instantiate(mIceCone, pointOfOrigin.position, pointOfOrigin.rotation);
		mSpell.transform.SetParent(spellOwner.transform);
		// Incrementing the used variable
		++mNumberOfTimesUsed;
	}
}
