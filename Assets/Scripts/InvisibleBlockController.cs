using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlockController : MapObject {
	private MeshRenderer mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshRenderer>();
		mesh.enabled = false;
	}

	// Update is called once per frame
	void Update () {

	}
}
