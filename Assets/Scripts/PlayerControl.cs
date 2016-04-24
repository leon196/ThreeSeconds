using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	float speed = 1f;

	void Start () 
	{	
	}
	
	void Update () 
	{
		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");

		transform.Rotate(Vector3.up * h * Time.deltaTime);
		transform.Rotate(transform.right * h * Time.deltaTime);

		transform.Translate(transform.forward * speed * Time.deltaTime);
	}
}
