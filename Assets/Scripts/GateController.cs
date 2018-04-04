using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateController : MapObject {
	float weight = 0;
	LightningController l;

	public SkinnedMeshRenderer renderer;
	int gateIndex;

	void Start () {
		renderer = GetComponent<SkinnedMeshRenderer>();
		gateIndex = renderer.sharedMesh.GetBlendShapeIndex("blendShape1.open3");
		l = GetComponent<LightningController>();
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator GateOpen(float speed = 2) {
		l.lightningSwitch = true;
		l.isLightning = true;

		for (; weight <= 100; weight+= speed) {
			renderer.SetBlendShapeWeight (gateIndex, weight);
			yield return null;
		}
		weight = 100;
		renderer.SetBlendShapeWeight (gateIndex, weight);
		l.isLightning = false;
		l.lightning = true;
	}

	public IEnumerator GateClose(float speed = 2) {
		l.lightningSwitch = false;
		l.isLightning = true;
		l.lightning = false;

		for (; weight >= 0; weight-= speed) {
			renderer.SetBlendShapeWeight (gateIndex, weight);
			yield return null;
		}
		weight = 0;
		renderer.SetBlendShapeWeight (gateIndex, weight);
		l.isLightning = false;
	}
}
