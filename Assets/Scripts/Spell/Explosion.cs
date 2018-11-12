using UnityEngine;

public class Explosion : MonoBehaviour 
{
	[SerializeField]
	private float mDamage = 10f;

	[SerializeField, Tooltip("Which layer to damage")]
	private LayerMask mLayermask;

	private Vector3 mRaycastTarget;
	private Vector3 mCenter;
	private	float mRadius = 3.4f;

	private void Start()
	{
			mCenter = transform.position;
 
      		Collider[] OverlappingObjects = Physics.OverlapSphere(mCenter, mRadius, mLayermask);
			
			foreach (Collider OverlappedObject in OverlappingObjects)
			{
				if(OverlappedObject.CompareTag("Player"))
				{
					mRaycastTarget = OverlappedObject.transform.position - transform.position;

					//Raycast to check line of sight
					RaycastHit mRaycastHit = new RaycastHit();
					Debug.DrawRay(transform.position, mRaycastTarget, Color.green);
					if(Physics.Raycast(transform.position, mRaycastTarget, out mRaycastHit))
					{
						//Check that raycast hit a player
						if(mRaycastHit.collider.gameObject.CompareTag("Player"))
						{
							PlayerHealth mEnemyPH = OverlappedObject.GetComponent<PlayerHealth>();
							//DEBUG:	print("Hit " +	mRaycastHit.collider.gameObject);
							mEnemyPH.DealDamage(mDamage);
						}
					}
				}
			}
	}
}
