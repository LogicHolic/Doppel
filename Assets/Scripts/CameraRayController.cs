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
	// Use this for initialization
	void Start () {
    se = stageEditor.GetComponent<StageEditor>();
    mc = mapController.GetComponent<MapController>();
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
				if (obj.tag == ("BaseBlock")) {
					HardObjectController h = obj.GetComponent<HardObjectController>();
					MapPos pos = h.nowPos;
					Replace(pos);
				} else if (obj.tag == ("Player")) {
					PlayerController p = obj.GetComponent<PlayerController>();
					MapPos pos = p.nowPos;
					Replace(pos);
				} else if (obj.tag == ("Doppel")) {
					DoppelController d = obj.GetComponent<DoppelController>();
					MapPos pos = d.nowPos;
					Replace(pos);
				} else if (obj.tag == ("Movable,Lightning")) {
          BlockController b = obj.GetComponent<BlockController>();
          MapPos pos = b.nowPos;
          Replace(pos);
        } else if (obj.tag == "Invisible") {
          InvisibleBlockController inv = obj.GetComponent<InvisibleBlockController>();
          MapPos pos = inv.nowPos;
          Replace(pos);
        } else if (obj.name == ("NullSelector")) {
          se.replaceState = "null";
        } else if (obj.name == ("BaseBlockSelector")) {
          se.replaceState = "BaseBlock";
        } else if (obj.name == ("MovableLightningBlockSelector")) {
          se.replaceState = "MovableLightningBlock";
        } else if (obj.name == ("MovableBlockSelector")) {
          se.replaceState = "MovableBlock";
        }
			}
		}
	}

  void Replace(MapPos pos) {
    if (se.replaceState == "null") {
      GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
      if (pos.floor == 0) {
        mc.Create("InvisibleBlock", pos);
      }
    }
    else if (se.replaceState == "BaseBlock") {
      if (pos.floor == 0) {
        if (goMap[pos.floor, pos.x, pos.z] == null || goMap[pos.floor, pos.x, pos.z].tag == "Invisible") {
          mc.Create("BaseBlock", pos);
        } else {
          pos.floor += 1;
          mc.Create("BaseBlock", pos);
        }
      } else {
        GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
        mc.Create("BaseBlock", pos);
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
        mc.Create("MovableBlock", pos, true);
      }
    }
  }
}
