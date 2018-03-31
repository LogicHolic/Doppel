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
				GameObject obj = goMap[mapPos.floor, mapPos.x, mapPos.z];
				if (obj != null && obj.tag != "Player" && obj.tag != "Doppel") {
					if (obj.tag.Contains("Lightning")) {
						Transform basePart = obj.transform.Find("BasePart");
						foreach (Transform child in basePart) {
							Renderer rend = child.gameObject.GetComponent<Renderer>();
							rend.material.SetColor("_Color",new Color(1,1,1,0.25f));
						}
					} else {
						Renderer rend = obj.GetComponent<Renderer>();
						rend.material.SetColor("_Color",new Color(1,1,1,0.25f));
					}
				}
			}
		}
		transparent = true;
	}
	void AntiTransparent () {
		for (int dz = 0; dz < mapSizeZ; dz++) {
			for (int dx = 0; dx < mapSizeX; dx++) {
				MapPos mapPos = new MapPos(1, dx, dz);
				GameObject obj = goMap[mapPos.floor, mapPos.x, mapPos.z];
				if (obj != null && obj.tag != "Player" && obj.tag != "Doppel") {
					if (obj.tag.Contains("Lightning")) {
						Transform basePart = obj.transform.Find("BasePart");
						foreach (Transform child in basePart) {
							Renderer rend = child.gameObject.GetComponent<Renderer>();
							rend.material.SetColor("_Color",new Color(1,1,1,1));
						}
					} else {
						Renderer rend = obj.GetComponent<Renderer>();
						rend.material.SetColor("_Color",new Color(1,1,1,1));
					}
				}
			}
		}
		transparent = false;
	}
}
