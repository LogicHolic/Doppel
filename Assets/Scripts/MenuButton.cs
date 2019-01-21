using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {
	public GameObject menu;
	// Use this for initialization
	void Start () {
		menu.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void OpenMenu() {
		menu.SetActive(true);
	}
}
