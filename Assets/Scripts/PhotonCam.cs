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

	GameObject triangle;
	Mesh triangleMesh;
	Filter filter;

	void Start ()
	{
		filter = GameObject.FindObjectOfType<Filter>();

		triangleMesh = new Mesh();
		triangleMesh.Clear();
		triangleMesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
		triangleMesh.normals = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
		triangleMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
		triangleMesh.triangles = new int[] { 0, 1, 2 };

		triangle = new GameObject();
		triangle.AddComponent<MeshRenderer>();
		triangle.GetComponent<MeshRenderer>().material = reflectionMaterial;
		triangle.AddComponent<MeshFilter>();
		// triangle.GetComponent<MeshFilter>().mesh = triangleMesh;

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
		transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		Ray reflectedRay;

		// Warp triangle

		if (Physics.Raycast(ray, out hit, 10000))
		{
			reflectionMaterial.SetVector("_RayDirection", hit.normal);
			reflectionMaterial.SetFloat("_RayDistance", hit.distance);

			// hit.distance;
			// MeshCollider meshCollider = hit.collider as MeshCollider;
			// if (meshCollider == null || meshCollider.sharedMesh == null)
			// 	return;


/*
			triangle.transform.position = hit.transform.position;
			triangle.transform.rotation = hit.transform.rotation;
			triangle.transform.localScale = hit.transform.localScale;
			
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			int[] triangles = mesh.triangles;
			int i0 = triangles[hit.triangleIndex * 3 + 0];
			int i1 = triangles[hit.triangleIndex * 3 + 1];
			int i2 = triangles[hit.triangleIndex * 3 + 2];
			Vector3 p0 = vertices[i0];
			Vector3 p1 = vertices[i1];
			Vector3 p2 = vertices[i2];
			Vector3 n0 = normals[i0];
			Vector3 n1 = normals[i1];
			Vector3 n2 = normals[i2];

			// Camera.main.transform.TransformPoint(Camera.main.transform.)

			Vector3[] triangleVertices = triangleMesh.vertices;
			Vector3[] triangleNormals = triangleMesh.normals;

			triangleVertices[0] = p0;
			triangleVertices[1] = p1;
			triangleVertices[2] = p2;

			triangleNormals[0] = n0;
			triangleNormals[1] = n1;
			triangleNormals[2] = n2;

			triangleMesh.vertices = triangleVertices;
			triangleMesh.normals = triangleVertices;
			triangleMesh.RecalculateBounds();
			*/
		}

		// Bounce

		if (GetReflection(ray, out reflectedRay, rayLength))
		{
			// transform.position = reflectedRay.origin; // cause of this reflection is not "seamless", there is a small jump, but it keeps moving along the preview ray
			transform.position = reflectedRay.origin;// + reflectedRay.direction * rayLength;

			Vector3 dir = reflectedRay.direction;
			// dir.y = 0.0f;
			// dir = Vector3.Normalize(dir);

			transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

			// Vector3 normal = Vector3.Normalize(Vector3.Cross(reflectedRay.direction, ray.direction));
			// transform.rotation = Quaternion.LookRotation(reflectedRay.direction, normal);
			// transform.forward = reflectedRay.direction;

			InverseX();
		}

		if (realTimeControl)
		{
			// gimbal lock prone, should not be allowed above/below certain degrees?
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
