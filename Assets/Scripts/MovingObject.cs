using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

//動かせるオブジェクトに付加するスーパークラス
public class MovingObject : MonoBehaviour {
  public const int MOVE_STEPS = 15;
  protected bool isMoving = false;
  protected GameObject thisObj;
  protected GameObject nextObj;
  public MapPos nowPos;

  //移動先の座標を取得
  protected MapPos GetNextPos(MapPos mapPos, Vector3 direc) {
    MapPos nextPos = mapPos + MapController.UniposToMappos(direc);
    return nextPos;
  }

  //最終的な移動先をmap上の動きのみから算出した座標に修正
  protected Vector3 ModifyPos(MapPos mapPos) {
    thisObj = this.gameObject;
    Vector3 ModifiedPos;
    //playerは高さが半分ずれているので処理を分ける
    if (thisObj.tag == "Player") {
      ModifiedPos = MapController.MapposToUnipos(mapPos) - new Vector3(0, 0.5f, 0);
    } else {
      ModifiedPos = MapController.MapposToUnipos(mapPos);
    }
    return ModifiedPos;
  }

  //オブジェクトが動く時の共通処理はここに書く
  protected IEnumerator Move(Vector3 direc) {
    isMoving = true;
    transform.localRotation = Quaternion.LookRotation(direc);
    nowPos = GetNextPos(nowPos, direc);

    for (int i = 0; i < MOVE_STEPS; i++) {
      transform.Translate(Vector3.forward / MOVE_STEPS);
      yield return null;
    }

    transform.position = ModifyPos(nowPos);
    isMoving = false;
  }

  protected IEnumerator Fall() {
    isMoving = true;
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
