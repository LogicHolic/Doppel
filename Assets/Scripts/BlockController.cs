using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class BlockController : MovingObject {
	public void BlockMove(Vector3 direc) {
		MapPos nextPos = GetNextPos(nowPos, direc);

		if(isViable(nextPos)) {
			StartCoroutine(Move(direc));
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (!isMoving) {
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
				moMap[nowPos.floor, nowPos.x, nowPos.z] = null;
				StartCoroutine(Fall());
      }
		}
  }
}
