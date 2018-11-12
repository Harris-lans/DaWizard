using System.Collections.Generic;
using UnityEngine;

public class IceCone : MonoBehaviour 
{
[SerializeField, Tooltip("Damage dealt per second")]
private float mDPS = 1f;

[SerializeField, Tooltip("Slow duration in seconds")]
private float mSlowDuration = 2f;

[SerializeField, Tooltip("0 is standstill, 1 is normal speed")]
private float mSlowPercentage = 0.6f;

private Vector3 mRaycastDirection;
private int mLayerMask = ~ (1 << 10);

private void OnTriggerStay(Collider other)
{
	//Only affects players
	if(other.CompareTag("Player"))
	{
		//Store the center of the player hit
		Vector3 TargetMid = new Vector3(other.transform.position.x, other.transform.position.y+1, other.transform.position.z);

		//Set the direction of the raycast
		mRaycastDirection = TargetMid - transform.position;

		//Raycast to check line of sight
		RaycastHit mRaycastHit = new RaycastHit();
		Debug.DrawRay(transform.position, mRaycastDirection, Color.green);
		if(Physics.Raycast(transform.position, mRaycastDirection, out mRaycastHit, 200f, mLayerMask))
		{
			//Check that raycast hit a player
			if(mRaycastHit.collider.gameObject.CompareTag("Player"))
			{
				PlayerController mEnemyPC = other.GetComponent<PlayerController>();
				PlayerHealth mEnemyPH = other.GetComponent<PlayerHealth>();
				//print("Hit " + mEnemyPC.name);
				
				mEnemyPC.Slow(mSlowPercentage, mSlowDuration);
				mEnemyPH.DealDamage(mDPS*Time.deltaTime);
				//print(mDPS*Time.deltaTime + " damage dealt");
			}
		}
	}
}





}
