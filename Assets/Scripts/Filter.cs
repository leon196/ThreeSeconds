using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Filter : MonoBehaviour 
{
	protected Material material;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Filter") );
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, material);
	}

	public void InverseX () {
		material.SetFloat("_InverseX", (material.GetFloat("_InverseX") + 1f) % 2f);
	}

	public void InverseY () {
		material.SetFloat("_InverseY", (material.GetFloat("_InverseY") + 1f) % 2f);
	}
}