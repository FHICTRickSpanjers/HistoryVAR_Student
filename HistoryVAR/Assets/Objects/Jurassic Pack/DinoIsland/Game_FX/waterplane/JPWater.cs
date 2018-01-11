using System;
using System.Collections.Generic;
using UnityEngine;

public enum WaterQuality { High = 2, Medium = 1, Low = 0, }

[ExecuteInEditMode]
public class JPWater : MonoBehaviour
{
  public Material WaterMaterial;
  public WaterQuality WaterQuality = WaterQuality.High;
  public bool EdgeBlend = true;
	public bool GerstnerDisplace = true;
	public bool DisablePixelLights = true;
	public int ReflectionSize = 256;
	public float ClipPlaneOffset = 0.07f;
	public LayerMask ReflectLayers = -1;

	private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>(); // Camera -> Camera table
	private RenderTexture m_ReflectionTexture;
	private int m_OldReflectionTextureSize;
	private static bool s_InsideWater;

	public Light DirectionalLight;

	void Update()
	{
		if(WaterMaterial)
		{
			if(WaterQuality > WaterQuality.Medium) WaterMaterial.shader.maximumLOD = 501;
			else if(WaterQuality > WaterQuality.Low) WaterMaterial.shader.maximumLOD = 301;
			else WaterMaterial.shader.maximumLOD = 201;
			
			if(DirectionalLight) WaterMaterial.SetVector("_DirectionalLightDir", DirectionalLight.transform.forward);
	
			if(!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) | !EdgeBlend)
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
				Shader.DisableKeyword("WATER_EDGEBLEND_ON");
			}
			else
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_ON");
				Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
				// just to make sure (some peeps might forget to add a water tile to the patches)
				if(Camera.main) Camera.main.depthTextureMode |= DepthTextureMode.Depth;
			}
			
			if(GerstnerDisplace)
			{
				Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
				Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
			}
			else
			{
				Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
				Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
			}
		}
	}

	// This is called when it's known that the object will be rendered by some
	// camera. We render reflections and do other updates here.
	// Because the script executes in edit mode, reflections for the scene view
	// camera will just work!
	void OnWillRenderObject()
	{
		Camera cam = Camera.current;
		if(!WaterMaterial | !cam | s_InsideWater) return;
		// Safeguard from recursive water reflections.
		s_InsideWater = true;
		
		Camera reflectionCamera;
		CreateWaterObjects(cam, out reflectionCamera);
		
		// find out the reflection plane: position and normal in world space
		Vector3 pos = transform.position;
		Vector3 normal = transform.up;
		
		// Optionally disable pixel lights for reflection
		int oldPixelLightCount = QualitySettings.pixelLightCount;
		if(DisablePixelLights) QualitySettings.pixelLightCount = 0;
		
		UpdateCameraModes(cam, reflectionCamera);
		
		// Reflect camera around reflection plane
		float d = -Vector3.Dot(normal, pos) - ClipPlaneOffset;
		Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);
		
		Matrix4x4 reflection = Matrix4x4.zero;
		CalculateReflectionMatrix(ref reflection, reflectionPlane);
		Vector3 oldpos = cam.transform.position;
		Vector3 newpos = reflection.MultiplyPoint(oldpos);
		reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;
		
		// Setup oblique projection matrix so that near plane is our reflection
		// plane. This way we clip everything below/above it for free.
		Vector4 clipPlane = CameraSpacePlane(reflectionCamera, pos, normal, 1.0f);
		reflectionCamera.projectionMatrix = cam.CalculateObliqueMatrix(clipPlane);
		
		reflectionCamera.cullingMask = ~(1 << 4) & ReflectLayers.value; // never render water layer
		reflectionCamera.targetTexture = m_ReflectionTexture;
		GL.invertCulling = true;
		reflectionCamera.transform.position = newpos;
		Vector3 euler = cam.transform.eulerAngles;
		reflectionCamera.transform.eulerAngles = new Vector3(-euler.x, euler.y, euler.z);
	
		reflectionCamera.Render();
		reflectionCamera.transform.position = oldpos;
		GL.invertCulling = false;
		GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", m_ReflectionTexture);
		
		// Restore pixel light count
		if(DisablePixelLights) QualitySettings.pixelLightCount = oldPixelLightCount;
		s_InsideWater = false;
	}

	// Cleanup all the objects we possibly have created
	void OnDisable()
	{
		if(m_ReflectionTexture)
		{
			DestroyImmediate(m_ReflectionTexture);
			m_ReflectionTexture = null;
		}
		
		foreach (var kvp in m_ReflectionCameras)
		{
			DestroyImmediate((kvp.Value).gameObject);
		}
		m_ReflectionCameras.Clear();
	}

	void UpdateCameraModes(Camera src, Camera dest)
	{
		if(dest == null) return;

		// set water camera to clear the same way as current camera
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		

		// update other values to match current camera.
		// even ifwe are supplying custom camera&projection matrices, 
		// some of values are used elsewhere (e.g. skybox uses far plane)
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}
	
	// On-demand create any objects we need for water
	void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera)
	{
		reflectionCamera = null;
			// Reflection render texture
			if(!m_ReflectionTexture | m_OldReflectionTextureSize != ReflectionSize)
			{
				if(m_ReflectionTexture)
				{
					DestroyImmediate(m_ReflectionTexture);
				}
				m_ReflectionTexture = new RenderTexture(ReflectionSize, ReflectionSize, 16);
				m_ReflectionTexture.name = "__WaterReflection" + GetInstanceID();
				m_ReflectionTexture.isPowerOfTwo = true;
				m_ReflectionTexture.hideFlags = HideFlags.DontSave;
				m_OldReflectionTextureSize = ReflectionSize;
			}
			// Camera for reflection
			m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
			if(!reflectionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
			{
				GameObject go = new GameObject("Water Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
				reflectionCamera = go.GetComponent<Camera>();
				reflectionCamera.enabled = false;
				reflectionCamera.transform.position = transform.position;
				reflectionCamera.transform.rotation = transform.rotation;
				reflectionCamera.gameObject.AddComponent<FlareLayer>();
				go.hideFlags = HideFlags.HideAndDontSave;
				m_ReflectionCameras[currentCamera] = reflectionCamera;
			}
	}

	// Given position/normal of the plane, calculates plane in camera space.
	Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 offsetPos = pos + normal * ClipPlaneOffset;
		Matrix4x4 m = cam.worldToCameraMatrix;
		Vector3 cpos = m.MultiplyPoint(offsetPos);
		Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
	}
	
	// Calculates reflection matrix around the given plane
	static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
		reflectionMat.m01 = (- 2F * plane[0] * plane[1]);
		reflectionMat.m02 = (- 2F * plane[0] * plane[2]);
		reflectionMat.m03 = (- 2F * plane[3] * plane[0]);
		
		reflectionMat.m10 = (- 2F * plane[1] * plane[0]);
		reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
		reflectionMat.m12 = (- 2F * plane[1] * plane[2]);
		reflectionMat.m13 = (- 2F * plane[3] * plane[1]);
		
		reflectionMat.m20 = (- 2F * plane[2] * plane[0]);
		reflectionMat.m21 = (- 2F * plane[2] * plane[1]);
		reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
		reflectionMat.m23 = (- 2F * plane[3] * plane[2]);
		
		reflectionMat.m30 = 0F;
		reflectionMat.m31 = 0F;
		reflectionMat.m32 = 0F;
		reflectionMat.m33 = 1F;
	}
}

