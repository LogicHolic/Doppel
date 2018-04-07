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

  public GameObject behind;
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
      yield return null;
    }
    transform.position = ModifyPos(nowPos);
    isMoving = false;

    //氷によるさらなる移動があるか調べる
    MapPos beneath = nowPos + new MapPos(-1, 0, 0);
    MapPos nextPos = GetNextPos(nowPos, direc);
    GameObject uObj = goMap[beneath.floor, beneath.x, beneath.z];
    //"地面が空でないかつ次のマスが移動可能"かつ"地面が氷または自身が氷"
    if ( (uObj != null  && isViable(nextPos))
      && (gameObject.tag.Contains("Ice") || uObj.tag.Contains("Ice")) ){
      StartCoroutine(Move(direc));
    }
  }


  public bool haveBehind() {
    return behind != null;
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
