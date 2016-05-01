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
	Filter filter;

	void Start ()
	{
		filter = GameObject.FindObjectOfType<Filter>();

		reflectionMaterial.SetVector("_RayDirection", Vector3.left);
		reflectionMaterial.SetFloat("_RayDistance", 100f);
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

	void FixedUpdate()
	{

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		Ray reflectedRay;

		// Warp shader effect

		if (Physics.Raycast(ray, out hit, 100f)) {
			reflectionMaterial.SetVector("_RayDirection", hit.normal);
			// reflectionMaterial.SetVector("_RayDirection", Vector3.Reflect(transform.forward.normalized, hit.normal));
			reflectionMaterial.SetFloat("_RayDistance", hit.distance);
		} else {
			reflectionMaterial.SetFloat("_RayDistance", 1000f);
		}

		if (GetReflection(ray, out reflectedRay, rayLength))
		{
			// transform.position = reflectedRay.origin; // cause of this reflection is not "seamless", there is a small jump, but it keeps moving along the preview ray
			transform.position = reflectedRay.origin;// + reflectedRay.direction * rayLength;

			Vector3 dir = reflectedRay.direction;
			// dir.y = 0.0f;
			dir = Vector3.Normalize(dir);

			transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
			// transform.Rotate(Vector3.forward * Vector3.Dot(dir, Vector3.up) * 360f, Space.Self);

			// Vector3 up = Vector3.Cross(ray.direction, reflectedRay.direction).normalized;
			// transform.rotation = Quaternion.LookRotation(reflectedRay.direction, up);
			// transform.forward = reflectedRay.direction;

			InverseX();
		}

		if (realTimeControl)
		{
			// if (Input.GetMouseButton(0)) {
				transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);
			// }

			transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * directionX, Space.World);
			transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * turnSpeed * directionY, Space.Self);
			transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * turnSpeed * directionX, Space.World);
			transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * turnSpeed * directionY, Space.Self);
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
