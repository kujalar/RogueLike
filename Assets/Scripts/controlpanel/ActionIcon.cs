using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour {

	//public string actionName;
	public Actionometer actionometer;
	public ActionOption actionOption;
	public Text text;

	public void Init(ActionOption actionOption,Actionometer actionometer){
		this.actionometer = actionometer;
		this.actionOption = actionOption;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SelectedAsAction(){
		actionometer.DoAction(actionOption);
	}
	public void SelectedAsBonusAction(){
		actionometer.DoBonusAction (actionOption);
	}
}
