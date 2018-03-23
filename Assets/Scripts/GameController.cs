using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using static Game.GameStatic;

public class GameController : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (gameOver) {
			GameOver();
		}
	}
	void GameOver(){
		
	}
}
