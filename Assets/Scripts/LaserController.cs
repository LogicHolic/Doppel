﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.MapStatic;

public class LaserController : MapObject {
	public Material laserMaterial;
	public GameObject laser;
	private Vector3 dv;
	private char laserSurface;
	private char laserDirec;
	private Vector3 unityNowPos;
	private List<SurfacePos> searchedSP;
	private List<GameObject> previousFrameLines;

	struct SurfacePos {
		MapPos pos;
		char surface;
		public SurfacePos(MapPos Pos, char Surface) {
			pos = Pos;
			surface = Surface;
		}
		public static bool operator== (SurfacePos s1, SurfacePos s2)
		{
			return s1.pos == s2.pos && s1.surface == s2.surface;
		}
		public static bool operator!= (SurfacePos s1, SurfacePos s2)
		{
			return !(s1==s2);
		}
	}

	// Use this for initialization
	void Start () {
		laserSurface = 'f';
		laserDirec = 'r';
		Vector3 laserPos = Vector3.zero;
		if (laserSurface == 'f') {
			laserPos = new Vector3(0, 0.5f, 0.5f);
		} else if (laserSurface == 'l') {
			laserPos = new Vector3(-0.5f, 0.5f, 0);
		} else if (laserSurface == 'b') {
			laserPos = new Vector3(0, 0.5f, -0.5f);
		} else {
			laserPos = new Vector3(0.5f, 0.5f, 0);
		}
		GameObject laserObj = Instantiate(laser, laserPos, Quaternion.identity);
		laserObj.transform.parent = transform;
		laserObj.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
		previousFrameLines = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		unityNowPos = transform.position;
		BlockController b = gameObject.GetComponent<BlockController>();
		nowPos = b.nowPos;
		searchedSP = new List<SurfacePos>();

		foreach (GameObject g in previousFrameLines) {
			GameObject.DestroyImmediate(g);
		}
		CalculateTrack();
	}

	void CreateDirection(char direc, char surface, ref Vector3 direc1, ref Vector3 direc2) {
		if (direc == 'l') {
			if (surface == 'f') {
				direc1 = Vector3.forward;
				direc2 = new Vector3(-1,0,1);
			} else if (surface == 'l') {
				direc1 = Vector3.left;
				direc2 = new Vector3(-1, 0, -1);
			} else if (surface == 'b') {
				direc1 = Vector3.back;
				direc2 = new Vector3(1,0,-1);
			} else if (surface == 'r') {
				direc1 = Vector3.right;
				direc2 = new Vector3(1, 0, 1);
			}
		} else if (direc == 'r') {
			if (surface == 'f') {
				direc1 = Vector3.forward;
				direc2 = new Vector3(1,0,1);
			} else if (surface == 'l') {
				direc1 = Vector3.left;
				direc2 = new Vector3(-1,0,1);
			} else if (surface == 'b') {
				direc1 = Vector3.back;
				direc2 = new Vector3(-1,0,-1);
			} else if (surface == 'r') {
				direc1 = Vector3.right;
				direc2 = new Vector3(1,0,-1);
			}
		}
	}

	void FrontReflection(char direc, char surface, ref char newDirec, ref char newSurface) {
		if (surface == 'f') {
			newSurface = 'b';
		} else if (surface == 'l') {
			newSurface = 'r';
		} else if (surface == 'b') {
			newSurface = 'f';
		} else if (surface == 'r') {
			newSurface = 'l';
		}

		if (direc == 'l') {
			newDirec = 'r';
		} else {
			newDirec = 'l';
		}
	}

	void SideReflection(char direc, char surface, ref char newDirec, ref char newSurface) {
		if (direc == 'r') {
			newDirec = 'r';
			if (surface == 'f') {
				newSurface = 'l';
			} else if (surface == 'l') {
				newSurface = 'b';
			} else if (surface == 'b') {
				newSurface = 'r';
			} else if (surface == 'r') {
				newSurface = 'f';
			}
		} else {
			newDirec = 'l';
			if (surface == 'f') {
				newSurface = 'r';
			} else if (surface == 'l') {
				newSurface = 'f';
			} else if (surface == 'b') {
				newSurface = 'l';
			} else if (surface == 'r') {
				newSurface = 'b';
			}
		}
	}

