using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
	#region Global Variables

		[SerializeField]
		private float mMaxHealth = 100;

		[SerializeField]
		private HitNumbers _HitNumberText;

		[SerializeField]
		private Text _HPPercentage;

		private float mHealth;

		private bool mInvincible;


		private PlayerController mPC;

		private GroundedCheck mGroundedCheck;

		private int mPlayerNumber;

		#region Events 

			private UnityEvent playerDamageEvent;

			internal delegate void PlayerDeathAction(int playerNumber);

			private event PlayerDeathAction playerDeathEvent;

		#endregion

		private Coroutine mInvinciblityCouroutine;

	#endregion

	#region Unity Functions
	void Awake()
	{
		mHealth = mMaxHealth;
		playerDamageEvent = new UnityEvent();
		mPC = GetComponent<PlayerController>();
		mGroundedCheck = GetComponent<GroundedCheck>();
		mInvincible = false;
	}

	private void Update()
	{
		_HPPercentage.text = Mathf.CeilToInt(mHealth) + "%";
	}
	#endregion 
	
	#region Class Functions

		internal void DealDamage(float damage)
		{
			// Not dealing damage if the player is invincible and on ground => He still takes damage if he is invincible and falls off
			if (mInvincible && mGroundedCheck.IsGrounded)
			{
				return;
			}

			mPlayerNumber = (int)mPC.PlayerIndex;
			mHealth -= damage;
			
			if(_HitNumberText!= null) _HitNumberText.SetText(damage);
			
			playerDamageEvent.Invoke();
			
			// Checking if the health is 0 and destroying the player
			if (mHealth <= 0)
			{
				playerDeathEvent.Invoke(mPlayerNumber);
				mPC.Death();
			} else
			{
				if(damage>2) mPC.HurtSound(); //Avoids playing hurtsound at every tick of the ice and lightning spell
			}
		}

		internal void subscribeToPlayerDamageEvent(UnityAction action)
		{
			playerDamageEvent.AddListener(action);
		}

		internal void subscribeToPlayerDeathEvent (PlayerDeathAction action)
		{
			playerDeathEvent += action;
		}

		internal float PlayerHealthFraction
		{
			get
			{
				return mHealth / mMaxHealth;
			}
		}

		internal void TurnOnInvincibilty(float invincibilityTime)
		{
			// Stopping the co-routine before running the new timer
			if (mInvinciblityCouroutine != null)
			{
				StopCoroutine(mInvinciblityCouroutine);
			}
			mInvinciblityCouroutine = StartCoroutine(InvincibilityTimer(invincibilityTime));
			
			// Letting the player know that he is invincible
			if(_HitNumberText!= null) 
			{
				_HitNumberText.SetText("Invincible", invincibilityTime);
			}	
		}

	#endregion
	
	#region Co-Routines
		private IEnumerator InvincibilityTimer(float invincibilityTime)
		{
			mInvincible = true;

			yield return new WaitForSeconds(invincibilityTime);
			mInvincible = false;
			mPC.mDefaultAttack.CanCast = true;
			if(mPC.Spell1 != null) mPC.mSpell1.CanCast = true;
			if(mPC.mSpell2 != null) mPC.mSpell2.CanCast = true;
		}
	#endregion
}
