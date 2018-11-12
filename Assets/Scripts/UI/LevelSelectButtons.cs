using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButtons : MonoBehaviour 
{

	public void OnLevel1Pressed()
	{
		SceneManager.LoadScene("LoadingScene1");
	}

	public void OnLevel2Pressed()
	{
		SceneManager.LoadScene("LoadingScene2");
	}

	public void OnLevel3Pressed()
	{
		SceneManager.LoadScene("LoadingScene3");
	}

	public void OnBackButtonPressed()
	{
		SceneManager.LoadScene("MainMenu");
	}

}
