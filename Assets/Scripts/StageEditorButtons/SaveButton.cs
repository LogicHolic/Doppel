using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

using static Game.MapStatic;
using static Game.GameStatic;

public class SaveButton : MonoBehaviour {
	public string dataNum;
	string userName = "NoName";
	bool cleared;
	string title = "NoTitle";
	string description = "desc";

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Save() {
		StreamWriter sw;
		FileInfo fi;
		string mapPath = Application.dataPath + "/UserMap/";

		if (!Directory.Exists(mapPath)) {
			Directory.CreateDirectory(mapPath);
		}
		fi = new FileInfo(mapPath + "map" + dataNum + ".txt");
		sw = fi.CreateText();
		DateTime dt = DateTime.Now;
		sw.WriteLine(userName);
		sw.WriteLine(dt.ToString("yyyyMMddHHmmss"));
		sw.WriteLine(title);
		sw.WriteLine(description);
		// sw.WriteLine(cleared.ToString());

		MakeMap();
		for (int i = 0; i < floorSize; i++) {
			for (int j = 0; j < mapSizeX; j++) {
				for (int k = 0; k < mapSizeZ; k++) {
					sw.Write(map[i,j,k] + ", ");
				}
				sw.WriteLine();
			}
			sw.WriteLine();
			sw.WriteLine();
		}

		sw.Flush();
		sw.Close();
	}

	//Date
	//Author
	//Cleared
	//Map
}
