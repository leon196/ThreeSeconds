using UnityEngine;
using System.Collections;

public class DistanceTo : MonoBehaviour 
{
	public Transform target;
	public Material material;

	void Update ()
	{
		material.SetFloat("_PlayerDistance", Vector3.Distance(transform.position, target.position));
	}

}