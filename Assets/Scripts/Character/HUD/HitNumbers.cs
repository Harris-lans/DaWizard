using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitNumbers : MonoBehaviour 
{
	private Text _HitText;
	private float _DamageToDisplay = 0;

	[SerializeField, Tooltip("Ensure that the time to fade is shorter than the life span")]
	private float _TimeToFade = 1f;
	
	private void Awake()
	{
		_HitText = GetComponent<Text>();

		
	}

	private void Update()
	{
		if(_HitText.canvasRenderer.GetColor().a == 0)
		{
			_DamageToDisplay = 0;
		}
	}

	internal void SetText(float DamageTaken)
	{
		_HitText.canvasRenderer.SetColor(Color.red);
		_DamageToDisplay += DamageTaken;
		_HitText.text = Mathf.FloorToInt(_DamageToDisplay).ToString();
		_HitText.canvasRenderer.SetAlpha(1); //Resets alpha to 1 whenever text is updated
		Fade();
	}

	internal void SetText(string messageToDisplay, float invincibilityTime)
	{
		_HitText.text = messageToDisplay;
		_HitText.canvasRenderer.SetAlpha(1); //Resets alpha to 1 whenever text is updated
		Fade(invincibilityTime);
	}

	internal void Fade()
	{
		_HitText.CrossFadeAlpha(0f , _TimeToFade, true);
	}

	internal void Fade(float timeToFade)
	{
		_HitText.CrossFadeAlpha(0f , timeToFade, true);
	}

}
