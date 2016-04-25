using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	float speedRotation = 20f;
	float speedTranslation = 0.1f;

	void Start () 
	{	
	}
	
	void Update () 
	{
		float h = Input.GetAxis("Mouse X") * speedRotation * Time.deltaTime;
		float v = -Input.GetAxis("Mouse Y") * speedRotation * Time.deltaTime;

		transform.Rotate(Vector3.up * h);
		transform.Rotate(transform.right * v);

		transform.Translate(transform.forward * speedTranslation * Time.deltaTime);
  }
}
