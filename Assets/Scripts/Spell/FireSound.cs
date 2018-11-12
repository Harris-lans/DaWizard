using UnityEngine;

public class FireSound : MonoBehaviour 
{
	internal void PlaySound(bool isLast)
	{
		if(!isLast)
		{
			GetComponent<AudioSource>().Play();
		} else if (isLast)
		{
			GetComponent<AudioSource>().Play();
			transform.SetParent(null);
			Destroy(gameObject,3);
		}

	}
}
