using UnityEngine;

public class ParticleTest : MonoBehaviour 
{

	private Rigidbody _RB;

	private void Awake()
	{
		_RB = GetComponent<Rigidbody>();
	}
	
	private void Start()
	{
		_RB.velocity = new Vector3(10,0,0);
	}

}
