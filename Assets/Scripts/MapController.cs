using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapStatic;
using Game;
using static Game.GameStatic;
using System;

public class MapController : MonoBehaviour {
	public GameObject hardBlock;
	public GameObject hardLightningBlock;
	public GameObject movableLightningBlock;
	public GameObject movableBlock;
	public GameObject hardIceBlock;
	public GameObject player;
	public GameObject doppel;
	public GameObject goal;
	public GameObject invisibleBlock;

	void Awake () {
		//player生成
		//今後処理長くなりそうならメソッドにする
		doppels = new List<GameObject>();

		floorSize = map.GetLength(0);
		mapSizeX = map.GetLength(1);
		mapSizeZ = map.GetLength(2);
		ExtendMap();

		floorSize = map.GetLength(0);
		mapSizeX = map.GetLength(1);
		mapSizeZ = map.GetLength(2);
		CreateMap();
	}

	void ExtendMap() {
		int[,,] extendedMap = new int[floorSize, mapSizeX+4, mapSizeZ+4];
		for (int i = 0; i < floorSize; i++) {
			for (int j = 0; j < mapSizeX; j++) {
				for (int k = 0; k < mapSizeZ; k++) {
					extendedMap[i,j+2,k+2] = map[i,j,k];
				}
			}
		}
		map = extendedMap;
	}

	void CreateMap() {
		goMap = new GameObject[floorSize, mapSizeX, mapSizeZ];
		int goalCount= 0;
		for (int floor = 0; floor < floorSize; floor++) {
			for (int dz = 0; dz < mapSizeZ; dz++) {
				for (int dx = 0; dx < mapSizeX; dx++) {
					MapPos mapPos = new MapPos(floor, dx, dz);
					switch (map[floor, dx, dz])
					 {
						case 0:
							break;
						case 1:
							Create("HardBlock", mapPos);
							break;
						case 2:
							Create("HardLightningBlock", mapPos);
							break;
						case 3:
							Create("HardLightningBlock", mapPos,true);
							break;
						case 4:
							Create("MovableBlock", mapPos);
							break;
						case 5:
							Create("MovableLightningBlock", mapPos);
							break;
						case 6:
							Create("MovableLightningBlock", mapPos, true);
							break;
						case 7:
							Create("HardIceBlock", mapPos);
							break;
						case 99:
							Create("Goal", mapPos);
							GoalBlockController g = goMap[floor, dx, dz].GetComponent<GoalBlockController>();
							g.goalNumber = goalCount++;
							break;
						case 100:
							Create("Player", mapPos);
							break;
						case 200:
							Create("Doppel", mapPos);
							doppels.Add(goMap[floor, dx, dz]);
							break;
					}
				}
			}
		}
		doppelNum = doppels.Count;
		goalFlag = new bool[goalCount];
	}

	public void Create(string objName, MapPos mapPos, bool always = false) {
		Vector3 vPos = MapposToUnipos(mapPos);

		if (objName == ("HardBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(hardBlock, vPos, Quaternion.identity);
			HardObjectController h = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<HardObjectController>();
			h.nowPos = mapPos;
		} else if (objName == ("HardLightningBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(hardLightningBlock, vPos, Quaternion.identity);
			HardObjectController h = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<HardObjectController>();
			LightningController l = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<LightningController>();
			if (always) {
				l.lightning = true;
				l.lightningSwitch = true;
				l.always = true;
			}
			h.nowPos = mapPos;
		} else if (objName == ("Player")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(player, vPos-new Vector3(0,0.5f,0), Quaternion.identity);
			PlayerController pc = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<PlayerController>();
			pc.nowPos = mapPos;
		} else if (objName == ("Doppel")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(doppel, vPos-new Vector3(0,0.5f,0), Quaternion.identity);
			DoppelController dc = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<DoppelController>();
			dc.nowPos = mapPos;
		} else if (objName == ("MovableBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(movableBlock, vPos, Quaternion.identity);
			BlockController b = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<BlockController>();
			b.nowPos = mapPos;
		} else if (objName == ("MovableLightningBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(movableLightningBlock, vPos, Quaternion.identity);
			BlockController b = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<BlockController>();
			LightningController l = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<LightningController>();
			if (always) {
				l.lightning = true;
				l.lightningSwitch = true;
				l.always = true;
			}
			b.nowPos = mapPos;
		} else if (objName == ("Goal")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(goal, vPos, Quaternion.identity);
			GoalBlockController g = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<GoalBlockController>();
			g.nowPos = mapPos;
		} else if (objName == ("InvisibleBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(invisibleBlock, vPos, Quaternion.identity);
			InvisibleBlockController inv = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<InvisibleBlockController>();
			inv.nowPos = mapPos;
		} else if (objName == ("HardIceBlock")) {
			goMap[mapPos.floor, mapPos.x, mapPos.z] = Instantiate(hardIceBlock, vPos, Quaternion.identity);
			HardIceBlockController ice = goMap[mapPos.floor, mapPos.x, mapPos.z].GetComponent<HardIceBlockController>();
			ice.nowPos = mapPos;
		}
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
