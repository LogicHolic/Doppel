using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

//Map上のすべてのオブジェクトに付加するスーパークラス
public class MapObject : MonoBehaviour {
  public MapPos nowPos;

  //移動先の座標を取得
  public MapPos GetNextPos(MapPos mapPos, Vector3 direc) {
    MapPos nextPos = mapPos + MapController.UniposToMappos(direc);
    return nextPos;
  }

  void Start()
  {
  }

  // Update is called once per frame
  void Update () {
  }
}
