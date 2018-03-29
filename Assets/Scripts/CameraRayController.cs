using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class CameraRayController : MonoBehaviour {
  public float distance = 100f;
  private GameObject prev;
	// Use this for initialization
	void Start () {

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
        Debug.Log(obj);
				if (obj.tag.Contains("BaseBlock")) {
					HardObjectController h = obj.GetComponent<HardObjectController>();
					MapPos pos = h.nowPos;
					GameObject.DestroyImmediate(goMap[0, pos.x, pos.z]);
					goMap[0, pos.x, pos.z] = null;
				} else if (obj.tag.Contains("Player")) {
					PlayerController p = obj.GetComponent<PlayerController>();
					MapPos pos = p.nowPos;
					GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
					goMap[pos.floor, pos.x, pos.z] = null;
				} else if (obj.tag.Contains("Doppel")) {
					DoppelController d = obj.GetComponent<DoppelController>();
					MapPos pos = d.nowPos;
					GameObject.DestroyImmediate(goMap[pos.floor, pos.x, pos.z]);
					goMap[pos.floor, pos.x, pos.z] = null;
				}
			}
		}
	}
}
