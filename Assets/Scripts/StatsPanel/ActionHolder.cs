using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionHolder : MonoBehaviour {
	public Text actionText;
	// Use this for initialization
	void Awaken () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setText(string text){
		actionText.text = text;
	}
}
