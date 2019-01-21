using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuButton : MonoBehaviour {
	public GameObject saveMenu;
	// Use this for initialization
	void Start () {
		saveMenu.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void OpenMenu() {
		saveMenu.SetActive(true);
	}
}
