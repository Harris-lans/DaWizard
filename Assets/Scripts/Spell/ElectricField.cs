using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
	[SerializeField]
	private float mConductionRange;
	[SerializeField]
	private float mDelayBeforeSearchingForTarget = 1;
	[SerializeField]
	private float mDamagePerSecond;
    [SerializeField]
    private LightningArc mLightningArcPrefab;

	private PlayerController mSpellOwner;
	private LightningSpell mFieldOwner;
    private LightningArc mLightningArc;

	#region Class Functions

    	internal void Initialize(PlayerController spellOwner, LightningSpell fieldOwner, int numberOfConductionsLeft, PlayerHealth target)
        {
            mSpellOwner = spellOwner;
            mFieldOwner = fieldOwner;

            //Changing materials in particlerenderer
            GetComponentInChildren<ParticleSystemRenderer>().material = spellOwner.ParticleMaterial;
            GetComponentInChildren<ParticleSystemRenderer>().trailMaterial= spellOwner.RibbonMaterial;

            // Adding self to the electricField list
            mFieldOwner.RegisterField(this);

            // Checking if the electric field can conduct to more objects
            if (numberOfConductionsLeft > 0)
            {
                --numberOfConductionsLeft;
                StartCoroutine(SearchForTargets(numberOfConductionsLeft));
            }

            // Start Damaging the target the electric field is on
            if (target != null) { StartCoroutine(DamageTarget(target)); }
        }

        void ConductTo(Transform objectToConductTo, int numberOfConductionsLeft)
        {
            PlayerHealth target = objectToConductTo.GetComponent<PlayerHealth>();
            GameObject newElectricField = Instantiate(gameObject, objectToConductTo.position, objectToConductTo.rotation);
            
            // Making the target the parent so that the electric field moves with the target
            newElectricField.transform.parent = objectToConductTo;

            // Maintaining the scale of the electricfield
            newElectricField.transform.localScale = transform.localScale;    
            newElectricField.GetComponent<ElectricField>().Initialize(mSpellOwner, mFieldOwner, numberOfConductionsLeft, target);

            // Connecting the electric fields with lightning arcs
            mLightningArc = Instantiate(mLightningArcPrefab, transform.position, transform.rotation);
			mLightningArc.Initialize(transform, objectToConductTo, mSpellOwner);

        }

	#endregion

	#region Unity Functions
        private void OnDestroy()
        {
            // Stopping the damage co-routine
            StopCoroutine("DamageTarget");

            // Removing the electric field from the owner's field as it is now destroyed
            mFieldOwner.RemoveField(this);

            // Deleting the Lightning Arc
            if (mLightningArc != null)
            {
                GameObject.Destroy(mLightningArc.gameObject);
            }
        }

    #endregion


	#region Co-Routines

        IEnumerator SearchForTargets(int numberOfConductionsLeft)
        {
            yield return new WaitForSeconds(mDelayBeforeSearchingForTarget);
        
            // Searching for colliders within a radius
            Collider[] objectsInRange = Physics.OverlapSphere(transform.position, mConductionRange);
            float minDistanceOfPlayer = Mathf.Infinity;
            float minDistanceOfObject = Mathf.Infinity;

            Transform nearestPlayer = null;
            Transform nearestConductiveObject = null;

            foreach (Collider objectInRange in objectsInRange)
            {
                // Checking if the object or player in range is conductive

                // Making sure that the electric field does not spawn another electric field on it's own target
                if (objectInRange.transform != transform.parent)
                {
                    PlayerController playerController = objectInRange.GetComponent<PlayerController>();
                    if (playerController != null && playerController != mSpellOwner)
                    {
                        // Selecting the nearest Player
                        float distanceFromTheObject = Vector3.Distance(transform.position, objectInRange.transform.position);
                        if (distanceFromTheObject < minDistanceOfPlayer)
                        {
                            minDistanceOfPlayer = distanceFromTheObject;
                            nearestPlayer = objectInRange.transform;
                        }
                    }
                    else if (objectInRange.GetComponent<IConductive>() != null)
                    {
                        // Selecting the nearest Conductive Object
                        float distanceFromTheObject = Vector3.Distance(transform.position, objectInRange.transform.position);
                        if (distanceFromTheObject < minDistanceOfObject)
                        {
                            minDistanceOfObject = distanceFromTheObject;
                            nearestConductiveObject = objectInRange.transform;
                        }
                    }
                }
            }

            // Spawning the electric field on the target (Priortizing Players over Objects)
            if (nearestPlayer != null)
            {
                ConductTo(nearestPlayer, numberOfConductionsLeft);
            }
            else if (nearestConductiveObject != null)
            {
                ConductTo(nearestConductiveObject, numberOfConductionsLeft);
            }
        }

        IEnumerator DamageTarget(PlayerHealth objectToDamage)
        {
            while (true)
            {
               
                if (objectToDamage != null)
                {
                    objectToDamage.DealDamage(mDamagePerSecond*0.5f);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

    #endregion

}
