using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour {

	public string actionName;
	public Text text;
	// Use this for initialization
	void Start () {
		text.text = actionName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectedAsAction(){
		Debug.Log (actionName+" was selected as an action.");
	}
	public void SelectedAsBonusAction(){
		Debug.Log (actionName + " was selected as a bonus action.");
	}
}
