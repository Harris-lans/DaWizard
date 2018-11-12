using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour 
{
	[SerializeField]
	private SoundtrackScript mSoundtrack;

	[SerializeField]
	private Text mAnnouncerText;

	[SerializeField]
	private AudioClip[] mAnnouncerSounds;

	private string[] mPlayerColours = new string[] { "Red", "Blue", "Green", "Yellow" };


	internal void AnnounceWinner(int playerIndex)
	{
		mSoundtrack.FadeOut(8f);
		AudioSource aSource = GetComponent<AudioSource>();

		// Disabling the input for all players
		foreach (PlayerController player in PlayerController.Players)
		{
			player.enabled = false;
		}
		if(mAnnouncerText != null)
		{
			mAnnouncerText.text = mPlayerColours[playerIndex] + " player has won the Trials";
		}

		aSource.clip = mAnnouncerSounds[playerIndex];
		aSource.PlayDelayed(2f);
		PlayerController.Players[0].VictorySound(3.5f);
	}

	public void QuitGame()
	{
		FindObjectOfType<MainMenuMusic>().PlayMusic();
		SceneManager.LoadScene("MainMenu");
	}

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelSelect()
    {
		SceneManager.LoadScene("LevelSelect");
        FindObjectOfType<MainMenuMusic>().PlayMusic();
    }
}
