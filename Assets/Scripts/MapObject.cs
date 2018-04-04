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

  protected bool isViable(GameObject g) {
    if (g == null) {
      return true;
    }
    if (g.tag.Contains("Gate")) {
      LightningController l = g.GetComponent<LightningController>();
      if (l.lightning) {
        return true;
      }
    }
    return false;
  }
}
