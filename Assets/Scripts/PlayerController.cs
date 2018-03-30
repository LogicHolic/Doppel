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


  public void PlayerMove(Vector3 direc) {
    MapPos nextPos = GetNextPos(nowPos, direc);
    nextObj = goMap[nextPos.floor, nextPos.x, nextPos.z];

    transform.localRotation = Quaternion.LookRotation(direc);
    animator.SetBool(key_walk, true);


    if(nextObj == null) {
      //現在地をnullに
      goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
      //移動先に自身を代入
      goMap[nextPos.floor, nextPos.x, nextPos.z] = gameObject;
      StartCoroutine(Move(direc));
      stayCnt = 0;
    }
    else if (nextObj.tag.Contains("Movable")) {
      GameObject moveBlock = goMap[nextPos.floor, nextPos.x, nextPos.z];
      BlockController b = moveBlock.GetComponent<BlockController>();
      MapPos nextnextPos = GetNextPos(nextPos, direc);
      GameObject nextnextObj = goMap[nextnextPos.floor, nextnextPos.x, nextnextPos.z];

      if (nextnextPos.ExceedRange() || nextnextObj == null || (!nextnextPos.ExceedRange() && nextnextObj.tag.Contains("Doppel"))) {
        b.BlockMove(direc);
        if (goMap[nextPos.floor, nextPos.x, nextPos.z] == null) {
          //現在地をnullに
          goMap[nowPos.floor, nowPos.x, nowPos.z] = null;
          //移動先に自身を代入
          goMap[nextPos.floor, nextPos.x, nextPos.z] = gameObject;
          //ブロック移動後移動先が空いているなら == ブロックが動けたなら　プレイヤーを動かす
          StartCoroutine(Move(direc));
          stayCnt = 0;
        }
      }
    }
    else if (nextObj.tag.Contains("Doppel")) {
      return;
    }
  }

  void MoveDoppels(Vector3 direc) {
    DoppelController d;
    for (int i = 0; i < doppelNum; i++) {
      if (doppels[i] == null) {
        continue;
      }
      d = doppels[i].GetComponent<DoppelController>();
      if (d.exist) {
        d.DoppelMove(direc);
      }
    }
  }

  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update () {
    stayCnt++;

    if (!isMoving && !gameOver) {
      animator.SetBool(key_walk, false);
      MapPos beneath = nowPos + new MapPos(-1, 0, 0);
      if (beneath.floor < 0 || goMap[beneath.floor, beneath.x, beneath.z] == null) {
        StartCoroutine(Fall());
        gameOver = true;
      }
      if (Input.GetKeyDown(KeyCode.A)) {
        PlayerMove(Vector3.left);
        MoveDoppels(Vector3.right);
      }
      if (Input.GetKeyDown(KeyCode.W)) {
        PlayerMove(Vector3.forward);
        MoveDoppels(Vector3.back);
      }
      if (Input.GetKeyDown(KeyCode.S)) {
        PlayerMove(Vector3.back);
        MoveDoppels(Vector3.forward);
      }
      if (Input.GetKeyDown(KeyCode.D)) {
        PlayerMove(Vector3.right);
        MoveDoppels(Vector3.left);
      }
    }
    animator.SetInteger(key_erapse, stayCnt);
  }
}
