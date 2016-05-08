using UnityEngine;
using System.Collections;

public class PhotonCam : MonoBehaviour
{

	public float speed = 10;
	public float rayLength = 0.3f;

	public int gizmoReflections = 10;

	public bool realTimeControl = true;
	public float turnSpeed = 0.3f;
	float directionX = 1f;
	float directionY = 1f;

	public Material reflectionMaterial;
	public Camera reflectionCamera;
	Filter filter;

	void Start ()
	{
		filter = GameObject.FindObjectOfType<Filter>();

		// Renderer[] renderArray = GameObject.FindObjectsOfType<Renderer>();
		// foreach (Renderer renderer in renderArray) {
		// 	if (renderer.material.shader.name == "Unlit/Reflection") {
		// 		renderer.gameObject.AddComponent<DistanceTo>();
		// 		renderer.gameObject.GetComponent<DistanceTo>().target = transform;
		// 		renderer.gameObject.GetComponent<DistanceTo>().material = renderer.material;
		// 	}
		// }
	}

	void OnDrawGizmos()
	{
		Ray ray = new Ray(transform.position, transform.forward);

		Color rayColor = Color.cyan;

		int counter = 0;

		while (counter < gizmoReflections)
		{
			float distance = 100;

			Ray newRay = ray;

			if (GetReflection(ray, out newRay, 10000, out distance))
			{
				Gizmos.color = rayColor;
				Gizmos.DrawRay(ray.origin, ray.direction * distance);
				ray = newRay;
			}
			else
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(newRay.origin, newRay.direction * 100);
				return;
			}

			// koolkolorz(tm)
			rayColor.r += 0.01f;
			rayColor.g -= 0.03f;

			counter++;
		}
	}

	bool GetReflection(Ray ray, out Ray outRay, float maxDistance)
	{
		float x;
		return GetReflection(ray, out outRay, maxDistance, out x);
	}

	bool GetReflection(Ray ray, out Ray outRay, float maxDistance, out float distance)
	{
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, maxDistance))
		{
			Vector3 outVector = Vector3.Reflect(ray.direction.normalized, hit.normal);

			outRay = new Ray(hit.point, outVector);
			distance = hit.distance;

			return true;
		}

		distance = 0;
		outRay = ray;
		return false;
	}

	void Update()
	{

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		Ray reflectedRay;
		float maxDist = 100f;

		if (Physics.Raycast(ray, out hit, maxDist))
		{
			Vector3 reflection = Vector3.Reflect(transform.forward.normalized, hit.normal);
			reflectionCamera.transform.position = hit.point;// + reflection * (1f - Mathf.Max(0f, Mathf.Min(1f, hit.distance)));
			reflectionCamera.transform.rotation = Quaternion.LookRotation(reflection, Vector3.up);
		}

		if (GetReflection(ray, out reflectedRay, rayLength))
		{
			transform.position = reflectionCamera.transform.position;
			transform.rotation = reflectionCamera.transform.rotation;
			InverseX();
		}

		if (realTimeControl)
		{
			// if (Input.GetMouseButton(0)) {
				transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);
			// }

			transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime * directionX, Space.World);
			transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * turnSpeed * Time.deltaTime * directionY, Space.Self);
			transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime * directionX, Space.World);
			transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime * directionY, Space.Self);
		}
	}

	void InverseX () 
	{
		directionX *= -1f;
		if (filter)
		{
			filter.InverseX();
		}
	}
}
