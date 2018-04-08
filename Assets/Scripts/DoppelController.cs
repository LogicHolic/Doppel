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
    nextObj = moMap[nextPos.floor, nextPos.x, nextPos.z];

    if (isViable(nextPos)) {
      animator.SetBool(key_walk, true);
      StartCoroutine(Move(direc));
    } else if (nextObj != null && nextObj.tag.Contains("Movable")) {
      GameObject moveBlock = nextObj;
      BlockController b = moveBlock.GetComponent<BlockController>();
      b.BlockMove(direc);
      stayCount = 0;
      // nextPos = GetNextPos(nowPos, direc);
      // if (isViable(nextPos)) {
      //   //ブロック移動後移動先が空いているなら == ブロックが動けたなら　プレイヤーを動かす
      //   StartCoroutine(Move(direc));
      //   stayCnt = 0;
      // }
    }
    else if (nextObj != null && nextObj.tag.Contains("Player")) {
      doppelTouchPlayer = true;
      animator.SetBool(key_walk, true);
      StartCoroutine(Move(direc));
    }
  }

  void Start()
  {
    direction = Vector3.forward;
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update () {
    stayCount++;
    if (!isMoving && doppelTouchPlayer) {
      gameOver = true;
    }

    if (!isMoving && exist) {
      animator.SetBool(key_walk, false);
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
        exist = false;
        StartCoroutine(Fall());
      }
    }
    animator.SetInteger(key_erapse, stayCount);
  }
}
