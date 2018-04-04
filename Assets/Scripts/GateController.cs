using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.MapStatic;

public class GateController : MapObject {
	private LightningController l;

	public SkinnedMeshRenderer renderer;
	private int gateIndex;
	private float weight = 0;

	private PlayerController p;
	private DoppelController[] d;

	public bool playerIsOn;

	void Start () {
		renderer = GetComponent<SkinnedMeshRenderer>();
		gateIndex = renderer.sharedMesh.GetBlendShapeIndex("blendShape1.open3");
		l = GetComponent<LightningController>();
		p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		d = new DoppelController[doppels.Count];
		for (int i = 0; i < doppels.Count; i++) {
			d[i] = doppels[i].GetComponent<DoppelController>();
		}
	}

	// Update is called once per frame
	void Update () {
		playerIsOn = playerCheck();
		if (!playerIsOn) {
			goMap[nowPos.floor, nowPos.x, nowPos.z] = gameObject;
		}
	}

	private bool playerCheck() {
		if (nowPos == p.nowPos) {
			return true;
		}
		for (int i = 0; i < d.Length; i++) {
			if (nowPos == d[i].nowPos) {
				return true;
			}
		}
		return false;
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
