using UnityEngine;

public class HitNumberRotation : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(Camera.main.transform);
		transform.Rotate(180,0,0);
	}
}
