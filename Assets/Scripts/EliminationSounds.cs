using UnityEngine;

public class EliminationSounds : MonoBehaviour 
{

	void Start () 
	{
		GetComponent<AudioSource>().PlayDelayed(0.5f);
	}

}
