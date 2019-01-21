using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static Game.MapStatic;
using static Game.GameStatic;

public class GameManagerButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Retry() {
		for (int i = 0; i < floorSize; i++) {
			for (int j = 0; j < mapSizeX; j++) {
				for (int k = 0; k < mapSizeZ; k++) {
					if (goMap[i,j,k] != null) {
						GameObject.DestroyImmediate(goMap[i,j,k]);
					}
					if (moMap[i,j,k] != null) {
						GameObject.DestroyImmediate(moMap[i,j,k]);
					}
				}
			}
		}
		GameObject mapController = GameObject.Find("MapController");
		MapController mc = mapController.GetComponent<MapController>();
		mc.CreateMap();
		gameOver = false;
	}
}
