using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapStatic;

namespace Game {
  // mapPosを管理する構造体
  // int型のvectorみたいなもの
  // +演算子に対応

  public struct MapPos {
    public int floor;
    public int x;
    public int z;

    public MapPos(int Floor, int X, int Z) {
      floor = Floor;
      x = X;
      z = Z;
    }
    public static MapPos operator+ (MapPos mapPos, MapPos mapDirec)
    {
      mapPos.floor += mapDirec.floor;
      mapPos.x += mapDirec.x;
      mapPos.z += mapDirec.z;
      return mapPos;
    }
    public static bool operator== (MapPos m1, MapPos m2)
    {
      return m1.floor == m2.floor && m1.x == m2.x && m1.z == m2.z;
    }
    public static bool operator!= (MapPos m1, MapPos m2)
    {
      return !(m1 == m2);
    }
    public static bool operator== (MapPos m1, MapPos[] m2)
    {
      for (int i = 0; i < m2.Length; i++) {
        if (m1 == m2[i]) {
          return true;
        }
      }
      return false;
    }
    public static bool operator!= (MapPos m1, MapPos[] m2)
    {
      return !(m1 == m2);
    }
    public static Vector3 ToVector(MapPos mapPos) {
      return new Vector3(mapPos.floor, mapPos.x, mapPos.z);
    }
    public static MapPos ToMappos(Vector3 Vec) {
      MapPos mapPos;
      mapPos.floor = (int)Vec.x;
      mapPos.x = (int)Vec.y;
      mapPos.z = (int)Vec.z;
      return mapPos;
    }
    public bool ExceedRange() {
      return floor < 0 || floor >= floorSize || x < 0 || x >= mapSizeX || z < 0 || z >= mapSizeZ;
    }
  }

  public class GameStatic {
    public static bool gameOver = false;
    public static bool stageClear = false;
    public static bool doppelTouchPlayer = false;
    public static bool[] goalFlag;
  }

  //mapに関するstaticな変数を入れておく
  public class MapStatic {
    public static List<GameObject> doppels;
    public static int doppelNum;

    public static int mapSizeX;
    public static int floorSize;
    public static int mapSizeZ;

    //map生成の時にだけ使うmap
    //動かさない(のでconstつけてもいい)
    //以下マップ情報(予定も含む)
    // ブロックタイプ
    // 0　= nothing
    // 1 = hard
    // 2 = hard & lightning
    // 3 = hard & always
    // 4 = movable
    // 5 = movable & lightning
    // 6 = movable & always
    // 7 = hardIce
    // 8 = movableIce
    // 9 = gate
    // 20 = invisible
    // 100 = player
    // 200 = doppel

    //属性
    //最終的には分ける？
    // 10 = arrow
    // 99 = goal_switch

    // 向き
    // 最終的には分ける？
    // 50 = up
    // 51 = left
    // 52 = down
    // 53 = right
    // public static int[,,] map;
    public static int[,,] map = {
      {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1},
        {1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1},
        {1, 2, 2, 2, 2, 2, 1, 7, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 7, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 7, 1, 1, 1, 1, 1},
        {1, 1, 1, 7, 7, 7, 7, 0, 1, 1, 1, 1, 1},
        {1, 1, 1, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 7, 1, 0, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
      },
      {
        {100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0},
        {0, 0, 5, 9, 0, 5, 5, 9, 0, 0, 0, 0, 0},
        {0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 200},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0}
      }
    };

    //GameObjectを格納するmap
    //基本これを動かす
    public static GameObject[,,] goMap;

    //以下4つはmap座標とunity座標の対応づけメソッド
    public static Vector3 MapvecToUnivec(Vector3 Vec) {
      return new Vector3(Vec.z, Vec.x, -Vec.y);
    }

    public static Vector3 UnivecToMapvec(Vector3 Vec) {
      return new Vector3(Vec.y, -Vec.z, Vec.x);
    }

    public static Vector3 MapposToUnipos(MapPos mapPos) {
      return(MapvecToUnivec(MapPos.ToVector(mapPos)));
    }

    public static MapPos UniposToMappos(Vector3 unityPos) {
      return(MapPos.ToMappos(UnivecToMapvec(unityPos)));
    }
  }
}
