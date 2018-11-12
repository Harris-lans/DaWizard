using UnityEngine;

public class SpawnParticleSpin : MonoBehaviour 
{
	[SerializeField]
	private float _RotationPerSecond = 180;

	void Update () 
	{
		float frameRotation = _RotationPerSecond * Time.deltaTime;
		transform.Rotate(0,frameRotation,0);
	}
}
