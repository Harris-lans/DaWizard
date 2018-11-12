using UnityEngine;

public class HatScript : MonoBehaviour 
{
	
	private void Awake()
	{
		//Moves the hat to the location of the players hat
		var HatHeight = GetComponent<MeshFilter>().sharedMesh.bounds.extents.y * transform.lossyScale.y;
		transform.position = new Vector3(transform.position.x, transform.position.y - HatHeight, transform.position.z);
	}

	private void Start()
	{
		//Plays death sound if player didn't die from falling
		if(transform.position.y> -1)
		{
			GetComponent<AudioSource>().Play();
		}
	}

	private void Update()
	{
		//Destroys the hat when it falls far enough
		if(transform.position.y < -20)
		{
			Destroy(gameObject);
		}
	}

}
