using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

using static Game.MapStatic;

public class StageManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void GoStage( int stageNum ) {
		LoadMap(stageNum);
		SceneManager.LoadScene("GameMain");
	}

	void LoadMap(int stageNum) {
		string filePath = String.Format("Maps/{0:000}", stageNum);
		TextAsset mapTextAsset = Resources.Load(filePath) as TextAsset;
		string[] mapText = Regex.Split(mapTextAsset.text,@"\D+");

		floorSize = int.Parse(mapText[0]);
		mapSizeX = int.Parse(mapText[1]);
		mapSizeZ = int.Parse(mapText[2]);
		// for (int i = 0; i < mapText.Length; i++) {
		// 	Debug.Log(mapText[i]);
		// }

		map = new int[floorSize, mapSizeX, mapSizeZ];
		int index = 3;
		for (int i = 0; i < floorSize; i++) {
			for (int j = 0; j < mapSizeX; j++) {
				for (int k = 0; k < mapSizeZ; k++) {
					if (index < mapText.Length) {
						int result;
						if (int.TryParse(mapText[index], out result)) {
							map[i,j,k] = result;
						}
						index++;
					}
				}
			}
		}
	}
}
