using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuCloseButton : MonoBehaviour {
	public GameObject saveMenu;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void CloseMenu() {
		saveMenu.SetActive(false);
	}
}
