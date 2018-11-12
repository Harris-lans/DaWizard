using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
	[SerializeField]
	private ElectricField mElectricFieldPrefab;
	[SerializeField, Tooltip("Delay Before removing the origin Electric Field")]
	private float mDelayBeforeRemovingElectricField = 1.0f;
	[SerializeField]
	[Tooltip("Number of objects the lightning spell conducts to")]
	private int mMaxNumberOfConductions = 3;

	// Stores the references to the lightning fields that have been generated
	private List<ElectricField> mElectricFieldsGenerated = new List<ElectricField>();

	#region Class Functions

    	override internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin)
    	{
    		// Spawning the first electric field
    		ElectricField newElectricField = Instantiate(mElectricFieldPrefab, pointOfOrigin.position, pointOfOrigin.rotation);
			newElectricField.transform.parent = spellOwner.transform;
    		newElectricField.Initialize(spellOwner, this, mMaxNumberOfConductions, null);
			++mNumberOfTimesUsed;

			// Destroying the first Electric Field
			Destroy(newElectricField.gameObject, mDelayBeforeRemovingElectricField);
    	}

    	internal void RegisterField(ElectricField newField)
    	{
    		mElectricFieldsGenerated.Add(newField);
    	}
    	internal void RemoveField(ElectricField fieldToDelete)
    	{
			mElectricFieldsGenerated.Remove(fieldToDelete);
    	}

	#endregion

	#region Properties
    	internal override bool CanCast
    	{
    		get
    		{
			    // Will return true only if timer is done and all the electric fields have been destroyed
			    return base.CanCast && (mElectricFieldsGenerated.Count <= 0);
    		}

    		set
    		{
			    base.CanCast = value;
    		}
    	}
	#endregion
}

// IConductive Interface for seeing if an object is conductive
interface IConductive
{
	// Not Using it now currently
}