using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public abstract class Spell : MonoBehaviour 
{
	[SerializeField]
	protected float mDelayBetweenSpells;
	[SerializeField]
	protected int mMaximuimNumberOfUses = 6;
	[SerializeField]
	internal Mesh mSpellMesh;
	[SerializeField]
	internal Material[] mSpellMaterial;
	[SerializeField]
	internal Material mSpellParticle;
	[SerializeField]
	internal Sprite mSpellSplashScreenIcon;

	protected int mNumberOfTimesUsed;
	protected Rigidbody mRigidBody;
	protected bool mCanCast;
	
	// Use this for initialization
	void Awake () 
	{
		mRigidBody = GetComponent<Rigidbody>();
		mNumberOfTimesUsed = 0;
		mCanCast = true;
	}

	#region Properties

	    internal virtual float DelayBetweenSpells
		{
			get
			{
				return mDelayBetweenSpells;
			}
		}
        
    	internal virtual bool CanCast
    	{
    		get
    		{
    			return this.mCanCast;
    		}
            set 
    		{
			    this.mCanCast = value;
    		}
    	}

		internal virtual int UsesLeft
		{
			get
			{
				return mMaximuimNumberOfUses - mNumberOfTimesUsed;
			}
		}

	#endregion

	#region Class Functions

    	abstract internal void CastSpell(PlayerController spellOwner, Transform pointOfOrigin);

    	internal bool CanBeUsed()
    	{
    		return (mNumberOfTimesUsed < mMaximuimNumberOfUses);
    	} 

	#endregion

}
