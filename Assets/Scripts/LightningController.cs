using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;
using static Game.GameStatic;

public class LightningController : MonoBehaviour {
	public const int DURATION = 20;
	public Color currentColor;
	public Color lightColor;
	public Color normalColor;
	public bool isLightning;
	public bool lightning;
	public bool lightningSwitch;
	private bool isMoving;
	public bool connectAlways;
	public bool always = false;
	private List<MapPos> searchedList;

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
			if (always) {
				lightColor = new Color (1, 1, 1, 0);
			}
		} else if (gameObject.tag.Contains("Goal")) {
			objectTag = 'g';
			lightColor = new Color(0.8f, 0.8f, 0.2f, 0);
		} else if (gameObject.tag.Contains("Hard")) {
			objectTag = 'h';
			lightColor = new Color(1, 0, 0, 0);
			if (always) {
				lightColor = new Color (1, 1, 1, 0);
			}
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
			isMoving = b.isMoving;
		} else if (objectTag == 'g') {
			nowPos = g.nowPos;
		} else if (objectTag == 'h') {
			nowPos = h.nowPos;
		}

		if (!gameOver) {
			if (!isLightning && lightningSwitch && !lightning) {
				StartCoroutine(GradLightning(currentColor, lightColor, true));
			}
			if (!isLightning && !lightningSwitch && lightning) {
				StartCoroutine(GradLightning(currentColor, normalColor, false));
			}

			if (always) {
				searchedList = new List<MapPos>();
				ConnectAlwaysSearch(nowPos);
			}

			if (!always && !connectAlways) {
				currentColor = normalColor;
				SetLPColor(currentColor);
				lightning = false;
				isLightning = false;
				lightningSwitch = false;
			}
			if (!isLightning && lightning) {
				SpreadLightning();
			}

		}
		connectAlways = false;
	}

	void ConnectAlwaysSearch(MapPos pos) {
		GameObject nObj;
		MapPos n = new MapPos(-1, -1, -1);

		searchedList.Add(pos);
		//前後左右
		for (int i = 0; i < 4; i++) {
			if (objectTag == 'b') {
				n = b.GetNextPos(pos, d[i]);
			} else if (objectTag == 'g') {
				n = g.GetNextPos(pos, d[i]);
			} else if (objectTag == 'h') {
				n = h.GetNextPos(pos, d[i]);
			}
			nObj = goMap[n.floor, n.x, n.z];
			if (nObj != null && nObj.tag.Contains("Lightning") && !searchedList.Contains(n)) {
				LightningController l = nObj.GetComponent<LightningController>();
				//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
				l.connectAlways = true;
				ConnectAlwaysSearch(n);
			}
		}
		//上下
		if (pos.floor == 0) {
			n = new MapPos(pos.floor+1, pos.x, pos.z);
			nObj = goMap[pos.floor+1, pos.x, pos.z];
		} else {
			n = new MapPos(pos.floor-1, pos.x, pos.z);
			nObj = goMap[pos.floor-1, pos.x, pos.z];
		}
		if (nObj != null && nObj.tag.Contains("Lightning") && !searchedList.Contains(n)) {
			LightningController l = nObj.GetComponent<LightningController>();
			//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
			l.connectAlways = true;
			ConnectAlwaysSearch(n);
		}
	}
	void SpreadLightning() {
		GameObject nObj;
		//前後左右
		for (int i = 0; i < 4; i++) {
			MapPos n = new MapPos(-1, -1, -1);
			if (objectTag == 'b') {
				n = b.GetNextPos(nowPos, d[i]);
			} else if (objectTag == 'g') {
				n = g.GetNextPos(nowPos, d[i]);
			} else if (objectTag == 'h') {
				n = h.GetNextPos(nowPos, d[i]);
			}
			nObj = goMap[n.floor, n.x, n.z];
			if (nObj != null && nObj.tag.Contains("Lightning")) {
				LightningController l = nObj.GetComponent<LightningController>();
				//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
				if (!l.lightning && !l.lightningSwitch) {
					l.lightningSwitch = true;
				}
			}
		}
		//上下
		if (nowPos.floor == 0) {
			nObj = goMap[nowPos.floor+1, nowPos.x, nowPos.z];
		} else {
			nObj = goMap[nowPos.floor-1, nowPos.x, nowPos.z];
		}
		if (nObj != null && nObj.tag.Contains("Lightning")) {
			LightningController l = nObj.GetComponent<LightningController>();
			//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
			if (!l.lightning && !l.lightningSwitch) {
				l.lightningSwitch = true;
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
		lightningSwitch = l;
		isLightning = true;
		Color grad = (to - from)/DURATION;
		currentColor = from;
		for (int i = 0; i < DURATION; i++) {
			if (i == DURATION - 1) {
				currentColor = to;
				SetLPColor(currentColor);
				lightning = l;
				isLightning = false;
				yield break;
			}
			currentColor = currentColor + grad;
			SetLPColor(currentColor);
			yield return null;
		}
	}
}
