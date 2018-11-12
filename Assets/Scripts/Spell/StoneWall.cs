using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : MonoBehaviour {

	[SerializeField]
	private float mSpeed = 10;
	[SerializeField]
	private float mLifeSpan = 4;

	private float mHeight;
	private float mAddedHeight;

	private void Awake()
	{
		//Get the half height of the wall
		mHeight= (GetComponent<MeshFilter>().sharedMesh.bounds.extents.y) * transform.lossyScale.y;
		//Move the wall just below the surface of the level
		transform.position = new Vector3 (transform.position.x, 0-mHeight, transform.position.z);
	}
	
	internal void Raise()
	{
		StartCoroutine(WallMovement());
		Invoke("Lower", mLifeSpan);
	}

	internal void Lower()
	{
		StartCoroutine(WallReverseMovement());
	}

	IEnumerator WallMovement()
	{
		while(mAddedHeight < mHeight)
		{
			float HeightToAdd = Time.deltaTime * mSpeed;
			transform.position = new Vector3 (transform.position.x, transform.position.y + HeightToAdd, transform.position.z);
			mAddedHeight += HeightToAdd;
			yield return null;
		}
	}

	IEnumerator WallReverseMovement()
	{
		while(mAddedHeight > -mHeight)
		{
			float heightToSubtract = Time.deltaTime * mSpeed;
			transform.position = new Vector3(transform.position.x, transform.position.y - heightToSubtract, transform.position.z);
			mAddedHeight -= heightToSubtract;
			yield return null;
		}
		Destroy(gameObject);
	}
}
