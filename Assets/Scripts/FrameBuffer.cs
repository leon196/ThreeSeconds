using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class FrameBuffer : MonoBehaviour
{
	public Material material;
	public string textureName = "_FrameBuffer";
	Camera cameraCapture;
	int currentTexture;
	RenderTexture[] textures;
	Texture2D placeholder;

	void Awake ()
	{
		currentTexture = 0;
		textures = new RenderTexture[2];
		placeholder = new Texture2D(1, 1);
    placeholder.SetPixel(0, 0, Color.black);
		placeholder.Apply();
	}

	void Start ()
	{
		cameraCapture = GetComponent<Camera>();
		CreateTextures();
	}

	void Update ()
	{
		material.mainTexture = GetCurrentTexture();
		Shader.SetGlobalTexture(textureName, GetCurrentTexture());
		NextTexture();
		cameraCapture.targetTexture = GetCurrentTexture();
	}

	void CreateTextures ()
	{
		Shader.SetGlobalTexture(textureName, placeholder);
		for (int i = 0; i < textures.Length; ++i) {
			if (textures[i]) {
				textures[i].Release();
			}
			textures[i] = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);
			textures[i].Create();
			textures[i].filterMode = FilterMode.Point;

			// CUBEMAP
			textures[i].isCubemap = true;
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

	public void UpdateResolution ()
	{
		cameraCapture.targetTexture = null;
		CreateTextures();
	}
}