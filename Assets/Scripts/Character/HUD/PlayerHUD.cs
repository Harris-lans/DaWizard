using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour 
{
	public static List<PlayerHUD> PlayerHUDList;

	#region Global Variables

		[SerializeField, Range(1, 4)]
		private int mLinkedToPlayer = 1;
		[SerializeField]
		private Sprite mEmptySlot;
		[SerializeField]
		private Sprite mDeadPlayerFrameWork;
		[SerializeField]
		private Image mSpell1Sprite;
		[SerializeField]
		private Image mSpell2Sprite;
		[SerializeField]
		private Text mSpell1Count;
		[SerializeField]
		private Text mSpell2Count;
		[SerializeField]
		private Slider mPlayerHealthBar;
		[SerializeField]
		private Slider mSpell1CoolDownBar;
		[SerializeField]
		private Slider mSpell2CoolDownBar;
		[SerializeField]
		private Image mUIFrameWork;
		[SerializeField]
		private Text mRespawnInformation;

		private PlayerController mPlayer;
		private PlayerHealth mPlayerHealth;
		private IEnumerator spell1CoolDown;
		private IEnumerator spell2CoolDown;
	
	#endregion

	#region Unity Functions

		void Start() 
		{
			PlayerHUDList.Add(this);

			// Disabling the UI for the players who do not exist
			gameObject.SetActive(false);
		}
		
		void OnDestroy()
		{
			PlayerHUDList.Remove(this);	
		}

		void RefreshSpellCount(int spellNumber, float coolDownTime)
		{
			// Refreshing Spell Count and also start cool-downs

			if (spellNumber == 1)
			{
				mSpell1Count.text = "";
				if(mPlayer.Spell1 != null && mPlayer.Spell1.UsesLeft > 0)
				{
					mSpell1Count.text += mPlayer.Spell1.UsesLeft;
					StartCoroutine(CoolDownTimer(mSpell1CoolDownBar, coolDownTime));
				}
			}

			else if (spellNumber == 2)
			{
				mSpell2Count.text = "";
				if(mPlayer.Spell2 != null && mPlayer.Spell2.UsesLeft > 0)
				{
					mSpell2Count.text += mPlayer.Spell2.UsesLeft;
					StartCoroutine(CoolDownTimer(mSpell2CoolDownBar, coolDownTime));
				}
			}
		}

		void RefreshSpellIcon()
		{
			// Refreshing Spell Icons on the Player HUD

			mSpell1Sprite.sprite = mEmptySlot;
			mSpell2Sprite.sprite = mEmptySlot;

			if(mPlayer.Spell1 != null)
			{
				mSpell1Sprite.sprite = mPlayer.Spell1.mSpellSplashScreenIcon;
				RefreshSpellCount(1, 0);
			}
			
			if(mPlayer.Spell2 != null)
			{
				mSpell2Sprite.sprite = mPlayer.Spell2.mSpellSplashScreenIcon;
				RefreshSpellCount(2, 0);
			}
		}

		void RefreshPlayerHealthBar()
		{
			// Refreshing Player Health Bar

			mPlayerHealthBar.value = mPlayerHealth.PlayerHealthFraction;

			if (mPlayerHealthBar.value <= 0)
			{
				Destroy(mSpell1Sprite);
				Destroy(mSpell2Sprite);
				Destroy(mSpell1Count);
				Destroy(mSpell2Count);
				mUIFrameWork.sprite = mDeadPlayerFrameWork;
			}
		}

		void ShowPlayerRespawnTimer(float timeTakenToRespawn)
		{
			StartCoroutine(RespawnTimer(timeTakenToRespawn));
		}

		internal void SpawnPlayerUI()
		{
			gameObject.SetActive(true);

			// Enabling the Player_HUD 
			mPlayer = PlayerController.Players[mLinkedToPlayer - 1];
			if(mPlayer != null)
			{
				mPlayerHealth = mPlayer.GetComponent<PlayerHealth>();

				// Subscribing to events to update UI elements
				mPlayer.SubscribeToPlayerCastSpellEvent(this.RefreshSpellCount);
				mPlayer.SubscribeToPlayerPickUpEvent(this.RefreshSpellIcon);
				mPlayer.SubscribeToPlayerRespawnEvent(this.ShowPlayerRespawnTimer);
				mPlayerHealth.subscribeToPlayerDamageEvent(this.RefreshPlayerHealthBar);
			}

			// Making Sure that the Respawn Information is empty
			mRespawnInformation.text = "";
		}

	#endregion

	#region  Co-Routine

		IEnumerator CoolDownTimer(Slider coolDownBar, float coolDownTime)
		{
			float timer = 0;
			while(timer < coolDownTime)
			{
				coolDownBar.value = timer / coolDownTime;
				yield return new WaitForSeconds(0.05f);
				timer += 0.05f;
			}
		}

		IEnumerator RespawnTimer(float timeTakenToRespawn)
		{
			float timer = timeTakenToRespawn;
			while (timer >= 0)
			{
				mRespawnInformation.text = "Respawns in " + ((int)timer).ToString();
				yield return new WaitForSeconds(0.5f);
				timer -= 0.5f;
			}
			mRespawnInformation.text = "";
		}

	#endregion

	#region Properties

		internal int LinkedToPlayer
		{
			get
			{
				return mLinkedToPlayer;
			}
		}

	#endregion
}
