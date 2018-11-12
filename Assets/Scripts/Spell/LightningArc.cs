using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArc : MonoBehaviour 
{
	private Transform mFromElectricField;
	private Transform mToElectricField; 

	// Update is called once per frame
	void Update () 
	{
		if (mFromElectricField != null && mToElectricField != null)
		{
			// Placing the arc in between the two points
			transform.position = (mToElectricField.position - mFromElectricField.position) / 2 + mFromElectricField.position;

			// Scaling the lightning arc
			Vector3 currentScale = transform.localScale;
			currentScale.y = Vector3.Magnitude(mToElectricField.position - mFromElectricField.position) / 2; 
			transform.localScale = currentScale;

			// Rotating the lightning arc
			transform.rotation = Quaternion.FromToRotation(Vector3.up, (mToElectricField.position - mFromElectricField.position)); 
		}
		else 
		{
			Destroy(gameObject);
		}
	}

	internal void Initialize(Transform fromElectricField, Transform toElectricField, PlayerController spellOwner)
	{
		// Used to initialize the Lightning Arc
		mFromElectricField = fromElectricField;
		mToElectricField = toElectricField;

		//Changing materials in particlerenderer
        GetComponentInChildren<ParticleSystemRenderer>().material = spellOwner.ParticleMaterial;
        GetComponentInChildren<ParticleSystemRenderer>().trailMaterial= spellOwner.RibbonMaterial;
	}
}
