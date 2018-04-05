using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class TeleporterController : MapObject {
	public int portNum;
	public bool active;

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
		active = true;
	}

	void Deactivate() {
		par1.SetActive(false);
		active = false;
	}
}
