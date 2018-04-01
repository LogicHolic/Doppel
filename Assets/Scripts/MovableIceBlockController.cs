using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapStatic;
using Game;

public class MovableIceBlockController : MovingObject {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (!isMoving) {
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
				goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
				StartCoroutine(Fall());
      }
		}
	}
}
