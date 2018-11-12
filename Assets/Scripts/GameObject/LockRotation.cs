using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour 
{

	[SerializeField]
	private bool lockXRotation = false;
	[SerializeField]
	private bool lockYRotation = false;
	[SerializeField]
	private bool lockZRotation = false;

	private float defaultX;
	private float defaultY;
	private float defaultZ;


	// Use this for initialization
	void Start () 
	{
		defaultX = transform.rotation.eulerAngles.x;
		defaultY = transform.rotation.eulerAngles.y;
		defaultZ = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x  = transform.rotation.eulerAngles.x;
		if(lockXRotation)
		{
			x = defaultX;
		}
		float y  = transform.rotation.eulerAngles.y;
		if(lockYRotation)
		{
			y = defaultY;
		}
		float z  = transform.rotation.eulerAngles.z;
		if(lockZRotation)
		{
			z = defaultZ;
		}
		transform.rotation = Quaternion.Euler(x, y, z);	
	}
}
