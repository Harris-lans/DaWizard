using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
	[SerializeField]
	private Text mGameStartTimer;
	[SerializeField]
	private Image mBackgroundCover;

	// Use this for initialization
	void Start () 
	{
		mGameStartTimer.enabled = false;
		mBackgroundCover.enabled = false;
	}
	
	internal void StartGameTimer(float timeToWaitFor)
	{
		StartCoroutine(GameStartTimer(timeToWaitFor));
	}

	IEnumerator GameStartTimer(float timeToWaitFor)
	{
		mGameStartTimer.enabled = true;
		mBackgroundCover.enabled = true;

		float time = timeToWaitFor;
		while(time > 0)
		{
			mGameStartTimer.text = (Mathf.CeilToInt(time)).ToString(); 
			yield return new WaitForSeconds(0.5f);
			time -= 0.5f;
		}
		GetComponent<AudioSource>().Play();
		mGameStartTimer.enabled = false;
		mBackgroundCover.enabled = false;
	}
}
