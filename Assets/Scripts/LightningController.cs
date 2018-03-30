using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;
using static Game.GameStatic;

public class LightningController : MonoBehaviour {
	private Renderer rend;
	public const int DURATION = 30;
	public Color currentColor;
	public Color lightColor;
	public bool lightning;
	Vector3[] d = new Vector3[]{Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

	private BlockController b;
	private HardObjectController h;
	private GoalBlockController g;
	private MapPos nowPos;
	private char objectTag;

	// Use this for initialization
	void Start () {
		b = transform.root.gameObject.GetComponent<BlockController>();
		h = transform.root.gameObject.GetComponent<HardObjectController>();
		g = transform.root.gameObject.GetComponent<GoalBlockController>();
		if (transform.root.gameObject.tag.Contains("Movable")) {
			objectTag = 'b';
			lightColor = new Color(1, 1, 1, 0);
		} else if (gameObject.tag.Contains("Goal")) {
			objectTag = 'g';
			lightColor = new Color(0.8f, 0.8f, 0.2f, 0);
		}

		rend = GetComponent<Renderer>();
		if (lightning) {
			currentColor = lightColor;
		} else {
			currentColor = new Color(0, 0, 0, 0);
		}
		rend.material.SetColor("_EmissionColor",currentColor);
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
		for (int i = 0; i < 4; i++) {
			MapPos n = new MapPos(-1, -1, -1);
			if (objectTag == 'b') {
				n = b.GetNextPos(nowPos, d[i]);
			} else if (objectTag == 'g') {
				n = g.GetNextPos(nowPos, d[i]);
			}
			GameObject nObj = goMap[n.floor, n.x, n.z];
			if (nObj != null && nObj.tag.Contains("Lightning")) {
				GameObject lightningSphere = nObj.transform.Find("LightningSphere").gameObject;
				LightningController l = lightningSphere.GetComponent<LightningController>();
				//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
				if (!l.lightning) {
					StartCoroutine(l.GradLightning(l.currentColor, l.lightColor, true));
				}
			}
		}
	}

	public IEnumerator GradLightning(Color from, Color to, bool l) {
		lightning = l;
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
