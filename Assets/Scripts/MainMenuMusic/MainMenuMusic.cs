using UnityEngine;

public class MainMenuMusic : MonoBehaviour 
{

	private AudioSource _Music;

	private void Awake()
	{
		//Destroys any new main menu music created
		MainMenuMusic[] MusicSources = FindObjectsOfType<MainMenuMusic>();
		if(MusicSources.Length >1) Destroy(MusicSources[1].gameObject);

		_Music = GetComponent<AudioSource>();
	}

	internal void PlayMusic()
	{
		_Music.Play();
	}
	
	internal void StopMusic()
	{
		_Music.Stop();
	}
}
