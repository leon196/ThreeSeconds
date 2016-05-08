using UnityEngine;
using System.Collections;

public class Transformation : MonoBehaviour {

	public float speed = 0f;
	public float radius = 0f;
	public Vector3 right = Vector3.zero;
	public Vector3 up = Vector3.zero;

	Vector3 position;
	float timeElapsed;

	void Start () {
		position = transform.position;
		timeElapsed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime * speed;
		transform.position = position + right * Mathf.Cos(timeElapsed * speed) * radius + up * Mathf.Sin(timeElapsed * speed) * radius;
	}
}
