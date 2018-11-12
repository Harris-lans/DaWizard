using UnityEngine;
using UnityEngine.UI;

public class AutoSelectButton : MonoBehaviour
{

	private void OnEnable()
	{
		GetComponent<Button>().Select();
	}

}
