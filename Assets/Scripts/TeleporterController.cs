using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class TeleporterController : MapObject {
	// 1 = green
	public int color;
	public int portNum;

	private GameObject par1;
	private LightningController l;

	public bool objectIsOn;

	// Use this for initialization
	void Start () {
		par1 = GameObject.Find("par1");
		Deactivate();
		l = gameObject.GetComponent<LightningController>();
 	}

	// Update is called once per frame
	void Update () {
		objectIsOn = objectCheck();
		if (!objectIsOn) {
			goMap[nowPos.floor, nowPos.x, nowPos.z] = gameObject;
		}

		if (l.lightning) {
			Activate();
			if (objectIsOn && !objIsMoving(goMap[nowPos.floor, nowPos.x, nowPos.z])) {
				TeleportCheck();
			}
		} else {
			Deactivate();
		}
	}

	private bool objectCheck() {
		if (goMap[nowPos.floor, nowPos.x, nowPos.z] == null) {
			return false;
		}
		if (goMap[nowPos.floor, nowPos.x, nowPos.z] != gameObject) {
			return true;
		}
		return false;
	}

	void Activate() {
		par1.SetActive(true);
	}

	void Deactivate() {
		par1.SetActive(false);
	}

	bool objIsMoving (GameObject obj) {
		if (obj.tag.Contains("Movable")) {
			BlockController b = obj.GetComponent<BlockController>();
			return b.isMoving;
		} else if (obj.tag.Contains("Player")) {
			PlayerController p = obj.GetComponent<PlayerController>();
			return p.isMoving;
		} else if (obj.tag.Contains("Doppel")) {
			DoppelController d = obj.GetComponent<DoppelController>();
			return d.isMoving;
		}
		return false;
	}

	void TeleportCheck() {
		if (portNum == 0) {
			LightningController l = teleporters[1, color].gameObject.GetComponent<LightningController>();
			if (l.lightning) {
				MapPos to = teleporters[1, color].nowPos;
				GameObject toObj = goMap[to.floor, to.x, to.z];
				if (isViable(toObj)) {
					Teleport(to);
				}
			}
		} else {
			LightningController l = teleporters[0, color].gameObject.GetComponent<LightningController>();
			if (l.lightning) {
				MapPos to = teleporters[0, color].nowPos;
				GameObject toObj = goMap[to.floor, to.x, to.z];
				if (isViable(toObj)) {
					Teleport(to);
				}
			}
		}
	}

	void Teleport(MapPos to) {
		GameObject obj = goMap[nowPos.floor, nowPos.x, nowPos.z];
		goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
		goMap[to.floor, to.x, to.z] = obj;
		SetPos(obj, to);
  }

	private static void SetPos (GameObject obj, MapPos pos) {
		if (obj.tag.Contains("Movable")) {
			BlockController b = obj.GetComponent<BlockController>();
			b.nowPos = pos;
			b.gameObject.transform.position = b.ModifyPos(pos);
		} else if (obj.tag.Contains("Player")) {
			PlayerController p = obj.GetComponent<PlayerController>();
			p.nowPos = pos;
			p.gameObject.transform.position = p.ModifyPos(pos);
		} else if (obj.tag.Contains("Doppel")) {
			DoppelController d = obj.GetComponent<DoppelController>();
			d.nowPos = pos;
			d.gameObject.transform.position = d.ModifyPos(pos);
		}
	}
}
