using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class BlockController : MovingObject {
	public const int DURATION = 30;
	private Renderer rend;
	public Color currentColor;
	public bool lightning = false;

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

			Vector3[] d = new Vector3[]{Vector3.forward, Vector3.left, Vector3.right, Vector3.back};
			for (int i = 0; i < 4; i++) {
				MapPos n = GetNextPos(nowPos, d[i]);
				GameObject nObj = goMap[n.floor, n.x, n.z];

				if (nObj != null && nObj.tag == "MovableBlock") {
					BlockController b = nObj.GetComponent<BlockController>();
					//オブジェクトが光っているとき，周りに光っていないオブジェクトがあったら光らせる
					if (lightning && !b.lightning) {
						Color c = new Color(1,1,1,0);
						StartCoroutine(b.GradLightning(b.currentColor, c, true));
					}
					//オブジェクトが光っていないとき，周りに光っているオブジェクトがあったら自身を光らせる
					else if (!lightning && b.lightning) {
						Color c = new Color(1,1,1,0);
						StartCoroutine(GradLightning(currentColor, c, true));
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		if (lightning) {
			currentColor = new Color(1, 1, 1, 0);
			rend.material.SetColor("_EmissionColor",currentColor);
		} else {
			currentColor = new Color(0, 0, 0, 0);
			rend.material.SetColor("_EmissionColor",currentColor);
		}
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
