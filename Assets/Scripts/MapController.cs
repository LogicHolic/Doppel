using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapStatic;
using Game;
using static Game.GameStatic;
using System;

public class MapController : MonoBehaviour {
	public GameObject baseBlock;
	public GameObject movableBlock;
	public GameObject iceBlock;
	public GameObject player;
	public GameObject doppel;
	public GameObject goal;
	public GameObject invisibleBlock;

	void Awake () {
		//player生成
		//今後処理長くなりそうならメソッドにする
		playerPos = new MapPos(1, 5, 10);
		doppelPos = new MapPos[doppelNum];

		floorSize = map.GetLength(0);
		mapSizeX = map.GetLength(1);
		mapSizeZ = map.GetLength(2);

		doppelPos[0] = new MapPos(1, 12, 6);
		GameObject d = Instantiate(doppel, MapposToUnipos(doppelPos[0]) - new Vector3(0, 0.5f, 0), Quaternion.identity);
		DoppelController dc = d.GetComponent<DoppelController>();
		dc.nowPos = doppelPos[0];
		dc.number = 0;

		doppelPos[1] = new MapPos(1, 10, 8);
		d = Instantiate(doppel, MapposToUnipos(doppelPos[1]) - new Vector3(0, 0.5f, 0), Quaternion.identity);
		dc = d.GetComponent<DoppelController>();
		dc.nowPos = doppelPos[1];
		dc.number = 1;

		doppelPos[2] = new MapPos(1, 3, 12);
		d = Instantiate(doppel, MapposToUnipos(doppelPos[2]) - new Vector3(0, 0.5f, 0), Quaternion.identity);
		dc = d.GetComponent<DoppelController>();
		dc.nowPos = doppelPos[2];
		dc.number = 2;

		GameObject p = Instantiate(player, MapposToUnipos(playerPos) - new Vector3(0, 0.5f, 0), Quaternion.identity);
		PlayerController pc = p.GetComponent<PlayerController>();
		pc.nowPos = playerPos;
		CreateMap();
	}


	void CreateMap() {
		goMap = new GameObject[floorSize, mapSizeX, mapSizeZ];
		int goalNum = 0;
		for (int floor = 0; floor < floorSize; floor++) {
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
							HardObjectController h = goMap[floor, dx, dz].GetComponent<HardObjectController>();
							h.nowPos = mapPos;
							break;
						case 2:
							goMap[floor, dx, dz] = Instantiate(movableBlock, Pos, Quaternion.identity);
							BlockController b2 = goMap[floor, dx, dz].GetComponent<BlockController>();
							LightningController l1 = goMap[floor, dx, dz].GetComponent<LightningController>();
							l1.lightning = false;
							b2.nowPos = mapPos;
							break;
						case 3:
							goMap[floor, dx, dz] = Instantiate(movableBlock, Pos, Quaternion.identity);
							BlockController b3 = goMap[floor, dx, dz].GetComponent<BlockController>();
							LightningController l2 = goMap[floor, dx, dz].GetComponent<LightningController>();
							b3.nowPos = mapPos;
							l2.lightning = true;
							break;
						case 4:
							goMap[floor, dx, dz] = Instantiate(iceBlock, Pos, Quaternion.identity);
							break;
						// case 20:
						// 	goMap[floor, dx, dz] = Instantiate(invisibleBlock, Pos, Quaternion.identity);
						// InvisibleBlockController iCon = goMap[floor, dx, dz].GetComponent<InvisibleBlockController>();
						// iCon.nowPos = mapPos;
						// 	break;
						case 99:
							goMap[floor, dx, dz] = Instantiate(goal, Pos, Quaternion.identity);
							GoalBlockController g = goMap[floor, dx, dz].GetComponent<GoalBlockController>();
							LightningController l3 = goMap[floor, dx, dz].GetComponent<LightningController>();
							l3.lightning = false;
							g.nowPos = mapPos;
							g.goalNumber = goalNum++;
							break;
						case 100:

							break;
					}
				}
			}
		}
		goalFlag = new bool[goalNum];
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
		if (GoalJudge()) {
			stageClear = true;
		}
	}

	private bool GoalJudge() {
		for (int i = 0; i < goalFlag.Length; i++) {
			if (!goalFlag[i]) {
				return false;
			}
		}
		return true;
	}
}
