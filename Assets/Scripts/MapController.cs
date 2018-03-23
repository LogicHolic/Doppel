using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapStatic;
using Game;

public class MapController : MonoBehaviour {
	public GameObject baseBlock;
	public GameObject movableBlock;
	public GameObject player;
	public GameObject goal;

	// Use this for initialization
	void Start () {
		//player生成
		//今後処理長くなりそうならメソッドにする
		playerPos = new MapPos(1, 5, 10);
		GameObject p = Instantiate(player, MapposToUnipos(playerPos) - new Vector3(0, 0.5f, 0), Quaternion.identity);
		PlayerController pc = p.GetComponent<PlayerController>();
		pc.nowPos = playerPos;
		CreateMap();
	}

	void CreateMap() {
		for (int floor = 0; floor < 2; floor++) {
			for (int dz = 0; dz < mapSizeZ; dz++) {
				for (int dx = 0; dx < mapSizeX; dx++) {
					MapPos mapPos = new MapPos(floor, dx, dz);
					Vector3 Pos = MapposToUnipos(mapPos);
					switch (map[floor, dx, dz])
					 {
						case 0:
							goMap[floor, dx, dz] = null;
							break;
						case 1:
							goMap[floor, dx, dz] = Instantiate(baseBlock, Pos, Quaternion.identity);
							break;
						case 2:
							goMap[floor, dx, dz] = Instantiate(movableBlock, Pos, Quaternion.identity);
							BlockController b2 = goMap[floor, dx, dz].GetComponent<BlockController>();
							b2.nowPos = mapPos;
							break;
						case 3:
							goMap[floor, dx, dz] = Instantiate(movableBlock, Pos, Quaternion.identity);
							BlockController b3 = goMap[floor, dx, dz].GetComponent<BlockController>();
							b3.nowPos = mapPos;
							b3.lightning = true;
							break;
						case 99:
							goMap[floor, dx, dz] = Instantiate(goal, Pos, Quaternion.identity);
							break;
					}
				}
			}
		}
	}

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

	// Update is called once per frame
	void Update () {

	}
}
