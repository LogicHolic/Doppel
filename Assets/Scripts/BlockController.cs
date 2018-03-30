using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class BlockController : MovingObject {
	public void BlockMove(Vector3 direc) {
		thisObj = this.gameObject;
		MapPos nextPos = GetNextPos(nowPos, direc);
		nextObj = goMap[nextPos.floor, nextPos.x, nextPos.z];

		if(nextObj == null) {
			//現在地をnullに
			goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
			//移動先に自身を代入
			goMap[nextPos.floor, nextPos.x, nextPos.z] = thisObj;
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
				goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
				StartCoroutine(Fall());
      }
		}
  }
}
