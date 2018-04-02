using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gate_anim : MonoBehaviour {
	float weight = 0;
	bool anim_flag = true;

	SkinnedMeshRenderer renderer;
	int gateIndex;

	void Start () {
		renderer = GetComponent<SkinnedMeshRenderer>();
		gateIndex = renderer.sharedMesh.GetBlendShapeIndex("blendShape1.open3");
	}
	
	// Update is called once per frame
	void Update () {
			if (anim_flag) {
				weight++;
			} else {
				weight--;
			}
			if (weight > 100)anim_flag = !anim_flag;
			if (weight < 0)anim_flag = !anim_flag;

		renderer.SetBlendShapeWeight (gateIndex, weight);
	}
}