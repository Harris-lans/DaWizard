using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour 
{
	[SerializeField]
	private List<GameObject> mSpellList;

	[SerializeField]
	private float mRotationPerSecond = 90f;

	[SerializeField]
	private float mBobHeight = 1f;

    private GameObject mSpawner;
	private Vector3 mStartPosition;
	private float mSinTimer = 0;
	private Spell mSelectedSpell;

	private void Awake()
	{
		int spellChoice = Random.Range(0, mSpellList.Count);
		if (mSpellList.Count > 0)
		{
			mSelectedSpell = mSpellList[spellChoice].GetComponent<Spell>();
			GetComponent<MeshFilter>().mesh = mSelectedSpell.mSpellMesh;
			GetComponent<MeshRenderer>().materials = mSelectedSpell.mSpellMaterial;
			GetComponent<ParticleSystemRenderer>().material = mSelectedSpell.mSpellParticle;
		}
		mStartPosition = transform.position;
	}

	private void Update()
	{
		//Rotating the pickup
		float FrameRotation = Time.deltaTime * mRotationPerSecond;
		transform.Rotate(0,FrameRotation,0);

		//Bobbing movement
		float HeightChange = Mathf.Sin(mSinTimer) * mBobHeight;
		transform.position = new Vector3(mStartPosition.x, mStartPosition.y + HeightChange, mStartPosition.z);

		//Resetting the timer when the sin curve is complete
		if(mSinTimer >= 2*Mathf.PI)
		{
			mSinTimer = 0;
		} else
		{
			mSinTimer += Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.GetComponent<PlayerController>();

		// Checking if a player has stepped on the pickup
		if(player != null)
		{
			// Passing a new spell object to the player
			player.PickUpSpells(Instantiate(mSelectedSpell));

			player.PickupSound();
            PickUpSpawner pickUpSpawner = mSpawner.GetComponent<PickUpSpawner>();
			if (pickUpSpawner != null)
			{
				pickUpSpawner.PickupTaken();
			}
			Destroy(gameObject);
		}
	}

    internal void SetSpawner(GameObject Spawner)
    {
        mSpawner = Spawner;
    }
	
}
