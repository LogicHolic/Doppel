using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using static Map.MapStatic;

public class BlockController : MovingObject {
	private const int DURATION = 20;
	private Renderer rend;
	public bool lightning = false;

	public void BlockMove(MapPos nowPos, Vector3 direc) {
		thisObj = this.gameObject;
		MapPos nextPos = GetNextPos(nowPos, direc);
		nextObj = goMap[nextPos.floor, nextPos.x, nextPos.z];

		if(nextObj == null) {
			//現在地をnullに
			goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
			//移動先に自身を代入
			goMap[nextPos.floor, nextPos.x, nextPos.z] = thisObj;
			StartCoroutine(Move(nowPos, direc));
		}
	}

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		if (lightning) {
			Color c = new Color(1, 1, 1, 0);
			rend.material.SetColor("_EmissionColor",c);
		}
	}

	// Update is called once per frame
	void Update () {

  }

	private IEnumerator GradLightning(Color from, Color to) {
		Color grad = (to - from)/DURATION;
		Color prev = from;
		for (int i = 0; i < DURATION; i++) {
			if (i == DURATION - 1) {
				rend.material.SetColor("_EmissionColor",to);
				yield break;
			}
			Color c = prev + grad;
			rend.material.SetColor("_EmissionColor",c);
			prev = c;
			yield return null;
		}
	}
}
