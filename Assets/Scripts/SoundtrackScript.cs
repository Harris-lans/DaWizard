using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackScript : MonoBehaviour 
{
	private AudioSource mAudio;

	private void Awake()
	{
		mAudio = GetComponent<AudioSource>();	
	}

	internal void FadeOut(float Time)
	{
		StartCoroutine(FadeVolume(Time));
	}

	IEnumerator FadeVolume(float FadeTime)
	{
		float startVolume = mAudio.volume;
 
        while (mAudio.volume > 0) {
            mAudio.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
 
        mAudio.Stop();
	}
}