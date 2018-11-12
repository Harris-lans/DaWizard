using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour 
{
	[SerializeField]
	private Text mPauseScreenText;
	
	private Canvas mPauseMenu;
	private bool mIsPaused;
	private StandaloneInputModule mInputModule;
	private int mPausedBy;
	private Button[] mPauseScreenButtons;
	private EventSystem mEventsystem;

	

	// Use this for initialization
	void Awake () 
	{
		mPauseMenu = GetComponent<Canvas>();
		mInputModule = GetComponent<StandaloneInputModule>();
		mPauseScreenButtons = GetComponentsInChildren<Button>();
		mEventsystem = GetComponent<EventSystem>();
		mPauseMenu.enabled = false;
		mIsPaused = false;
		mPausedBy = 0;
		DisablePauseMenu();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameManager.GameStarted)
		{
			for(int i = 1; i <= PlayerController.Players.Count; i++)
			{
				// Checkig if the start button is being pressed on each joystick
				if(Input.GetKeyDown("joystick " + i + " button 7"))
				{

					if(!mIsPaused)
					{
						mPausedBy = i;
						mInputModule.submitButton = "Confirm-" + i;
						mInputModule.horizontalAxis = "LeftStickHorizontal-Controller" + i; 
						mInputModule.verticalAxis = "LeftStickVertical-Controller" + i;
						mPauseScreenText.text = "Paused by Player " + i;
						PauseGame();
					}
					else if (mIsPaused && mPausedBy == i)
					{
						// Resuming only if the person who pause it resumes
						ResumeGame();
					}
				}
			}
		}
	}

	public void PauseGame()
	{
		EnablePauseMenu();
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		DisablePauseMenu();
		Time.timeScale = 1;
	}

	public void QuitGame()
	{
		Time.timeScale = 1;

		FindObjectOfType<MainMenuMusic>().PlayMusic();

		SceneManager.LoadScene("MainMenu");
	}

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EnablePauseMenu()
	{
		mIsPaused = true;
		mPauseMenu.enabled = true;
		mEventsystem.enabled = true;
		foreach (Button button in mPauseScreenButtons)
		{
			button.enabled = true;
		}
	}

	private void DisablePauseMenu()
	{
		mIsPaused = false;
		mPauseMenu.enabled = false;
		mEventsystem.enabled = false;
		foreach (Button button in mPauseScreenButtons)
		{
			button.enabled = false;
		}
	}

	public void LevelSelect()
    {
        FindObjectOfType<MainMenuMusic>().PlayMusic();
		SceneManager.LoadScene("LevelSelect");
    }
}
