using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Unit 
{
	public static List<PlayerController> Players = new List<PlayerController>();
	public static List<PlayerController> ActivePlayers = new List<PlayerController>();

	#region Global Variables

    	#region Movement System Variables
        	// Variables used for the movement system
        	[SerializeField]
    		private float mMovementSpeed = 0.1f;

    		private Rigidbody mRigidBody; 
    		private float movementX;
    		private float movementY;
			private Vector3 movementDirection;
    	#endregion 

    	#region Targetting System Variables
    		// Variables used for the targetting system
    		[SerializeField]
    		private float mTurnSpeed = 7.0f;
    		private Quaternion mTargetRotation;
    	#endregion

    	#region Casting Spell System Variables
    		// Variables used for the Spell Casting system
    		[SerializeField]
    		internal Spell mDefaultAttack;
    		[SerializeField]
    		private Transform mRightHand;
    		[SerializeField]
    		private Transform mLeftHand;
    		internal Spell mSpell1;
    		internal Spell mSpell2;
    	#endregion

    	#region Player Input Variables

    		internal PlayerIndex mPlayerIndex;
			private GamePadState mGamePadState;
			private GamePadState mGamePadPrevoiusState;

    	#endregion

		#region Grounded Check Variables
			private GroundedCheck mGroundedCheck;

		#endregion

		#region Player Controller Events

			internal delegate void SpellCastAction(int spellNumber, float spellCoolDown);
			event SpellCastAction playerCastSpell;

			internal delegate void OnPlayerRespawn(float timeBeforePlayerRespawns);
			event OnPlayerRespawn playerReSpawnEvent;

			UnityEvent playerPickedUpSpell;

		#endregion

		//Slow effect variable
		private float mSlowTimeLeft = 0;
		private float mNormalSpeed;
		private bool mCoroutineStarted = false;

		#region Player Controller Spawn Information

			[SerializeField]
			private GameObject mSpawnParticles;

		#endregion

		#region LightningSpellMaterials

			[Header("Lightning Spell Materials")]
			public Material ParticleMaterial;
			public Material RibbonMaterial;

		#endregion LightningSpellMaterials

		#region SoundVariables

			private AudioSource mPickupSound;
			private AudioSource mFallSound;
			private AudioSource mHurtSound;
			private AudioSource mTauntSound;
			private AudioSource mVictorySound;
			private AudioSource mSpawnSound;

			private bool mFallPlayed;

		#endregion SoundVariables

		#region HatPrefab

		[SerializeField]
		private GameObject mHatObject;

		#endregion HatPrefab

		private Animator mAnim;

    #endregion

	#region Unity Functions
		
		void Awake()
		{
			// Adding the player to the static list
			Players.Add(this);

			mAnim = GetComponent<Animator>();
			mRigidBody = GetComponent<Rigidbody>();
			mSpell1 = null;
			mSpell2 = null;
			mDefaultAttack = Instantiate(mDefaultAttack);
			mNormalSpeed = mMovementSpeed;
			mGroundedCheck = GetComponent<GroundedCheck>();

			// Initializing events triggered by players
			playerPickedUpSpell = new UnityEvent();

			// Setting up the sounds
			AudioSource[] aSounds = GetComponents<AudioSource>();
			mPickupSound = aSounds[0];
			mFallSound = aSounds[1];
			mHurtSound = aSounds[2];
			mTauntSound = aSounds[3];
			mVictorySound = aSounds[4];
			mSpawnSound = aSounds[5];

		}

		void OnEnable()
		{
			// Adding the player to the static list of active players
			ActivePlayers.Add(this);
			GameObject spawnParticles = Instantiate(mSpawnParticles, transform.position, transform.rotation);
			spawnParticles.transform.SetParent(transform);
			mSpawnSound.Play();
			mFallPlayed = false;
		}

		void OnDisable()
		{
			// Removing the player from the static list of active players
			ActivePlayers.Remove(this);
		}

		void OnDestroy()
		{
			// Removing the player from the static list
			Players.Remove(this);
		}

		void FixedUpdate()
		{
			// Moving the Character
			if(mGroundedCheck.IsGrounded)
			{
				Vector3 NewMovement = new Vector3(movementX, 0, movementY).normalized;
				mRigidBody.velocity = new Vector3(NewMovement.x * mMovementSpeed, mRigidBody.velocity.y , NewMovement.z * mMovementSpeed);
			}
		}

	#endregion

	#region Class Functions
		// Update is called once per frame
		protected override void UnitUpdate () 
		{
			if (!GameManager.GameStarted)
			{
				return;
			}

			// Updating the Input States
			mGamePadPrevoiusState = mGamePadState;
			mGamePadState = GamePad.GetState(mPlayerIndex);

			#region Player Movement

				// Collecting information about movement input
				movementX = mGamePadState.ThumbSticks.Left.X;
				movementY = mGamePadState.ThumbSticks.Left.Y;

				// Updating Animator Parameters
				mAnim.SetFloat("HorizontalMovement", movementX);
				mAnim.SetFloat("VerticalMovement", movementY);

			#endregion

			#region Player Rotation

				// Getting state of the right stick
				float aimHorizontal = mGamePadState.ThumbSticks.Right.X;
				float aimVertical = - mGamePadState.ThumbSticks.Right.Y;
				
				Vector3 playerDirection = Vector3.right * aimVertical + Vector3.forward * aimHorizontal;
				if (playerDirection.magnitude > 0.0f)
				{
					mTargetRotation = Quaternion.LookRotation(playerDirection.normalized, Vector3.up);
					// Rotating it around one direction
					mTargetRotation *= Quaternion.Euler(0, 90, 0);
					transform.rotation = Quaternion.Slerp(transform.rotation, mTargetRotation, Time.deltaTime * mTurnSpeed);
				}

			#endregion

			#region Checking Availabiity of Spell

			// Setting the Spells to null if they cannot be used  any further
				if(mSpell1 != null && !mSpell1.CanBeUsed()) 
				{ 
					DestroyImmediate(mSpell1.gameObject);
					mSpell1 = null;
					playerPickedUpSpell.Invoke(); 
				}
				if(mSpell2 != null && !mSpell2.CanBeUsed()) 
				{ 
					DestroyImmediate(mSpell2.gameObject);
					mSpell2 = null;
					playerPickedUpSpell.Invoke(); 
				}

			#endregion

			#region Spell Casting

				if ((mGamePadState.Triggers.Left != 0 && mGamePadPrevoiusState.Triggers.Left == 0) && mSpell1 != null && mSpell1.CanCast)
				{
					mSpell1.CanCast = false;
					mAnim.SetTrigger("CastSpell");
					mSpell1.CastSpell(this, mLeftHand);
					playerCastSpell.Invoke(1, mSpell1.DelayBetweenSpells);
					StartCoroutine(SpellTimer(mSpell1));
				}
				if ((mGamePadState.Triggers.Right != 0 && mGamePadPrevoiusState.Triggers.Right == 0) && mSpell2 != null && mSpell2.CanCast)
				{
					mSpell2.CanCast = false;
					mAnim.SetTrigger("CastSpell");
					mSpell2.CastSpell(this, mRightHand);
					playerCastSpell.Invoke(2, mSpell2.DelayBetweenSpells);
					StartCoroutine(SpellTimer(mSpell2));
				}
				
				if ((mGamePadState.Buttons.RightShoulder != 0 && mGamePadPrevoiusState.Buttons.RightShoulder == 0) && mDefaultAttack.CanCast)
				{
					mDefaultAttack.CanCast = false;
					mDefaultAttack.CastSpell(this, mRightHand);
					StartCoroutine(SpellTimer(mDefaultAttack));
				}
			
			#endregion

			#region DeathFromFalling
			
				// Plays the falling sound
				if(transform.position.y < -1 && !mFallPlayed)
				{
					mFallSound.Play();
					mFallPlayed = true;
				}

			#endregion DeathFromFalling

		}

		#region DeathHandling

		internal void Death()
		{
			//Remove from player list
			Players.Remove(this);

			//Instantiate Hat
			var Height = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.extents.y * transform.lossyScale.y;
			Vector3 HatPosition = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
			Instantiate(mHatObject, HatPosition, transform.rotation);

			//Destroy gameObject
			Destroy(gameObject);
		}

		#endregion DeathHandling

		#region Subscriptions for events
			internal void SubscribeToPlayerCastSpellEvent(SpellCastAction action)
			{
				playerCastSpell += action;
			}
			
			internal void SubscribeToPlayerPickUpEvent(UnityAction action)
			{
				playerPickedUpSpell.AddListener(action);
			}

			internal void SubscribeToPlayerRespawnEvent(OnPlayerRespawn action)
			{
				playerReSpawnEvent += action;
			}

		#endregion

		#region Invoking Player Event

			internal void InvokeRespawnEvent(float timeBeforePlayerRespawn)
			{
				playerReSpawnEvent.Invoke(timeBeforePlayerRespawn);
			}

		#endregion

		#region Player Initialization
		
			public void Initialize(PlayerIndex playerIndex)
			{
				mPlayerIndex = playerIndex;
			}

		#endregion

		#region Spell Management

			#region Spell Pickups
				internal void PickUpSpells(Spell spellToPickUp)
				{
					if (mSpell1 != null && mSpell2 != null)
					{
						int randomNumber = Random.Range(0,2);
						if (randomNumber == 0) 
						{ 
							mSpell1 = spellToPickUp; 
						}
						else 
						{ 
							mSpell1 = spellToPickUp; 
						}
					}
					else if (mSpell1 == null)
					{
						mSpell1 = spellToPickUp;
					}
					else if (mSpell2 == null)
					{
						mSpell2 = spellToPickUp;
					}

					// Informing the UI about the Pick Up
					playerPickedUpSpell.Invoke();
				}

			#endregion 

			#region Spell Timer Co-Routines

				IEnumerator SpellTimer(Spell spell)
				{
					yield return new WaitForSeconds(spell.DelayBetweenSpells);
					spell.CanCast = true;
				}

			#endregion

		#endregion

		#region Sounds
		
		//Functions for each sound to be played by another script or this one

			internal void PickupSound()
			{
				mPickupSound.Play();
			}

			internal void HurtSound()
			{
				mHurtSound.Play();
			}
			internal void TauntSound()
			{
				mTauntSound.Play();
			}
			internal void VictorySound(float delay)
			{
				mVictorySound.PlayDelayed(delay);
			}

		#endregion Sounds

		#region CharacterEffects
			internal void Slow(float SlowFactor, float SlowDuration)
			{
				mMovementSpeed = mNormalSpeed*SlowFactor;
				mSlowTimeLeft = SlowDuration;
				if(!mCoroutineStarted)
				{
					StartCoroutine(SlowEffect());
					mCoroutineStarted = true;
				}
			}

			IEnumerator SlowEffect()
			{
				while(mSlowTimeLeft > 0)
				{
					mSlowTimeLeft -= Time.deltaTime;
					//print(mSlowTimeLeft);
					yield return null;
				}

				mSlowTimeLeft = 0;
				mMovementSpeed = mNormalSpeed;
				mCoroutineStarted = false;
			}


		#endregion CharacterEffects
	#endregion

	#region Properties
		internal Spell Spell1
		{
			get
			{
				return mSpell1;
			}
		}
		internal Spell Spell2
		{
			get
			{
				return mSpell2;
			}
		}

		internal PlayerIndex PlayerIndex
		{
			get
			{
				return mPlayerIndex;
			}
		}

	#endregion
}
