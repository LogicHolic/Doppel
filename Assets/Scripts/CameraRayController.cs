using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class CameraRayController : MonoBehaviour {
  public float distance = 100f;
  public GameObject stageEditor;
  public GameObject mapController;
  private GameObject prev;
  private StageEditor se;
  private MapController mc;
  private MapPos playerPos;
	// Use this for initialization
	void Start () {
    se = stageEditor.GetComponent<StageEditor>();
    mc = mapController.GetComponent<MapController>();
    playerPos = new MapPos(1,2,2);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			// クリックしたスクリーン座標をrayに変換
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Rayの当たったオブジェクトの情報を格納する
			RaycastHit hit = new RaycastHit();
			// オブジェクトにrayが当たった時
			if (Physics.Raycast(ray, out hit, distance)) {
				GameObject obj = hit.collider.gameObject;
				if (obj.name == ("HardBlock(Clone)")) {
					HardObjectController h = obj.GetComponent<HardObjectController>();
					MapPos pos = h.nowPos;
					Replace(pos);
				} else if (obj.name == ("Player(Clone)")) {
					PlayerController p = obj.GetComponent<PlayerController>();
					MapPos pos = p.nowPos;
					Replace(pos);
				} else if (obj.name == ("Doppel(Clone)")) {
					DoppelController d = obj.GetComponent<DoppelController>();
					MapPos pos = d.nowPos;
					Replace(pos);
				} else if (obj.name == ("MovableLightningBlock(Clone)")) {
          BlockController b = obj.GetComponent<BlockController>();
          MapPos pos = b.nowPos;
          Replace(pos);
        } else if (obj.name == "InvisibleBlock(Clone)") {
          InvisibleBlockController inv = obj.GetComponent<InvisibleBlockController>();
          MapPos pos = inv.nowPos;
          Replace(pos);
        } else if (obj.name == ("NullSelector")) {
          se.replaceState = "null";
        } else if (obj.name == ("PlayerSelector")) {
          se.replaceState = "Player";
        } else if (obj.name == ("DoppelSelector")) {
          se.replaceState = "Doppel";
        } else if (obj.name == ("HardPowerBlockSelector")) {
          se.replaceState = "HardPowerBlock";
        } else if (obj.name == ("HardBlockSelector")) {
          se.replaceState = "HardBlock";
        } else if (obj.name == ("MovableBlockSelector")) {
          se.replaceState = "MovableBlock";
        } else if (obj.name == ("IceBlockSelector")) {
          se.replaceState = "IceBlock";
        } else if (obj.name == ("MovableLightningBlockSelector")) {
          se.replaceState = "MovableLightningBlock";
        }
			}
		}
	}

  void Replace(MapPos pos) {
    if (se.replaceState == "null") {
      GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
      GameObject.DestroyImmediate(moMap[pos.floor, pos.x, pos.z]);
      if (pos.floor == 0) {
        mc.Create("InvisibleBlock", pos);
      }
    }
    else if (se.replaceState == "Player") {
      if (pos.floor == 1) {
        GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
        GameObject.DestroyImmediate(moMap[pos.floor, pos.x, pos.z]);
        GameObject.DestroyImmediate(moMap[playerPos.floor, playerPos.x, playerPos.z]);
        mc.Create("Player", pos);
        playerPos = pos;
      } else if (goMap[pos.floor, pos.x, pos.z] != null && goMap[pos.floor, pos.x, pos.z].name == "HardBlock(Clone)") {
        pos.floor += 1;
        GameObject.DestroyImmediate(moMap[playerPos.floor, playerPos.x, playerPos.z]);
        mc.Create("Player", pos);
        playerPos = pos;
      }
    }
    else if (se.replaceState == "HardBlock") {
      if (pos.floor == 0) {
        if (goMap[pos.floor, pos.x, pos.z] == null || goMap[pos.floor, pos.x, pos.z].tag == "Invisible") {
          mc.Create("HardBlock", pos);
        } else {
          pos.floor += 1;
          mc.Create("HardBlock", pos);
        }
      } else {
        GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
        GameObject.DestroyImmediate(moMap[pos.floor, pos.x, pos.z]);
        mc.Create("HardBlock", pos);
      }
    } else if (se.replaceState == "MovableBlock") {
      if (pos.floor == 0) {
        if (goMap[pos.floor, pos.x, pos.z] == null || goMap[pos.floor, pos.x, pos.z].tag == "Invisible") {
          mc.Create("MovableBlock", pos);
        } else {
          pos.floor += 1;
          mc.Create("MovableBlock", pos);
        }
      } else {
        GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
        GameObject.DestroyImmediate(moMap[pos.floor, pos.x, pos.z]);
        mc.Create("MovableBlock", pos);
      }
    } else if (se.replaceState == "MovableLightningBlock") {
      if (pos.floor == 0) {
        if (goMap[pos.floor, pos.x, pos.z] == null || goMap[pos.floor, pos.x, pos.z].tag == "Invisible") {
          mc.Create("MovableBlock", pos, true);
        } else {
          pos.floor += 1;
          mc.Create("MovableBlock", pos, true);
        }
      } else {
        GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
        GameObject.DestroyImmediate(moMap[pos.floor, pos.x, pos.z]);
        mc.Create("MovableBlock", pos, true);
      }
    }
  }
}
