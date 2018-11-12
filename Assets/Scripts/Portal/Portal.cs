using UnityEngine;

public class Portal : MonoBehaviour 
{
	#region ClassVariables

		[SerializeField]
		private GameObject Target;

	#endregion ClassVariables

	#region  LifeCycle
		private void OnTriggerEnter(Collider other)
		{
			Unit u  = other.GetComponent<Unit>();
			AudioSource aTelePorterSound = GetComponent<AudioSource>();
			//Exits the function if teleporting is on cooldown
			if (u != null && u.TeleportReady == true) 
			{
				//Teleporting the object
				Vector3 newPos = Target.transform.position;
				aTelePorterSound.Play();
				newPos.y = other.transform.position.y;
				other.transform.position =  newPos;
				u.TeleportReady = false;
			}
		}
	#endregion LifeCycle
}
