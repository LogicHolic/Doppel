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
		if (moMap[nowPos.floor, nowPos.x, nowPos.z] != null) {
			objectIsOn = true;
		} else {
			objectIsOn = false;
		}

		if (l.lightning) {
			Activate();
			if (objectIsOn && !objIsMoving(moMap[nowPos.floor, nowPos.x, nowPos.z])) {
				TeleportCheck();
			}
		} else {
			Deactivate();
		}
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

	bool ObjIsTeleported (GameObject obj) {
		if (obj.tag.Contains("Movable")) {
			BlockController b = obj.GetComponent<BlockController>();
			return b.isTeleported;
		} else if (obj.tag.Contains("Player")) {
			PlayerController p = obj.GetComponent<PlayerController>();
			return p.isTeleported;
		} else if (obj.tag.Contains("Doppel")) {
			DoppelController d = obj.GetComponent<DoppelController>();
			return d.isTeleported;
		}
		return false;
	}

	void SetIsTeleported (GameObject obj) {
		if (obj.tag.Contains("Movable")) {
			BlockController b = obj.GetComponent<BlockController>();
			b.isTeleported = true;
		} else if (obj.tag.Contains("Player")) {
			PlayerController p = obj.GetComponent<PlayerController>();
			p.isTeleported = true;
		} else if (obj.tag.Contains("Doppel")) {
			DoppelController d = obj.GetComponent<DoppelController>();
			d.isTeleported = true;
		}
	}

	void TeleportCheck() {
		if (portNum == 0) {
			LightningController l = teleporters[1, color].gameObject.GetComponent<LightningController>();
			if (l.lightning) {
				MapPos to = teleporters[1, color].nowPos;
				if (isViable(to)) {
					Teleport(to);
				}
			}
		} else {
			LightningController l = teleporters[0, color].gameObject.GetComponent<LightningController>();
			if (l.lightning) {
				MapPos to = teleporters[0, color].nowPos;
				if (isViable(to)) {
					Teleport(to);
				}
			}
		}
	}

	void Teleport(MapPos to) {
		GameObject obj = moMap[nowPos.floor, nowPos.x, nowPos.z];
		if (!ObjIsTeleported(obj)) {
			moMap[nowPos.floor, nowPos.x, nowPos.z] = null;
			moMap[to.floor, to.x, to.z] = obj;
			SetPos(obj, to);
			SetIsTeleported(obj);
		}
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
