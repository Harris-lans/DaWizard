using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTheScene : MonoBehaviour
{
	public void LoadScene1()
	{
		StopMusic();
		SceneManager.LoadScene("Stage1");
	}
	
	public void LoadScene2()
	{
		StopMusic();
		SceneManager.LoadScene("Stage2");
	}

	public void LoadScene3()
	{
		StopMusic();
		SceneManager.LoadScene("Stage3");
	}

	private void StopMusic()
	{
		MainMenuMusic MusicSource = FindObjectOfType<MainMenuMusic>();
		MusicSource.StopMusic();
	}
}
