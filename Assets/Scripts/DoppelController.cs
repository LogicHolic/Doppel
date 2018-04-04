using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;
using static Game.GameStatic;

public class DoppelController : MovingObject {
  private Animator animator;
  private int stayCnt = 0;
  public bool exist = true;

  private const string key_noboru = "noboru";
  private const string key_clear = "clear";
  private const string key_walk = "walk";
  private const string key_erapse = "erapsed";

  public void DoppelMove(Vector3 direc) {
    MapPos nextPos = GetNextPos(nowPos, direc);
    nextObj = goMap[nextPos.floor, nextPos.x, nextPos.z];

    transform.localRotation = Quaternion.LookRotation(direc);
    animator.SetBool(key_walk, true);

    GameObject g = goMap[nextPos.floor, nextPos.x, nextPos.z];
    if (isViable(g)) {
      StartCoroutine(Move(direc));
      stayCnt = 0;
    } else if (nextObj.tag.Contains("Movable")) {
      GameObject moveBlock = goMap[nextPos.floor, nextPos.x, nextPos.z];
      BlockController b = moveBlock.GetComponent<BlockController>();
      b.BlockMove(direc);
      if (goMap[nextPos.floor, nextPos.x, nextPos.z] == null) {
        //ブロック移動後移動先が空いているなら == ブロックが動けたなら　プレイヤーを動かす
        StartCoroutine(Move(direc));
        stayCnt = 0;
      }
    }
    else if (nextObj.tag.Contains("Player")) {
      doppelTouchPlayer = true;
      StartCoroutine(Move(direc));
      stayCnt = 0;
    }
  }

  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update () {
    stayCnt++;
    if (!isMoving && doppelTouchPlayer) {
      gameOver = true;
    }

    if (!isMoving) {
      animator.SetBool(key_walk, false);
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
        StartCoroutine(Fall());
        exist = false;
      }
    }
    animator.SetInteger(key_erapse, stayCnt);
  }
}
