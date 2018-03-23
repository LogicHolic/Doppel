using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Game.GameStatic;

public class TextContoroller : MonoBehaviour {
	TextMeshProUGUI tm;
	// Use this for initialization
	void Start () {
 		tm = gameObject.GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update () {
		if(gameOver) {
			tm.text = "GameOver";
		}
	}
}
