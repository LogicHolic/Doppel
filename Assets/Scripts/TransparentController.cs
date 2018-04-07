using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class TransparentController : MonoBehaviour {
	private bool transparent = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OnClick() {
		if (transparent) {
			AntiTransparent();
		} else {
			Transparent();
		}
	}

	void Transparent () {
		for (int dz = 0; dz < mapSizeZ; dz++) {
			for (int dx = 0; dx < mapSizeX; dx++) {
				MapPos mapPos = new MapPos(1, dx, dz);
				GameObject obj1 = goMap[mapPos.floor, mapPos.x, mapPos.z];
				GameObject obj2 = moMap[mapPos.floor, mapPos.x, mapPos.z];
				SetTransparent(obj1);
				SetTransparent(obj2);
			}
		}
		transparent = true;
	}

	void SetTransparent(GameObject obj) {
		if (obj != null && obj.tag != "Player" && obj.tag != "Doppel") {
			if (obj.tag.Contains("Lightning") && !obj.tag.Contains("Gate")) {
				Transform basePart = obj.transform.Find("BasePart");
				if (basePart != null) {
					foreach (Transform child in basePart) {
						Renderer rend = child.gameObject.GetComponent<Renderer>();
						Color c = rend.material.GetColor("_Color");
						c.a = 0.25f;
						rend.material.SetColor("_Color",c);
					}
				}
			} else {
				Renderer rend = obj.GetComponent<Renderer>();
				Color c = rend.material.GetColor("_Color");
				c.a = 0.25f;
				rend.material.SetColor("_Color",c);
			}
		}
	}
	void AntiTransparent () {
		for (int dz = 0; dz < mapSizeZ; dz++) {
			for (int dx = 0; dx < mapSizeX; dx++) {
				MapPos mapPos = new MapPos(1, dx, dz);
				GameObject obj1 = goMap[mapPos.floor, mapPos.x, mapPos.z];
				GameObject obj2 = moMap[mapPos.floor, mapPos.x, mapPos.z];
				SetAntiTransparent(obj1);
				SetAntiTransparent(obj2);
			}
		}
		transparent = false;
	}

	void SetAntiTransparent(GameObject obj) {
		if (obj != null && obj.tag != "Player" && obj.tag != "Doppel") {
			if (obj.tag.Contains("Lightning") && !obj.tag.Contains("Gate")) {
				Transform basePart = obj.transform.Find("BasePart");
				if (basePart != null) {
					foreach (Transform child in basePart) {
						Renderer rend = child.gameObject.GetComponent<Renderer>();
						Color c = rend.material.GetColor("_Color");
						c.a = 1;
						rend.material.SetColor("_Color",c);
					}
				}
			} else {
				Renderer rend = obj.GetComponent<Renderer>();
				Color c = rend.material.GetColor("_Color");
				if (obj.tag.Contains("Ice")) {
					rend.material.SetFloat("_Transparency", 0f);
					c.a = 0.6f;
				} else {
					c.a = 1;
				}
				rend.material.SetColor("_Color",c);
			}
		}
	}
}
