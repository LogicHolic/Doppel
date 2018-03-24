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
    public static bool[] goalFlag;
  }

  //mapに関するstaticな変数を入れておく
  public class MapStatic {
    public static MapPos playerPos;
    public static MapPos[] doppelPos;
    public const int doppelNum = 3;

    public static int mapSizeX;
    public static int floorSize;
    public static int mapSizeZ;


    //map生成の時にだけ使うmap
    //動かさない(のでconstつけてもいい)
    //以下マップ情報(予定も含む)
    // ブロックタイプ
    // -1 = hole
    // 0　= nothing
    // 1 = hard block
    // 2 = movable
    // 3 = movable & lightning
    // 4 = ice
    // 5 = stair

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
    public static int[,,] map;
    // public static int[,,] map = {
    //   {
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    //   },
    //   {
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 3, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 99, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 99, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 2, 0, 99, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    //   }
    // };

    //GameObjectを格納するmap
    //基本これを動かす
    public static GameObject[,,] goMap;
  }
}
