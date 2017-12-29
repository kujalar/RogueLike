using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionHolder : MonoBehaviour {
	public Text actionText;
	public Image actionImage;
	// Use this for initialization
	void Awaken () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSymbolColor(Color color){
		actionImage.color = color;
		
	}

	public void SetText(string text){
		actionText.text = text;
	}
}
