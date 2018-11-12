using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour 
{
	[SerializeField]
	private float mLifeSpan = 4.0f;
	// Use this for initialization
	void Start () 
	{
		Destroy(gameObject, mLifeSpan);	
	}
	
}
