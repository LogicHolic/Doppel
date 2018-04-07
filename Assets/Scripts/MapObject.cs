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
    MapPos nextPos = mapPos + UniposToMappos(direc);
    return nextPos;
  }

  void Start()
  {
  }

  // Update is called once per frame
  void Update () {
  }

  protected bool isViable(MapPos pos) {
    GameObject g = goMap[pos.floor, pos.x, pos.z];
    GameObject m = moMap[pos.floor, pos.x, pos.z];
    bool b1 = false, b2 = false;
    if (g == null  || g.tag.Contains("Teleporter")) {
      b1 = true;
    } else if (g.tag.Contains("Gate")) {
      LightningController l = g.GetComponent<LightningController>();
      if (l.lightning) {
        b1 = true;
      }
    }
    if (m == null) {
      b2 = true;
    }
    return b1 && b2;
  }
}
