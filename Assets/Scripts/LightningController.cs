using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;
using static Game.GameStatic;

public class LightningController : MonoBehaviour {
	public const int DURATION = 30;
	public Color currentColor;
	public Color lightColor;
	public Color normalColor;
	public bool lightning;
	Vector3[] d = new Vector3[]{Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

	private Transform lightningPart;
	private BlockController b;
	private HardObjectController h;
	private GoalBlockController g;
	private MapPos nowPos;
	private char objectTag;

	// Use this for initialization
	void Start () {
		b = gameObject.GetComponent<BlockController>();
		h = gameObject.GetComponent<HardObjectController>();
		g = gameObject.GetComponent<GoalBlockController>();
		if (gameObject.tag.Contains("Movable")) {
			objectTag = 'b';
			lightColor = new Color(1, 0, 0, 0);
		} else if (gameObject.tag.Contains("Goal")) {
			objectTag = 'g';
			lightColor = new Color(0.8f, 0.8f, 0.2f, 0);
		}
		normalColor = new Color(0.1f, 0.1f, 0.1f, 0);

		lightningPart = transform.Find("LightningPart");

		if (lightning) {
			currentColor = lightColor;
		} else {
			currentColor = normalColor;
		}
		SetLPColor(currentColor);
	}

	// Update is called once per frame
	void Update () {
		if (objectTag == 'b') {
			nowPos = b.nowPos;
		} else if (objectTag == 'g') {
			nowPos = g.nowPos;
		}
		if (!gameOver && lightning) {
			SpreadLightning();
		}
	}

	void SpreadLightning() {
		GameObject nObj;
		for (int i = 0; i < 4; i++) {
			MapPos n = new MapPos(-1, -1, -1);
			if (objectTag == 'b') {
				n = b.GetNextPos(nowPos, d[i]);
			} else if (objectTag == 'g') {
				n = g.GetNextPos(nowPos, d[i]);
			}
			nObj = goMap[n.floor, n.x, n.z];
			if (nObj != null && nObj.tag.Contains("Lightning")) {
				LightningController l = nObj.GetComponent<LightningController>();
				//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
				if (!l.lightning) {
					StartCoroutine(l.GradLightning(l.currentColor, l.lightColor, true));
				}
			}
		}
		if (nowPos.floor == 0) {
			nObj = goMap[nowPos.floor+1, nowPos.x, nowPos.z];
		} else {
			nObj = goMap[nowPos.floor-1, nowPos.x, nowPos.z];
		}
		if (nObj != null && nObj.tag.Contains("Lightning")) {
			LightningController l = nObj.GetComponent<LightningController>();
			//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
			if (!l.lightning) {
				StartCoroutine(l.GradLightning(l.currentColor, l.lightColor, true));
			}
		}
	}

	void SetLPColor(Color c) {
		foreach (Transform child in lightningPart) {
			Renderer rend = child.gameObject.GetComponent<Renderer>();
			rend.material.SetColor("_EmissionColor",c);
		}
	}

	public IEnumerator GradLightning(Color from, Color to, bool l) {
		lightning = l;
		Color grad = (to - from)/DURATION;
		Color prev = from;
		for (int i = 0; i < DURATION; i++) {
			if (i == DURATION - 1) {
				SetLPColor(to);
				yield break;
			}
			Color c = prev + grad;
			SetLPColor(c);
			prev = c;
			yield return null;
		}
	}
}
