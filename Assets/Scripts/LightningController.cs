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
	//光りつつあるor消えつつある
	public bool isLightning;
	//光っている
	public bool lightning;
	//光るという指令or消えるという指令を受けている
	public bool lightningSwitch;
	private bool isMoving;
	//playerまたはdoppelが乗っている

	public bool connectAlways;
	public bool always = false;
	private List<MapPos> searchedList;

	Vector3[] d = new Vector3[]{Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

	private Transform lightningPart;
	private BlockController b;
	private HardObjectController h;
	private GateController g;
	private TeleporterController t;
	private MapPos nowPos;
	private char objectTag;

	// Use this for initialization
	void Start () {
		b = gameObject.GetComponent<BlockController>();
		h = gameObject.GetComponent<HardObjectController>();
		g = gameObject.GetComponent<GateController>();
		t = gameObject.GetComponent<TeleporterController>();
		if (gameObject.tag.Contains("Movable")) {
			objectTag = 'b';
			lightColor = new Color(1, 0, 0, 0);
			if (always) {
				lightColor = new Color (1, 1, 1, 0);
			}
		} else if (gameObject.tag.Contains("Gate")) {
			objectTag = 'g';
		} else if (gameObject.tag.Contains("Hard")) {
			objectTag = 'h';
			lightColor = new Color(1, 0, 0, 0);
			if (always) {
				lightColor = new Color (1, 1, 1, 0);
			}
		} else if (gameObject.tag.Contains("Teleporter")) {
			objectTag = 't';
		}
		normalColor = new Color(0.1f, 0.1f, 0.1f, 0);

		lightningPart = transform.Find("LightningPart");

		if (objectTag != 'g' && objectTag != 't') {
			if (lightning) {
				currentColor = lightColor;
			} else {
				currentColor = normalColor;
			}
			SetLPColor(currentColor);
		}
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
		} else if (objectTag == 't') {
			nowPos = t.nowPos;
		}

		if (!gameOver) {
			if (objectTag == 'g' && g.objectIsOn) {
				connectAlways = true;
				lightningSwitch = true;
				lightning = true;
			}

			if (!isLightning && lightningSwitch && !lightning) {
				if (objectTag == 'g') {
					if (!g.objectIsOn) {
						StartCoroutine(g.GateOpen());
					}
				} else if (objectTag == 't') {
					lightning = true;
				} else {
					StartCoroutine(GradLightning(currentColor, lightColor, true));
				}
			}
			if (!isLightning && !lightningSwitch && lightning) {
				if (objectTag == 'g') {
					if (!g.objectIsOn) {
						StartCoroutine(g.GateClose());
					}
				} else if (objectTag == 't') {
					lightning = false;
				} else {
					StartCoroutine(GradLightning(currentColor, normalColor, false));
				}
			}
			//alwaysとつながっているすべてのオブジェクトのconnectAlwaysをtrueに
			if (always) {
				searchedList = new List<MapPos>();
				ConnectAlwaysSearch(nowPos);
			}

			if (!always && !connectAlways) {
				if (objectTag != 'g' && objectTag != 't') {
					currentColor = normalColor;
					SetLPColor(currentColor);
					lightning = false;
					isLightning = false;
					lightningSwitch = false;
				} else {
					lightningSwitch = false;
				}
			}
			if (!isLightning && lightning && objectTag != 'g' && objectTag != 't') {
				SpreadLightning();
			}

			//毎フレームステータスを更新するためfalseに
			connectAlways = false;
		}
	}

	void ConnectAlwaysSearch(MapPos pos) {
		GameObject nObj;
		MapPos n = new MapPos(-1, -1, -1);

		searchedList.Add(pos);
		LightningController l;
		//前後左右
		for (int i = 0; i < 4; i++) {
			if (objectTag == 'b') {
				n = b.GetNextPos(pos, d[i]);
			} else if (objectTag == 'g') {
				n = g.GetNextPos(pos, d[i]);
			} else if (objectTag == 'h') {
				n = h.GetNextPos(pos, d[i]);
			}
			nObj = EntityOrBehind(goMap[n.floor, n.x, n.z]);
			if (nObj != null && nObj.tag.Contains("Lightning") && !searchedList.Contains(n)) {
				l = nObj.GetComponent<LightningController>();
				//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
				l.connectAlways = true;
				ConnectAlwaysSearch(n);
			}
		}
		//上下
		if (pos.floor == 0) {
			n = new MapPos(pos.floor+1, pos.x, pos.z);
			nObj = EntityOrBehind(goMap[pos.floor+1, pos.x, pos.z]);
		} else {
			n = new MapPos(pos.floor-1, pos.x, pos.z);
			nObj = EntityOrBehind(goMap[pos.floor-1, pos.x, pos.z]);
		}
		if (nObj != null && nObj.tag.Contains("Lightning") && !searchedList.Contains(n)) {
			l = nObj.GetComponent<LightningController>();
			//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
			l.connectAlways = true;
			ConnectAlwaysSearch(n);
		}
	}

	GameObject EntityOrBehind (GameObject obj) {
		if (obj == null) {
			return null;
		}
		if (obj.tag.Contains("Lightning")) {
			return obj;
		}  else if (obj.tag.Contains("Movable")) {
			BlockController b = obj.GetComponent<BlockController>();
			return b.behind;
		} else if (obj.tag.Contains("Player")) {
			PlayerController p = obj.GetComponent<PlayerController>();
			return p.behind;
		} else if (obj.tag.Contains("Doppel")) {
			DoppelController d = obj.GetComponent<DoppelController>();
			return d.behind;
		}
		return null;
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
