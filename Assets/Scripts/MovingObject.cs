using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

//動かせるオブジェクトに付加するスーパークラス
public class MovingObject : MapObject {
  public bool isMoving = false;
  protected GameObject thisObj;
  protected GameObject nextObj;

  //最後に動き終わってからの経過フレーム
  protected int stayCount = 0;

  public bool isTeleported;

  //最終的な移動先をmap上の動きのみから算出した座標に修正
  public Vector3 ModifyPos(MapPos mapPos) {
    Vector3 ModifiedPos;
    //playerは高さが半分ずれているので処理を分ける
    if (gameObject.tag == "Player" || gameObject.tag == "Doppel" || gameObject.tag.Contains("Teleporter")) {
      ModifiedPos = MapposToUnipos(mapPos) - new Vector3(0, 0.5f, 0);
    } else {
      ModifiedPos = MapposToUnipos(mapPos);
    }
    return ModifiedPos;
  }

  //オブジェクトが動く時の共通処理はここに書く
  protected IEnumerator Move(Vector3 direc, int MOVE_STEPS = 15) {
    isMoving = true;
    isTeleported = false;
    transform.localRotation = Quaternion.LookRotation(direc);

    //現在地をnullに
    moMap[nowPos.floor, nowPos.x, nowPos.z] = null;
    //位置更新
    nowPos = GetNextPos(nowPos, direc);
    //移動先に自身を代入
    moMap[nowPos.floor, nowPos.x, nowPos.z] = gameObject;

    for (int i = 0; i < MOVE_STEPS; i++) {
      transform.Translate(Vector3.forward / MOVE_STEPS);
      stayCount = 0;
      yield return null;
    }
    transform.position = ModifyPos(nowPos);
    isMoving = false;

    //氷によるさらなる移動があるか調べる
    MapPos beneath = nowPos + new MapPos(-1, 0, 0);
    GameObject uObj = goMap[beneath.floor, beneath.x, beneath.z];
    MapPos nextPos = GetNextPos(nowPos, direc);
    //"地面が空でないかつ次のマスが移動可能"かつ"地面が氷または自身が氷"
    bool move = false;
    if ( (uObj != null  && isViable(nextPos))
    && (gameObject.tag.Contains("Ice") || uObj.tag.Contains("Ice")) ){
      if (gameObject.tag.Contains("Ice")) {
        move = true;
        GameObject obj = goMap[nowPos.floor, nowPos.x, nowPos.z];
        if (obj != null && obj.tag.Contains("Teleporter")) {
          TeleporterController t = obj.GetComponent<TeleporterController>();
          LightningController l = obj.GetComponent<LightningController>();
          if (l.lightning) {
            if (t.portNum == 0) {
              LightningController l1 = teleporters[1, t.color].gameObject.GetComponent<LightningController>();
              if (l1.lightning) {
                move = false;
              }
            } else {
              LightningController l0 = teleporters[0, t.color].gameObject.GetComponent<LightningController>();
              if (l0.lightning) {
                move = false;
              }
            }
          }
        }
      } else {
        move = true;
      }
    }
    if (move) {
      StartCoroutine(Move(direc));
    }
  }

  protected IEnumerator Fall(int MOVE_STEPS = 15) {
    isMoving = true;
    moMap[nowPos.floor, nowPos.x, nowPos.z] = null;
    for (int i = 0; i < MOVE_STEPS * 10; i++) {
      transform.Translate(Vector3.down * 15 / (MOVE_STEPS * 10));
      yield return null;
    }
    GameObject.DestroyImmediate(gameObject);
  }


  void Start()
  {
  }

  // Update is called once per frame
  void Update () {
  }
}
