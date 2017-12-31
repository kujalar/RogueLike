using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour {
	public GameObject actionButton;
	public GameObject bonusActionButton;
	//public string actionName;
	[HideInInspector] public Actionometer actionometer;
	[HideInInspector] public ActionOption actionOption;
	public Text text;

	public void Init(ActionOption actionOption,Actionometer actionometer){
		this.actionometer = actionometer;
		this.actionOption = actionOption;
		actionButton.SetActive (actionOption.isAction);
		bonusActionButton.SetActive (actionOption.isBonusAction);
		this.text.text = actionOption.action.GetName ();
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
