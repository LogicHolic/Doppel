using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorText : MonoBehaviour {
	private Text replaceStateText;
	private StageEditor st;

	// Use this for initialization
	void Start () {
		replaceStateText = GetComponent<Text>();
		GameObject obj = GameObject.Find("StageEditor");
		st = obj.GetComponent<StageEditor>();
	}

	// Update is called once per frame
	void Update () {
		replaceStateText.text = st.replaceState;
	}
}