	void CalculateTrack() {
		MapPos pos = nowPos;
		char direcChar = laserDirec;
		char surfaceChar = laserSurface;

		while(true) {
			MapPos newPos = new MapPos();
			char newDirecChar = ' ';
			char newSurfaceChar = ' ';
			if (!Reflection(pos, direcChar, surfaceChar, ref newPos, ref newDirecChar, ref newSurfaceChar)) {
				break;
			}
			pos = newPos;
			direcChar = newDirecChar;
			surfaceChar = newSurfaceChar;
		}
	}

	bool Reflection(MapPos pos, char direcChar, char surfaceChar, ref MapPos newPos, ref char newDirecChar, ref char newSurfaceChar) {
		MapPos currentPos = pos;
		Vector3 direc1 = Vector3.zero, direc2 = Vector3.zero;
		CreateDirection(direcChar, surfaceChar, ref direc1, ref direc2);

		int length = 0;
		while(true) {
			MapPos pos1 = GetNextPos(currentPos, direc1);
			if (pos1.ExceedRange()) {
				CreateInfinityLine(pos, surfaceChar, direc2);
				return false;
			}
			if (LightningObjectCheck(pos1)) {
				FrontReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				if (EnlightenObject(pos1, newSurfaceChar)) {
					CreateLine(pos, surfaceChar, direc2, length);
					return false;
				}
			}
			if (AbsorbObjectCheck(pos1)) {
				FrontReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				CreateLine(pos, surfaceChar, direc2, length);
				return false;
			}
			if (ReflectionObjectCheck(pos1)) {
				FrontReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				CreateLine(pos, surfaceChar, direc2, length);
				if (searchedSP.Contains(new SurfacePos(pos1, newSurfaceChar))) {
					return false;
				}
				newPos = pos1;
				break;
			}
			length++;
			MapPos pos2 = GetNextPos(currentPos, direc2);
			if (pos2.ExceedRange()) {
				CreateInfinityLine(pos, surfaceChar, direc2);
				return false;
			}
			if (LightningObjectCheck(pos2)) {
				SideReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				if (EnlightenObject(pos2, newSurfaceChar)) {
					CreateLine(pos, surfaceChar, direc2, length);
					return false;
				}
			}
			if (AbsorbObjectCheck(pos2)) {
				SideReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				CreateLine(pos, surfaceChar, direc2, length);
				return false;
			}
			if (ReflectionObjectCheck(pos2)) {
				SideReflection(direcChar, surfaceChar, ref newDirecChar, ref newSurfaceChar);
				CreateLine(pos, surfaceChar, direc2, length);
				if (searchedSP.Contains(new SurfacePos(pos2, newSurfaceChar))) {
					return false;
				}
				newPos = pos2;
				break;
			}
			currentPos = pos2;
			length++;
		}
		searchedSP.Add(new SurfacePos(pos, surfaceChar));
		return true;
	}

	bool EnlightenObject(MapPos pos, char surface) {
		Vector3 d = Vector3.zero;
		if (surface == 'f') {
			d = Vector3.forward;
		} else if (surface == 'l') {
			d = Vector3.left;
		} else if (surface == 'b') {
			d = Vector3.back;
		} else if (surface == 'r') {
			d = Vector3.right;
		}
		if (goMap[pos.floor, pos.x, pos.z] != null && goMap[pos.floor, pos.x, pos.z].tag.Contains("Lightning")) {
			LightningController l = goMap[pos.floor, pos.x, pos.z].GetComponent<LightningController>();
			if (l.Accept(d)) {
				l.laserLightning = true;
				return true;
			}
		}
		if (moMap[pos.floor, pos.x, pos.z] != null && moMap[pos.floor, pos.x, pos.z].tag.Contains("Lightning")) {
			LightningController l = moMap[pos.floor, pos.x, pos.z].GetComponent<LightningController>();
			if (l.Accept(d)) {
				l.laserLightning = true;
				return true;
			}
		}
		return false;
	}

