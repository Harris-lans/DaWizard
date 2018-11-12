using UnityEngine;

public class Fireball : MonoBehaviour 
{
	[SerializeField]
	private GameObject mExplosionPrefab;

	[SerializeField]
	private float mDamage = 10f;

	private GameObject mSelf;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerHealth otherPH = other.GetComponent<PlayerHealth>();
			otherPH.DealDamage(mDamage);
		}
		Instantiate(mExplosionPrefab,transform.position,transform.rotation);
		Destroy(gameObject);
	}
}
