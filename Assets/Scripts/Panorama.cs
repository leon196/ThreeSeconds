using UnityEngine;
using System.Collections;

public class Panorama : MonoBehaviour 
{
	int cubemapSize = 1024;
	bool oneFacePerFrame = false;
	Camera cam;
	int currentTexture;
	RenderTexture[] textures;
	public Material material;

	void Start () {
		currentTexture = 0;
		textures = new RenderTexture[2];
		CreateTextures();
		// render all six faces at startup
		UpdateCubemap( 63 );
	}

	void LateUpdate () {
		if (oneFacePerFrame) {
			int faceToRender = Time.frameCount % 6;
			int faceMask = 1 << faceToRender;
			UpdateCubemap (faceMask);
		} else {
			UpdateCubemap (63); // all six faces
		}
	}

	void UpdateCubemap (int faceMask) {
		if (!cam) {
			GameObject go = new GameObject ("CubemapCamera");
			go.hideFlags = HideFlags.HideAndDontSave;
			go.transform.position = transform.position;
			go.transform.rotation = Quaternion.identity;
			go.AddComponent<Camera>();
			cam = go.GetComponent<Camera>();
			cam.backgroundColor = new Color(0,0,0,0);
			cam.clearFlags = CameraClearFlags.SolidColor;
			cam.farClipPlane = 100; // don't render very far into cubemap
			cam.cullingMask = 1 << 0;
			cam.enabled = false;
		}

		material.mainTexture = GetCurrentTexture();

		NextTexture();
		
		cam.transform.position = transform.position;
		cam.RenderToCubemap (GetCurrentTexture(), faceMask);
	}

	void CreateTextures ()
	{
		// Shader.SetGlobalTexture(textureName, placeholder);
		for (int i = 0; i < textures.Length; ++i) {
			textures[i] = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);
			// textures[i].filterMode = FilterMode.Point;
			textures[i].isCubemap = true;
			textures[i].Create();
		}
	}

	void NextTexture ()
	{
		currentTexture = (currentTexture + 1) % 2;
	}

	public RenderTexture GetCurrentTexture ()
	{
		return textures[currentTexture];
	}
}