	bool LightningObjectCheck(MapPos pos) {
		if (goMap[pos.floor, pos.x, pos.z] != null && goMap[pos.floor, pos.x, pos.z].tag.Contains("Lightning")) {
			return true;
		}
		if (moMap[pos.floor, pos.x, pos.z] != null && moMap[pos.floor, pos.x, pos.z].tag.Contains("Lightning")) {
			return true;
		}
		return false;
	}

	bool ReflectionObjectCheck(MapPos pos) {
		if (goMap[pos.floor, pos.x, pos.z] != null && goMap[pos.floor, pos.x, pos.z].tag.Contains("Reflection")) {
			return true;
		}
		if (moMap[pos.floor, pos.x, pos.z] != null && moMap[pos.floor, pos.x, pos.z].tag.Contains("Reflection")) {
			return true;
		}
		return false;
	}

	bool AbsorbObjectCheck(MapPos pos) {
		if (goMap[pos.floor, pos.x, pos.z] != null && goMap[pos.floor, pos.x, pos.z].tag.Contains("Absorb")) {
			return true;
		}
		if (moMap[pos.floor, pos.x, pos.z] != null && moMap[pos.floor, pos.x, pos.z].tag.Contains("Absorb")) {
			return true;
		}
		return false;
	}

	void CreateLine(MapPos pos, char surface, Vector3 direc, int length) {
		GameObject obj = new GameObject();
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.name = "laserLine";
		LineRenderer line = obj.AddComponent<LineRenderer>();
		previousFrameLines.Add(obj);

		Vector3 firstPos = MapposToUnipos(pos);
		firstPos = ModifyToRealPos(firstPos);
		if (surface == 'f') {
			firstPos += new Vector3(0,0.5f,0.5f);
		} else if (surface == 'l') {
			firstPos += new Vector3(-0.5f,0.5f,0);
		} else if (surface == 'b') {
			firstPos += new Vector3(0,0.5f,-0.5f);
		} else if (surface == 'r') {
			firstPos += new Vector3(0.5f,0.5f,0);
		}
		line.sortingOrder = 1000;
		line.material = laserMaterial;
		// line.useWorldSpace = false;
		line.SetWidth(0.5f, 0.5f);
		line.SetVertexCount(2);
		line.SetPosition(0, firstPos);
		line.SetPosition(1, firstPos + direc * (float)(length * 0.5));
	}

	Vector3 ModifyToRealPos(Vector3 vPos) {
		Vector3 nowVPos = MapposToUnipos(nowPos);
		return vPos + (unityNowPos - nowVPos);
	}

	void CreateInfinityLine(MapPos pos, char surface, Vector3 direc) {
		GameObject obj = new GameObject();
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.name = "laserLine";
		LineRenderer line = obj.AddComponent<LineRenderer>();
		previousFrameLines.Add(obj);

		Vector3 firstPos = MapposToUnipos(pos);
		firstPos = ModifyToRealPos(firstPos);
		if (surface == 'f') {
			firstPos += new Vector3(0,0.5f,0.5f);
		} else if (surface == 'l') {
			firstPos += new Vector3(-0.5f,0.5f,0);
		} else if (surface == 'b') {
			firstPos += new Vector3(0,0.5f,-0.5f);
		} else if (surface == 'r') {
			firstPos += new Vector3(0.5f,0.5f,0);
		}
		line.sortingOrder = 1000;
		line.material = laserMaterial;
		// line.useWorldSpace = false;
		line.SetWidth(0.5f, 0.5f);
		line.SetVertexCount(2);
		line.SetPosition(0, firstPos);
		line.SetPosition(1, firstPos + direc * 1000);
	}

	void CreateLines(Vector3[] vertices) {
		for (int i = 0; i < vertices.Length - 1; i++) {
			GameObject obj = new GameObject();
			obj.transform.parent = transform;
			obj.transform.localPosition = new Vector3(0,0.5f,0.5f);
			LineRenderer line = obj.AddComponent<LineRenderer>();

			line.sortingOrder = 1000;
			line.material = laserMaterial;
			line.useWorldSpace = false;
			line.SetWidth(0.5f, 0.5f);
			line.SetVertexCount(2);
			line.SetPosition(0, vertices[i]);
			line.SetPosition(1, vertices[i + 1]);
		}
	}
}
