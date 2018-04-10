using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLLaser : MonoBehaviour {

	public Material lineMaterial;
	// static void CreateLineMaterial(){
	// 	if (!lineMaterial)
	// 	{
	// 		// Unity has a built-in shader that is useful for drawing
	// 		// simple colored things.
	// 		Shader shader = Shader.Find("Custom/LaserShader");
	// 		// Shader shader = Shader.Find("Hidden/Internal-Colored");
	// 		lineMaterial = new Material(shader);
	// 		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
	// 		// Turn on alpha blending
	// 		lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
	// 		lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
	// 		// Turn backface culling off
	// 		lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
	// 		// Turn off depth writes
	// 		lineMaterial.SetInt("_ZWrite", 0);
	// 	}
	// }

	void OnRenderObject () {
		// CreateLineMaterial();
		// Apply the line material
		lineMaterial.SetPass(0);
    GL.PushMatrix ();
    GL.MultMatrix (transform.localToWorldMatrix);
    GL.Begin (GL.LINES);
    GL.Vertex3 (0f, 0f, 0f);
    GL.Vertex3 (10f, 0f, 10f);
    GL.End ();
    GL.PopMatrix ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
