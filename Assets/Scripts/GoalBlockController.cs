using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.GameStatic;

public class GoalBlockController : MapObject {
	private LightningController l;
	public int goalNumber;
	// Use this for initialization
	void Start () {
		l = gameObject.GetComponent<LightningController>();
	}

	// Update is called once per frame
	void Update () {
		if (l.lightning == true) {
			goalFlag[goalNumber] = true;
		}
	}
}
