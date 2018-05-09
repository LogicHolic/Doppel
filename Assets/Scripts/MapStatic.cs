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
    public static bool testPlay = false;
    public static bool gameOver = false;
    public static bool stageClear = false;
    public static bool stageEdit = false;
    public static bool doppelTouchPlayer = false;
    public static bool[] goalFlag;
  }

  //mapに関するstaticな変数を入れておく
  public class MapStatic {
    public static List<GameObject> doppels;
    public static TeleporterController[,] teleporters;

    public static int mapSizeX;
    public static int floorSize;
    public static int mapSizeZ;


    //map生成の時にだけ使うmap
    //動かさない(のでconstつけてもいい)
    //以下マップ情報(予定も含む)
    // ブロックタイプ
    // 0　= nothing
    // 1 = hard
    // 2 = hardPower
    // 3 = hardIce
    // 11 = hardLightning(+)
    // 12 = hardLightning(-)
    // 13 = hardLightning(|)
    // 14 = hardLightning(└)
    // 15 = hardLightning(┌)
    // 16 = hardLightning(┐)
    // 17 = hardLightning(┘)
    // 31 = hardLaser(fr)
    // 32 = hardLaser(fl)
    // 33 = hardLaser(rr)
    // 34 = hardLaser(rl)
    // 35 = hardLaser(br)
    // 36 = hardLaser(bl)
    // 37 = hardLaser(lr)
    // 38 = hardLaser(ll)
    // 51 = movable
    // 52 = movablePower
    // 53 = movableIce
    // 61 = movableLightning(+)
    // 62 = movableLightning(-)
    // 63 = movableLightning(|)
    // 64 = movableLightning(└)
    // 65 = movableLightning(┌)
    // 66 = movableLightning(┐)
    // 67 = movableLightning(┘)
    // 81 = movableLaser(fr)
    // 82 = movableLaser(fl)
    // 83 = movableLaser(rr)
    // 84 = movableLaser(rl)
    // 85 = movableLaser(br)
    // 86 = movableLaser(bl)
    // 87 = movableLaser(lr)
    // 88 = movableLaser(ll)
    // 100 = gate
    // 101 = battery
    // 110 = teleporterGreen
    // 111 = teleporterRed
    // 112 = teleporterBlue
    // 200 = player
    // 201 = doppel

    // public static int[,,] map;
    public static int[,,] map = {
      {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1},
        {1, 1, 1, 3, 3, 3, 3, 0, 1, 1, 1, 1, 1},
        {1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
      },
      {
        {201, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 112, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 101, 0, 0, 0, 0, 0, 0, 0, 101, 200, 0, 0},
        {0, 0, 0, 0, 0, 0, 2, 53, 0, 0, 0, 0, 0},
        {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 111, 0, 0, 0, 0, 0, 0, 0},
        {0, 61, 0, 0, 0, 0, 0, 0, 0, 53, 0, 0, 0},
        {0, 110, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 112, 0, 110, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 111, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
      }
    };

    //GameObjectを格納するmap
    //動かないobjectを格納するmap
    public static GameObject[,,] goMap;

    //動くobjectを格納するmap
    public static GameObject[,,] moMap;

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

    public static void MakeMap() {
  		floorSize = goMap.GetLength(0);
  		mapSizeX = goMap.GetLength(1) - 4;
  		mapSizeZ = goMap.GetLength(2) - 4;

  		map = new int[floorSize, mapSizeX, mapSizeZ];

  		for (int i = 0; i < floorSize; i++) {
  			for (int j = 0; j < mapSizeX; j++) {
  				for (int k = 0; k < mapSizeZ; k++) {
  					if (goMap[i,j+2,k+2] != null) {
  						map[i,j,k] = GOtoObjectNum(goMap[i,j+2,k+2]);
  					} else if (moMap[i,j+2,k+2] != null) {
  						map[i,j,k] = GOtoObjectNum(moMap[i,j+2,k+2]);
  					} else {
  						map[i,j,k] = 0;
  					}
  				}
  			}
  		}
  	}
    static int GOtoObjectNum (GameObject obj) {
    		if (obj.name == ("HardBlock(Clone)")) {
    			return 1;
    		} else if (obj.name == "HardPowerBlock(Clone)") {
    			return 2;
    		} else if (obj.name == "HardIceBlock(Clone)") {
    			return 3;
    		} else if (obj.name == "MovableBlock(Clone)") {
    			return 51;
    		} else if (obj.name == "MovableIceBlock(Clone)") {
    			return 53;
    		} else if (obj.name == "MovableLightningBlock(Clone)") {
    			return 61;
    		} else if (obj.name == "Player(Clone)") {
    			return 200;
    		} else if (obj.name == "Doppel(Clone)") {
    			return 201;
    		}
    		return 0;
    	}
  }
}
