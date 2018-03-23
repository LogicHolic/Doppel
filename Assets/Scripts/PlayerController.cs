using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using static Map.MapStatic;

public class PlayerController : MovingObject {
  private Animator animator;
  private int stayCnt = 0;

  private const string key_noboru = "noboru";
  private const string key_clear = "clear";
  private const string key_walk = "walk";
  private const string key_erapse = "erapsed";

  public void PlayerMove(Vector3 direc) {
    MapPos nextPos = GetNextPos(playerPos, direc);
    nextObj = goMap[nextPos.floor, nextPos.x, nextPos.z];

    transform.localRotation = Quaternion.LookRotation(direc);
    animator.SetBool(key_walk, true);

    if(nextObj == null) {
      StartCoroutine(Move(playerPos, direc));
      playerPos = nextPos;
    } else {
      switch (nextObj.tag) {
        case "BaseBlock":
          break;
        case "MovableBlock":
          GameObject moveBlock = goMap[nextPos.floor, nextPos.x, nextPos.z];
          BlockController b = moveBlock.GetComponent<BlockController>();
          b.BlockMove(nextPos, direc);
          if (goMap[nextPos.floor, nextPos.x, nextPos.z] == null) {
            //ブロック移動後移動先が空いているなら == ブロックが動けたなら　プレイヤーを動かす
            StartCoroutine(Move(playerPos, direc));
            playerPos = nextPos;
            stayCnt = 0;
          }
          break;
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
    if (!isMoving) {
      animator.SetBool(key_walk, false);
      if (Input.GetKeyDown(KeyCode.A)) {
        PlayerMove(Vector3.left);
      }
      if (Input.GetKeyDown(KeyCode.W)) {
        PlayerMove(Vector3.forward);
      }
      if (Input.GetKeyDown(KeyCode.S)) {
        PlayerMove(Vector3.back);
      }
      if (Input.GetKeyDown(KeyCode.D)) {
        PlayerMove(Vector3.right);
      }
    }
    animator.SetInteger(key_erapse, stayCnt);
  }
}
