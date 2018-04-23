using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;
using static Game.GameStatic;

public class PlayerController : MovingObject {
  private Animator animator;
  private int stayCnt = 0;

  private const string key_noboru = "noboru";
  private const string key_clear = "clear";
  private const string key_walk = "walk";
  private const string key_erapse = "erapsed";


  public bool PlayerMove(Vector3 direc) {
    MapPos nextPos = GetNextPos(nowPos, direc);
    nextObj = moMap[nextPos.floor, nextPos.x, nextPos.z];

    if (direc != direction) {
      transform.localRotation = Quaternion.LookRotation(direc);
      direction = direc;
      stayCount = 0;
      return false;
    }
    direction = direc;
    if (isViable(nextPos)) {
      animator.SetBool(key_walk, true);
      //現在地をnullに
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
    return true;
  }

  void MoveDoppels(Vector3 direc) {
    DoppelController d;
    for (int i = 0; i < doppels.Count; i++) {
      if (doppels[i] == null) {
        continue;
      }
      d = doppels[i].GetComponent<DoppelController>();
      if (d.exist) {
        d.DoppelMove(direc);
      }
    }
  }

  bool isDoppelMoving () {
    DoppelController d;
    bool moving = false;
    for (int i = 0; i < doppels.Count; i++) {
      if (doppels[i] == null) {
        continue;
      }
      d = doppels[i].GetComponent<DoppelController>();
      if (d.exist && d.isMoving) {
        moving = true;
        break;
      }
    }
    return moving;
  }

  void Start()
  {
    direction = Vector3.forward;
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update () {
    stayCount++;
    if (stayCount >= 100000) {
      stayCount = 100;
    }
    if (!isMoving) {
      animator.SetBool(key_walk, false);
    }
    if (!isMoving && !isDoppelMoving() && !gameOver && stayCount >= 8) {
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
        StartCoroutine(Fall());
        gameOver = true;
      }
      else if (Input.GetKey(KeyCode.A)) {
        if (PlayerMove(Vector3.left))
          MoveDoppels(Vector3.right);
      }
      else if (Input.GetKey(KeyCode.W)) {
        if (PlayerMove(Vector3.forward))
          MoveDoppels(Vector3.back);
      }
      else if (Input.GetKey(KeyCode.S)) {
        if (PlayerMove(Vector3.back))
          MoveDoppels(Vector3.forward);
      }
      else if (Input.GetKey(KeyCode.D)) {
        if (PlayerMove(Vector3.right))
          MoveDoppels(Vector3.left);
      }
      else if (Input.GetKeyDown(KeyCode.F)) {
        RotateObject();
      }
    }
    animator.SetInteger(key_erapse, stayCount);
  }
  void RotateObject() {
    MapPos n = GetNextPos(nowPos, direction);
    GameObject nObj1 = goMap[n.floor, n.x, n.z];
    GameObject nObj2 = moMap[n.floor, n.x, n.z];
    if (nObj1 != null && nObj1.tag.Contains("Rotatable")) {
      LightningController l = nObj1.GetComponent<LightningController>();
      BlockController b = nObj1.GetComponent<BlockController>();
      b.Rotate();
      l.Rotate();
    }
    if (nObj2 != null && nObj2.tag.Contains("Rotatable")) {
      LightningController l = nObj2.GetComponent<LightningController>();
      BlockController b = nObj2.GetComponent<BlockController>();
      b.Rotate();
      l.Rotate();
    }
  }
}
