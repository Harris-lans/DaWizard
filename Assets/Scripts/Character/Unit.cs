using System;
using System.Collections;
using UnityEngine;

public abstract class Unit : MonoBehaviour 
{
	protected float TeleportCooldown = 0.5f;
	internal bool TeleportReady = true;
	
	private bool CooldownStarted = false;

	private void Update()
	{
		if(TeleportReady == false && CooldownStarted == false)
		{
			StartCoroutine(Cooldown());
			CooldownStarted = true;
		}
		UnitUpdate();
	}

    protected abstract void UnitUpdate();

	IEnumerator Cooldown()
	{
		yield return new WaitForSeconds(TeleportCooldown);
		TeleportReady = true;
		CooldownStarted = false;
	}


}
